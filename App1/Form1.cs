
using System.Drawing.Drawing2D;

namespace App1
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
            ProcessImageAndDrawSectors();
        }

        public List<GraphicsPath> DivideImageIntoSectors(int numSectors, Size imageSize)
        {
            List<GraphicsPath> sectors = new List<GraphicsPath>();
            Point topRight = new Point(imageSize.Width - 1, 0);

            for (int i = 0; i < numSectors; i++)
            {
                GraphicsPath path = new GraphicsPath();

                // Define the sector's angles and points
                float angleStep = 90f / numSectors;
                float startAngle = i * angleStep;
                float endAngle = startAngle + angleStep;

                PointF point1 = new PointF(
                    (float)(imageSize.Width * Math.Cos(Math.PI * startAngle / 180)),
                    (float)(imageSize.Height * Math.Sin(Math.PI * startAngle / 180))
                );

                PointF point2 = new PointF(
                    (float)(imageSize.Width * Math.Cos(Math.PI * endAngle / 180)),
                    (float)(imageSize.Height * Math.Sin(Math.PI * endAngle / 180))
                );

                // Add triangle points for the sector
                path.AddPolygon(new PointF[] { topRight, point1, point2 });
                sectors.Add(path);
            }

            return sectors;
        }

        private void DrawSectorLines(Bitmap image, int numSectors)
        {
            // Create a graphics object from the image
            using (Graphics g = Graphics.FromImage(image))
            {
                // Set the pen color and thickness for drawing
                Pen pen = new Pen(Color.Red, 2);

                // Define the starting point (top-right corner of the image)
                Point topRight = new Point(image.Width - 1, 0);

                // Calculate the angle step for the number of sectors
                float angleStep = 90f / numSectors;

                for (int i = 1; i < numSectors; i++)
                {
                    // Calculate the angle for the current line
                    float angle = i * angleStep;

                    // Calculate the end point of the line based on the angle
                    float x = (float)(image.Width - 1 - image.Width * Math.Cos(Math.PI * angle / 180));
                    float y = (float)(image.Height * Math.Sin(Math.PI * angle / 180));

                    Point endPoint = new Point((int)x, (int)y);

                    // Draw the line
                    g.DrawLine(pen, topRight, endPoint);
                }
            }
        }

        private void ProcessImageAndDrawSectors()
        {

            // Convert it to grayscale (if necessary)
            Bitmap grayscaleImage = new Bitmap(pictureBox1.Image);

            // Number of sectors (get this from user input or set as needed)
            int numSectors = 4; // Example value

            // Draw sector lines on the grayscale image
            DrawSectorLines(grayscaleImage, numSectors);

            // Update the pictureBox with the modified image
            pictureBox1.Image = grayscaleImage;

            // Optional: process sectors to count black pixels
            var sectors = DivideImageIntoSectors(numSectors, grayscaleImage.Size);

            //for (int i = 0; i < sectors.Count; i++)
            //{
            //    int blackPixels = CountBlackPixelsInSector(grayscaleImage, sectors[i]);
            //    Console.WriteLine($"Sector {i + 1}: {blackPixels} black pixels");
            //}
        }

        private int CountBlackPixels(PointF start, PointF end)
        {
            // Implement the method to count black pixels within the sector
            // This requires more advanced geometric calculations to determine which pixels are within each sector
            return 0; // Placeholder
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
