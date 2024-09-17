using System.Drawing;
using System.Windows.Forms;

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
                    // ������������ ����������
                    Image img = Image.FromFile(ofd.FileName);
                    OriginalImg = img;
                    pictureBox1.Image = img;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        private void btnRestoreOriginalImg_Click(object sender, EventArgs e)
        {
            // ������������ ����������
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

                pictureBox1.Invalidate(); // ��������� ���������� ��� ��������� ������������
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
            if (isCropping)
            {
                isCropping = false;
                Rectangle rec = new Rectangle(startPoint.X, startPoint.Y, Math.Abs(e.X - startPoint.X), Math.Abs(e.Y - startPoint.Y));
                cropRect = rec;
                CropImage(); // ��������� ������� ������ ���� ���������� ��������
            }
        }

        private void CropImage()
        {
            if (pictureBox1.Image == null || cropRect == Rectangle.Empty)
                return;

            // ��������� ��������� Bitmap ��� ������
            Bitmap sourceBitmap = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
            Graphics g = pictureBox1.CreateGraphics();
            pictureBox1.Refresh();

            g.DrawImage(sourceBitmap, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height), cropRect, GraphicsUnit.Pixel);
            sourceBitmap.Dispose();
            //pictureBox1.Image.Dispose();

            var path = Environment.CurrentDirectory.ToString();
            ms = new System.IO.MemoryStream();
            pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] ar = new byte[ms.Length];

            var timeout = ms.WriteAsync(ar, 0, ar.Length);

            //pictureBox1.Image = sourceBitmap.Clone(
            //    new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
            //    System.Drawing.Imaging.PixelFormat.DontCare
            //);

            // ����������, �� ���������� ������ �� �������� �� ��� ����������
            //cropRect = Rectangle.Intersect(cropRect, new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height));

            //// ������� ���������� �� ��������� ������� Clone
            //if (cropRect.Width > 0 && cropRect.Height > 0)
            //{
            //    Bitmap croppedBitmap = sourceBitmap.Clone(cropRect, sourceBitmap.PixelFormat);
            //    pictureBox1.Image = croppedBitmap;
            //}


        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("No image loaded!");
                return;
            }

            // ����������, �� ������� �������� �������� � ������
            int threshold;
            if (!int.TryParse(txtThreshold.Text, out threshold) || threshold < 0 || threshold > 255)
            {
                MessageBox.Show("Please enter a valid threshold value between 0 and 255.");
                return;
            }

            // ������������ ���������� � ���� ����
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    // �������� ������
                    Color pixelColor = bmp.GetPixel(x, y);

                    // ������� ��� ������������ � ���� (�����������) ����
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));

                    // ������������ ������ � ���� ����
                    Color grayColor = Color.FromArgb(grayScale, grayScale, grayScale);
                    bmp.SetPixel(x, y, grayColor);
                }
            }

            // ������ ����: �����-��� ������������ �� ����� ������
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    // �������� ������ � ���� ��������� (��� ����)
                    Color grayColor = bmp.GetPixel(x, y);
                    int brightness = grayColor.R;  // ѳ� ����� ����� ������� �������� R, G, B

                    // ��������� ��������� � ������� � ������������ �� ������ ��� ����
                    if (brightness < threshold)
                        bmp.SetPixel(x, y, Color.Black);
                    else
                        bmp.SetPixel(x, y, Color.White);
                }
            }

            // ³��������� ����������� ���������� � PictureBox
            pictureBox1.Image = bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
