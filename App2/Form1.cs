
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace App2
{
    public partial class Form1 : Form
    {
        private Rectangle cropRect;
        private bool isCropping = false;
        private Point startPoint;
        private MemoryStream ms;
        private Image OriginalImg;
        private ImageService imageService;

        private List<InputClass> classes = new List<InputClass>();

        public Form1()
        {
            InitializeComponent();
            imageService = new ImageService(logRichTextBox);
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

            
            pictureBox1.Image = imageService.bw(pictureBox1.Image, threshold);
        }

        private void buttonSegment_Click(object sender, EventArgs e)
        {
            int numSectors = 0;
            int.TryParse(sectorsNumber.Text, out numSectors);

            pictureBox1.Image = imageService.ProcessImageAndDrawSectors(numSectors, pictureBox1.Image);
        }

        private void buttonFillSegment_Click(object sender, EventArgs e)
        {
            int numSectors = 0;
            int.TryParse(sectorsNumber.Text, out numSectors);

            ProcessFillDrawSectors(numSectors);
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
                    Point endPoint = imageService.CalculateIntersectionWithImageBorder(angle, image.Width, image.Height, ref isHaveLeftCornerPoint);

                    // Draw the line from the top-right corner to the calculated intersection point
                    g.DrawLine(pen, topRight, endPoint);
                }
            }
        }

        private void ProcessFillDrawSectors(int numSectors)
        {

            // Convert it to grayscale (if necessary)
            Bitmap grayscaleImage = new Bitmap(pictureBox1.Image);

            // Draw sector lines on the grayscale image
            imageService.DrawSectorLines(grayscaleImage, numSectors);

            // Update the pictureBox with the modified image
            pictureBox1.Image = grayscaleImage;

            // Optional: process sectors to count black pixels
            var sectors = imageService.DivideImageIntoSectors(numSectors, grayscaleImage.Size);
            Random r = new Random();
            Color[] sectorColors = { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta };

            for (int i = 0; i < sectors.Count; i++)
            {

                fillSector(grayscaleImage, sectors[i], Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256)));
            }

            pictureBox1.Image = grayscaleImage;
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

        private void InputClass_FormClosed(object sender, FormClosedEventArgs e)
        {
            InputClass closedForm = sender as InputClass;
            if (closedForm != null)
            {
                // Видаляємо форму із списку після закриття
                classes.Remove(closedForm);
            }
        }

        private void AddClass_Click(object sender, EventArgs e)
        {
            int numSectors = 0;
            int threshold;

            if (!int.TryParse(txtThreshold.Text, out threshold) || threshold < 0 || threshold > 255)
            {
                MessageBox.Show("Please enter a valid threshold value between 0 and 255.");
                return;
            }

            if (!int.TryParse(sectorsNumber.Text, out numSectors) || numSectors < 2)
            {
                MessageBox.Show("Please enter a valid Number of sectors value greater than 1");
                return;
            }

            int.TryParse(sectorsNumber.Text, out numSectors);
            // Викликаємо діалог для введення назви форми
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.ShowDialog() == DialogResult.OK)
            {
                string formName = inputDialog.FormName;

                // Створюємо нове додаткове вікно з переданою назвою
                InputClass secondaryForm = new(formName, numSectors, threshold, logRichTextBox);
                secondaryForm.FormClosed += InputClass_FormClosed; // Для відслідковування закриття форми
                classes.Add(secondaryForm); // Додаємо форму в список
                secondaryForm.Show();
            }
        }
    }
}
