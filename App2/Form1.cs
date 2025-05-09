
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace App2
{
    public partial class Hopfield : Form
    {
        private Rectangle cropRect;
        private bool isCropping = false;
        private Point startPoint;
        private MemoryStream ms;
        private Image OriginalImg;
        private ImageService imageService;
        private HopfieldNetwork hopfieldNetwork;

        private List<InputClass> classes = new List<InputClass>();

        public Hopfield()
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

            // ����������, �� ������� �������� �������� � ������
            int threshold;
            if (!int.TryParse(txtThreshold.Text, out threshold) || threshold < 0 || threshold > 255)
            {
                MessageBox.Show("Please enter a valid threshold value between 0 and 255.");
                return;
            }


            pictureBox1.Image = imageService.bw(pictureBox1.Image, threshold);
        }

        private int[] Binarize(int[] features)
        {
            int avg = (int)features.Average();
            int[] binary = new int[features.Length];

            for (int i = 0; i < features.Length; i++)
                binary[i] = features[i] >= avg ? 1 : -1;

            return binary;
        }

        private int[] BinarizeFloat(float[] features, float regulator)
        {
            regulator = features.Average();
            int[] binary = new int[features.Length];

            for (int i = 0; i < features.Length; i++)
                binary[i] = features[i] >= regulator ? 1 : -1;

            return binary;
        }

        private void buttonSegment_Click(object sender, EventArgs e)
        {
            int sectorsCnt = 0;
            int.TryParse(sectorsNumber.Text, out sectorsCnt);

            float binarizeRegulator = 0;
            float.TryParse(binarizeTextBox.Text, out binarizeRegulator);

            Bitmap bw = new Bitmap(pictureBox1.Image);
            imageService.DrawSectorLines(bw, sectorsCnt);
            int[] blackPixels = imageService.getBlackPixels(sectorsCnt, bw);

            string vector = String.Join("; ", blackPixels);
            vector = $"Loaded image - vector: ({vector})";

            imageService.outToLog(vector);

            //float[] maxNormalize = imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);
            float[] sumNormalize = imageService.getSumNormalizeVector(blackPixels, sectorsCnt);
            int[] binarized = BinarizeFloat(sumNormalize, binarizeRegulator);

            //string maxNormalizeVector = String.Join("; ", maxNormalize);
            //maxNormalizeVector = $"Loaded image - normalize by max: ({maxNormalizeVector})";
            //imageService.outToLog(maxNormalizeVector);

            string sumNormalizeVector = String.Join("; ", sumNormalize);
            sumNormalizeVector = $"Loaded image - normalize by sum: ({sumNormalizeVector})";
            imageService.outToLog(sumNormalizeVector);

            string binarizedJoin = String.Join("; ", binarized);
            string binarizedText = $"Loaded image - binarized: ({binarizedJoin})";

            imageService.outToLog(binarizedText);

    

            pictureBox1.Image = bw;

            foreach (InputClass classImage in classes)
            {
                if (Enumerable.SequenceEqual(binarized, classImage.binarized))
                {
                    imageService.outToLog($"Found class: {classImage.Text}");
                    return;
                }
            }


            int[] result = hopfieldNetwork.Recognize(binarized);

            string resultJoin = String.Join("; ", result);
            string resulText = $"Loaded image - recognize vector: ({resultJoin})";
            imageService.outToLog(resulText);

            int classIndex = 0;

            this.dataGridView.DataSource = null;
            hopfieldNetwork.DisplayWeights(dataGridView);

            List<int[]> patterns = new List<int[]>();
            foreach (InputClass classImage in classes)
            {
                patterns.Add(classImage.binarized);
            }

            foreach (InputClass classImage in classes)
            {
                if (Enumerable.SequenceEqual(result, classImage.binarized))
                {
                    imageService.outToLog($"Found class: {classImage.Text}");
                    return;
                }
                ++classIndex;
            }
            
            imageService.outToLog("Class not found");
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
                // ��������� ����� �� ������ ���� ��������
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

            float binarizeRegulator = 0;
            float.TryParse(binarizeTextBox.Text, out binarizeRegulator);

            int.TryParse(sectorsNumber.Text, out numSectors);
            // ��������� ����� ��� �������� ����� �����
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.ShowDialog() == DialogResult.OK)
            {
                string formName = inputDialog.FormName;

                // ��������� ���� ��������� ���� � ��������� ������
                InputClass secondaryForm = new(formName, numSectors, threshold, logRichTextBox, binarizeRegulator);
                secondaryForm.FormClosed += InputClass_FormClosed; // ��� �������������� �������� �����
                classes.Add(secondaryForm); // ������ ����� � ������
                secondaryForm.Show();
            }
        }

        private void findByDistanceBtn_Click(object sender, EventArgs e)
        {
            int sectorsCnt = 0;
            int.TryParse(sectorsNumber.Text, out sectorsCnt);

            Bitmap bw = new Bitmap(pictureBox1.Image);
            imageService.DrawSectorLines(bw, sectorsCnt);
            int[] blackPixels = imageService.getBlackPixels(sectorsCnt, bw);

            string vector = String.Join("; ", blackPixels);
            vector = $"{Name} - vector: ({vector})";

            imageService.outToLog(vector);

            float[] sumNormalize = imageService.getSumNormalizeVector(blackPixels, sectorsCnt);
            float[] maxNormalize = imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);

            string sumNormalizeVector = String.Join("; ", sumNormalize);
            sumNormalizeVector = $"main - FarynaS1 - normalize by sum: ({sumNormalizeVector})";
            imageService.outToLog(sumNormalizeVector);

            string maxNormalizeVector = String.Join("; ", maxNormalize);
            maxNormalizeVector = $"main - FarynaM1 - normalize by max: ({maxNormalizeVector})";
            imageService.outToLog(maxNormalizeVector);

            pictureBox1.Image = bw;

            ArrayComparer arrayComparer = new ArrayComparer();
            bool isFind = false;
            float[] sDistances = new float[classes.Count];
            float[] mDistances = new float[classes.Count];
            int i = 0;
            foreach (InputClass classImage in classes)
            {
                sDistances[i] = imageService.ChebyshevDistance(classImage.FarynaS1Centr, sumNormalize);
                mDistances[i] = imageService.ChebyshevDistance(classImage.FarynaM1Centr, maxNormalize);
                i++;
                //bool findBySum = arrayComparer.Greater(sumNormalize, classImage.sumMinComponentsVector)
                //    && arrayComparer.Less(sumNormalize, classImage.sumMaxComponentsVector);

                //bool findByMax = arrayComparer.Greater(maxNormalize, classImage.maxMinComponentsVector)
                //    && arrayComparer.Less(maxNormalize, classImage.maxMaxComponentsVector);

                //if (findBySum)
                //{
                //    imageService.outToLog("found by FarynaS1 vector");

                //    string s1MaxComponentsVectorJoin = String.Join("; ", classImage.sumMaxComponentsVector);
                //    s1MaxComponentsVectorJoin = $"{Text} - FarynaS1MAX - max components: ({s1MaxComponentsVectorJoin})";

                //    string s1MinComponentsVectorJoin = String.Join("; ", classImage.sumMinComponentsVector);
                //    s1MinComponentsVectorJoin = $"{Text} - FarynaS1MIN - min components: ({s1MinComponentsVectorJoin})";

                //    imageService.outToLog(s1MaxComponentsVectorJoin);
                //    imageService.outToLog(s1MinComponentsVectorJoin);
                //}

                //if (findByMax)
                //{
                //    imageService.outToLog("found by FarynaM1 vector");

                //    string m1MaxComponentsVectorJoin = String.Join("; ", classImage.maxMaxComponentsVector);
                //    m1MaxComponentsVectorJoin = $"{Text} - FarynaM1MAX - max components: ({m1MaxComponentsVectorJoin})";

                //    string m1MinComponentsVectorJoin = String.Join("; ", classImage.maxMinComponentsVector);
                //    m1MinComponentsVectorJoin = $"{Text} - FarynaM1MIN - min components: ({m1MinComponentsVectorJoin})";

                //    imageService.outToLog(m1MaxComponentsVectorJoin);
                //    imageService.outToLog(m1MinComponentsVectorJoin);
                //}

                //if (findBySum || findByMax)
                //{
                //    imageService.outToLog($"found class: {classImage.Text}");
                //    isFind = true;
                //}
            }

            for (int j = 0; j < sDistances.Length; j++)
            {
                imageService.outToLog($"by sum d{j} = {sDistances[j]} - {classes[j].Text}");
            }

            for (int j = 0; j < mDistances.Length; j++)
            {
                imageService.outToLog($"by max d{j} = {mDistances[j]} - {classes[j].Text}");
            }

            int sMinIndex = Array.IndexOf(sDistances, sDistances.Min());
            int mMinIndex = Array.IndexOf(mDistances, mDistances.Min());
            imageService.outToLog($"find min distance by S1Centr d{sMinIndex} = {sDistances[sMinIndex]} - {classes[sMinIndex].Text}");
            imageService.outToLog($"find min distance by M1Centr d{mMinIndex} = {mDistances[mMinIndex]} - {classes[mMinIndex].Text}");


            //if (!isFind)
            //{
            //    imageService.outToLog("Class not found");
            //}
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void binarizeTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void trainButton_Click(object sender, EventArgs e)
        {
            int sectorsCnt = 0;
            int.TryParse(sectorsNumber.Text, out sectorsCnt);

            float binarizeRegulator = 0;
            float.TryParse(binarizeTextBox.Text, out binarizeRegulator);


            List<int[]> patterns = new List<int[]>();
            foreach (InputClass classImage in classes)
            {
                patterns.Add(classImage.binarized);
            }

            int vectorSize = patterns[0].Length;
            hopfieldNetwork = new HopfieldNetwork(vectorSize);
            hopfieldNetwork.Train(patterns);

            this.dataGridView.DataSource = null;
            hopfieldNetwork.DisplayWeights(dataGridView);
        }
    }
}
