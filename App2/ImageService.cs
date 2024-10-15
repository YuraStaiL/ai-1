using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App2
{
    internal class ImageService
    {
        TextBoxBase? logTextBox = null;

        public ImageService(TextBoxBase? logTextBox = null)
        {
            this.logTextBox = logTextBox;
        }

        public List<GraphicsPath> DivideImageIntoSectors(int numSectors, Size imageSize)
        {
            List<GraphicsPath> sectors = new List<GraphicsPath>();

            // Define the top-right corner (origin point for the sectors)
            Point topRight = new Point(imageSize.Width - 1, 0);

            // Calculate the angle step for the number of sectors (90 degrees)
            float angleStep = 90f / numSectors;
            bool isHaveLeftCornerPoint = false;
            // Create a list of points that will define the boundary lines between sectors
            List<Point> sectorPoints = new List<Point>();
            sectorPoints.Add(new Point(imageSize.Width - 1, imageSize.Height));  // Start from the bottom-right corner

            for (int i = 1; i < numSectors; i++)
            {
                bool defaultHaveLeftCornerPoint = isHaveLeftCornerPoint;
                // Calculate the angle for the current sector boundary
                float angle = i * angleStep;

                // Calculate the intersection of the boundary with the image borders
                Point endPoint = CalculateIntersectionWithImageBorder(angle, imageSize.Width, imageSize.Height, ref isHaveLeftCornerPoint);
                if (defaultHaveLeftCornerPoint != isHaveLeftCornerPoint)
                {
                    sectorPoints.Add(new Point(0, imageSize.Height));
                }
                else
                {
                    sectorPoints.Add(endPoint);
                }

                // Add the intersection point to the list
                sectorPoints.Add(endPoint);
            }

            if (!isHaveLeftCornerPoint)
            {
                sectorPoints.Add(new Point(0, imageSize.Height));
            }

            // Finally, add the bottom-left corner of the image to close the last sector
            sectorPoints.Add(new Point(0, 0));
            sectorPoints.Add(new Point(0, 0));

            // Now create each sector by connecting the top-right corner to two consecutive points in the list
            for (int i = 0; i < sectorPoints.Count - 2; i = i + 2)
            {
                GraphicsPath sectorPath = new GraphicsPath();
                sectorPath.StartFigure();
                sectorPath.AddLine(topRight, sectorPoints[i]);    // Line from top-right to the first point
                sectorPath.AddLine(sectorPoints[i], sectorPoints[i + 1]);  // Line between two consecutive points
                sectorPath.AddLine(sectorPoints[i + 1], sectorPoints[i + 2]);  // Line between two consecutive points
                sectorPath.AddLine(sectorPoints[i + 2], topRight);  // Line back to top-right
                sectorPath.CloseFigure();
                sectors.Add(sectorPath);
            }

            return sectors;
        }

        public Point CalculateIntersectionWithImageBorder(float angle, int imageWidth, int imageHeight, ref bool isHaveLeftCornerPoint)
        {
            // Convert the angle from degrees to radians for calculation
            double radians = angle * Math.PI / 180.0;

            // Determine the end point based on which border (bottom or left) the line intersects
            if (radians == Math.PI / 2)  // Special case: if the angle is exactly 90 degrees, the line is vertical
            {
                return new Point(0, imageHeight - 1); // Bottom-left corner
            }
            else
            {
                // Calculate x and y distances based on trigonometry
                double tan = Math.Tan(radians);

                // Calculate the intersection with the bottom edge of the image
                int xBottom = (int)(imageHeight * tan);
                if (xBottom <= imageWidth - 1)  // If the intersection is within the width of the image
                {
                    return new Point(imageWidth - 1 - xBottom, imageHeight - 1);
                }
                else if (!isHaveLeftCornerPoint)
                {
                    isHaveLeftCornerPoint = true;
                }

                // Otherwise, calculate the intersection with the left edge of the image
                int yLeft = (int)((imageWidth - 1) / tan);
                return new Point(0, yLeft);
            }
        }

        public void DrawSectorLines(Bitmap image, int numSectors)
        {
            // Create a graphics object from the image
            using (Graphics g = Graphics.FromImage(image))
            {
                // Set the pen color and thickness for drawing
                Pen pen = new Pen(Color.Red, 2);

                // Define the starting point (top-right corner of the image)
                Point topRight = new Point(image.Width - 1, 0);
                bool isHaveLeftCornerPoint = false;
                // Calculate the angle step for the number of sectors
                float angleStep = 90f / numSectors;

                for (int i = 1; i < numSectors; i++)
                {
                    // Calculate the angle for the current line (in degrees)
                    float angle = i * angleStep;

                    // Calculate the intersection of the line with the image borders
                    Point endPoint = CalculateIntersectionWithImageBorder(angle, image.Width, image.Height, ref isHaveLeftCornerPoint);

                    // Draw the line from the top-right corner to the calculated intersection point
                    g.DrawLine(pen, topRight, endPoint);
                }
            }
        }
        public int CountBlackPixelsInSector(Bitmap image, GraphicsPath sector)
        {
            int blackPixelCount = 0;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (sector.IsVisible(x, y))
                    {
                        Color pixelColor = image.GetPixel(x, y);

                        if (pixelColor.ToArgb() == Color.Black.ToArgb())
                        {
                            blackPixelCount++;
                        }
                    }
                }
            }

            return blackPixelCount;
        }
        public void outToLog(string output)
        {
            if (logTextBox == null) {
                return;
            }

            logTextBox.AppendText("\r\n" + output);
            logTextBox.ScrollToCaret();
        }

        public int[] getBlackPixels(int numSectors, Bitmap bwImage)
        {
            var sectors = DivideImageIntoSectors(numSectors, bwImage.Size);
            Random r = new Random();
            Color[] sectorColors = { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta };
            int[] blackPixels = new int[sectors.Count];
            for (int i = 0; i < sectors.Count; i++)
            {
                int blackPixelsCnt = CountBlackPixelsInSector(bwImage, sectors[i]);
                blackPixels[i] = blackPixelsCnt;
            }

            return blackPixels;
        }

        public float[] getSumNormalizeVector(int[] blackPixels, int size)
        {
            float[] sumNormalize = new float[size];
            int sum = blackPixels.Sum();

            for (int i = 0; i < size; i++)
            {
                sumNormalize[i] = (float)blackPixels[i] / sum;
            }

            return sumNormalize;
        }

        public float[] getMaxNormalizeVector(int[] blackPixels, int size)
        {
            float[] maxNormalize = new float[size];
            int maxValue = blackPixels.Max();

            for (int i = 0; i < size; i++)
            {
                maxNormalize[i] = (float)blackPixels[i] / maxValue;
            }

            return maxNormalize;
        }

        public float[] getMaxComponentsVector(float[,] normalizeVectorsDict)
        {
            int sectorCnt = normalizeVectorsDict.GetUpperBound(1) + 1;
            float[] max = new float[sectorCnt];

            for (int columnIndex = 0; columnIndex < sectorCnt; columnIndex++)
            {
                float[] column = new float[normalizeVectorsDict.GetUpperBound(0) + 1];
                for (int row = 0; row < normalizeVectorsDict.GetUpperBound(0) + 1; row++)
                {
                    column[row] = normalizeVectorsDict[row, columnIndex];
                }
                max[columnIndex] = column.Max();
            }

            return max;
        }

        public float[] getMinComponentsVector(float[,] normalizeVectorsDict)
        {
            int sectorCnt = normalizeVectorsDict.GetUpperBound(1) + 1;
            float[] min = new float[sectorCnt];

            for (int columnIndex = 0; columnIndex < sectorCnt; columnIndex++)
            {
                float[] column = new float[normalizeVectorsDict.GetUpperBound(0) + 1];
                for (int row = 0; row < normalizeVectorsDict.GetUpperBound(0) + 1; row++)
                {
                    column[row] = normalizeVectorsDict[row, columnIndex];
                }
                min[columnIndex] = column.Min();
            }

            return min;
        }

        //public Bitmap ProcessImageAndDrawSectors(int numSectors, Bitmap bwImage, string className)
        //{
        //    string vector = String.Join("; ", blackPixels);
        //    vector = $"{className} - vector: ({vector})";

        //    outToLog(vector);

        //    string sumNormalizeVector = String.Join("; ", sumNormalize);
        //    sumNormalizeVector = $"{className} - FarynaS1 - normalize by sum: ({sumNormalizeVector})";

        //    //outToLog(sumNormalizeVector);


        //    string maxNormalizeVector = String.Join("; ", maxNormalize);
        //    maxNormalizeVector = $"{className} - FarynaM1 - normalize by max: ({maxNormalizeVector})";

        //    //outToLog(maxNormalizeVector);

        //    return bwImage;
        //}

        public Bitmap bw(Image image, int threshold)
        {
            // Перетворення зображення в сірий колір
            Bitmap bmp = new Bitmap(image);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    // Отримуємо піксель
                    Color pixelColor = bmp.GetPixel(x, y);

                    // Формула для перетворення в сірий (напівтоновий) колір
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));

                    // Перетворюємо піксель у сірий колір
                    Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);
                    bmp.SetPixel(x, y, grayColor);
                }
            }

            // Другий етап: чорно-біле перетворення на основі порогу
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    // Отримуємо піксель і його яскравість (вже сірий)
                    Color grayColor = bmp.GetPixel(x, y);
                    int brightness = grayColor.R;  // Сірі пікселі мають однакові значення R, G, B

                    // Порівнюємо яскравість з порогом і перетворюємо на чорний або білий
                    if (brightness < threshold)
                        bmp.SetPixel(x, y, Color.Black);
                    else
                        bmp.SetPixel(x, y, Color.White);
                }
            }

            return bmp;
        }
    }
}
