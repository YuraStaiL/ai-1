
using System.Drawing.Drawing2D;
using System.Linq;

namespace App2
{
    public partial class Form1 : Form
    {
        private Rectangle cropRect;
        private bool isCropping = false;
        private Point startPoint;
        private MemoryStream ms;
        private Image OriginalImg;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Завантаження зображення
                    Image img = Image.FromFile(ofd.FileName);
                    OriginalImg = img;
                    pictureBox1.Image = img;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        private void btnRestoreOriginalImg_Click(object sender, EventArgs e)
        {
            // Завантаження зображення
            pictureBox1.Image = OriginalImg;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null)
                return;

            pictureBox1.Invalidate();
            isCropping = true;
            startPoint = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCropping)
            {
                Point currentPoint = e.Location;
                cropRect = new Rectangle(
                    Math.Min(startPoint.X, currentPoint.X),
                    Math.Min(startPoint.Y, currentPoint.Y),
                    Math.Abs(startPoint.X - currentPoint.X),
                    Math.Abs(startPoint.Y - currentPoint.Y)
                );

                pictureBox1.Invalidate(); // Оновлюємо зображення для малювання прямокутника
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (isCropping && pictureBox1.Image != null)
            {
                using (Pen cropPen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(cropPen, cropRect);
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isCropping = false;

            if (cropRect.Width > 0 && cropRect.Height > 0)
            {
                CropImage();
            }
        }

        private void CropImage()
        {
            if (pictureBox1.Image != null)
            {
                // Convert selection rectangle from PictureBox coordinates to image coordinates
                var image = pictureBox1.Image;
                var scaleX = (float)image.Width / pictureBox1.Width;
                var scaleY = (float)image.Height / pictureBox1.Height;
                var newImageRec = new Rectangle(
                    (int)(cropRect.X * scaleX),
                    (int)(cropRect.Y * scaleY),
                    (int)(cropRect.Width * scaleX),
                    (int)(cropRect.Height * scaleY));

                // Crop the image
                Bitmap croppedImage = new Bitmap(newImageRec.Width, newImageRec.Height);
                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.DrawImage(image, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), newImageRec, GraphicsUnit.Pixel);
                }

                // Replace the old image with the cropped image
                pictureBox1.Image = croppedImage;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("No image loaded!");
                return;
            }

            // Перевіряємо, чи введене порогове значення є числом
            int threshold;
            if (!int.TryParse(txtThreshold.Text, out threshold) || threshold < 0 || threshold > 255)
            {
                MessageBox.Show("Please enter a valid threshold value between 0 and 255.");
                return;
            }

            // Перетворення зображення в сірий колір
            Bitmap bmp = new Bitmap(pictureBox1.Image);
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

            // Відображаємо перетворене зображення в PictureBox
            pictureBox1.Image = bmp;
        }

        private void buttonSegment_Click(object sender, EventArgs e)
        {
            int numSectors = 0;
            int.TryParse(sectorsNumber.Text, out numSectors);

            ProcessImageAndDrawSectors(numSectors);
        }

        private void buttonFillSegment_Click(object sender, EventArgs e)
        {
            int numSectors = 0;
            int.TryParse(sectorsNumber.Text, out numSectors);

            ProcessFillDrawSectors(numSectors);
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
            for (int i = 0; i < sectorPoints.Count - 2; i=i+2)
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
                } else if (!isHaveLeftCornerPoint) {
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

        public void findSectorsPoints(Bitmap image, int numSectors)
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

        void outToLog(string output)
        {
            logRichTextBox.AppendText("\r\n" + output);
            logRichTextBox.ScrollToCaret();
        }

        private void ProcessImageAndDrawSectors(int numSectors)
        {

            // Convert it to grayscale (if necessary)
            Bitmap grayscaleImage = new Bitmap(pictureBox1.Image);

            // Draw sector lines on the grayscale image
            DrawSectorLines(grayscaleImage, numSectors);

            // Update the pictureBox with the modified image
            pictureBox1.Image = grayscaleImage;

            // Optional: process sectors to count black pixels
            var sectors = DivideImageIntoSectors(numSectors, grayscaleImage.Size);
            Random r = new Random();
            Color[] sectorColors = { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta };
            int[] blackPixels = new int[sectors.Count];
            for (int i = 0; i < sectors.Count; i++)
            {

                int blackPixelsCnt = CountBlackPixelsInSector(grayscaleImage, sectors[i]);
                blackPixels[i] = blackPixelsCnt;
            }

            string vector = String.Join(", ", blackPixels);
            vector = $"vector: ({vector})";

            outToLog(vector);
            int sum = blackPixels.Sum();
            float[] sumNormalize = new float[sectors.Count];

            for (int i = 0; i < sectors.Count; i++)
            {
                sumNormalize[i] = (float) blackPixels[i] / sum;
            }

            string sumNormalizeVector = String.Join(", ", sumNormalize);
            sumNormalizeVector = $"FarynaS1 - normalize by sum: ({sumNormalizeVector})";

            outToLog(sumNormalizeVector);

            float[] maxNormalize = new float[sectors.Count];
            int maxValue = blackPixels.Max();

            for (int i = 0; i < sectors.Count; i++)
            {
                maxNormalize[i] = (float)blackPixels[i] / maxValue;
            }

            string maxNormalizeVector = String.Join(", ", maxNormalize);
            maxNormalizeVector = $"FarynaM1 - normalize by max: ({maxNormalizeVector})";

            outToLog(maxNormalizeVector);

            pictureBox1.Image = grayscaleImage;
        }

        private void ProcessFillDrawSectors(int numSectors)
        {

            // Convert it to grayscale (if necessary)
            Bitmap grayscaleImage = new Bitmap(pictureBox1.Image);

            // Draw sector lines on the grayscale image
            DrawSectorLines(grayscaleImage, numSectors);

            // Update the pictureBox with the modified image
            pictureBox1.Image = grayscaleImage;

            // Optional: process sectors to count black pixels
            var sectors = DivideImageIntoSectors(numSectors, grayscaleImage.Size);
            Random r = new Random();
            Color[] sectorColors = { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta };

            for (int i = 0; i < sectors.Count; i++)
            {

                fillSector(grayscaleImage, sectors[i], Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256)));
            }

            pictureBox1.Image = grayscaleImage;
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

        public void fillSector(Bitmap image, GraphicsPath sector, Color highlightColor)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (sector.IsVisible(x, y))
                    {
                        image.SetPixel(x, y, highlightColor);
                    }
                }
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
