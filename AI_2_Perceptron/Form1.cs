
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace AI_2_Perceptron
{
    public partial class Form1 : Form
    {
        private Rectangle cropRect;
        private bool isCropping = false;
        private Point startPoint;
        private MemoryStream ms;
        private Image OriginalImg;
        private ImageService imageService;
        private Perceptron Perceptron;

        private List<InputClass> classes = new List<InputClass>();
        private List<Image> loadedImages = new List<Image>();

        public Form1()
        {
            InitializeComponent();
            imageService = new ImageService(logRichTextBox);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void train(int threshold = 177, int sectorsCnt = 6)
        {
            double learningRate;
            double.TryParse(learningRateValue.Text, out learningRate);

            int.TryParse(sectorsNumber.Text, out sectorsCnt);
            int.TryParse(txtThreshold.Text, out threshold);

            if (Perceptron == null)
            {
                Perceptron = new Perceptron(imageService, sectorsCnt, learningRate);
            }

            int epoch;
            int.TryParse(epochCnt.Text, out epoch);


            Perceptron.SetFirstClassName(firstClassName.Text);
            Perceptron.SetSecondClassName(secondClassName.Text);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            openFileDialog.Multiselect = true;


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Random r = new Random();
                foreach (string fileName in openFileDialog.FileNames.OrderBy(x => r.Next()))
                {
                    Image image = Image.FromFile(fileName);
                    Bitmap bw = imageService.bw(image, threshold);

                    imageService.DrawSectorLines(bw, sectorsCnt);
                    int[] blackPixels = imageService.getBlackPixels(sectorsCnt, bw);

                    string vector = String.Join("; ", blackPixels);
                    vector = $"absolute vector: ({vector})";
                    imageService.outToLog(vector);

                    imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);

                    pictureBox1.Image = image;

                    float[] maxNormalize = getMaxNormalize(bw, threshold, sectorsCnt);

                    string maxNormalizeString = String.Join("; ", Array.ConvertAll(maxNormalize, x => Math.Round(x, 3)));
                    vector = $"normalize by max vector: ({maxNormalizeString})";
                    imageService.outToLog(vector);

                    pictureBox1.Image = bw;

                    Perceptron.TrainOne(Array.ConvertAll(maxNormalize, x => (double)x), epoch);
                    //flowLayoutPanel1.Controls.Add(pictureBox);
                    //loadedImages.Add(Image.FromFile(fileName)); // Додаємо зображення в список
                }
            }
        }

        private float[] getMaxNormalize(Bitmap bw, int threshold = 177, int sectorsCnt = 6)
        {
            imageService.DrawSectorLines(bw, sectorsCnt);
            int[] blackPixels = imageService.getBlackPixels(sectorsCnt, bw);

            string vector = String.Join("; ", blackPixels);
            vector = $"{Text} - absolute vector: ({vector})";

            return imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);
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
            foreach (InputClass classImage in classes)
            {
                bool findBySum = arrayComparer.Greater(sumNormalize, classImage.sumMinComponentsVector)
                    && arrayComparer.Less(sumNormalize, classImage.sumMaxComponentsVector);

                bool findByMax = arrayComparer.Greater(maxNormalize, classImage.maxMinComponentsVector)
                    && arrayComparer.Less(maxNormalize, classImage.maxMaxComponentsVector);

                if (findBySum)
                {
                    imageService.outToLog("found by FarynaS1 vector");

                    string s1MaxComponentsVectorJoin = String.Join("; ", classImage.sumMaxComponentsVector);
                    s1MaxComponentsVectorJoin = $"{Text} - FarynaS1MAX - max components: ({s1MaxComponentsVectorJoin})";

                    string s1MinComponentsVectorJoin = String.Join("; ", classImage.sumMinComponentsVector);
                    s1MinComponentsVectorJoin = $"{Text} - FarynaS1MIN - min components: ({s1MinComponentsVectorJoin})";

                    imageService.outToLog(s1MaxComponentsVectorJoin);
                    imageService.outToLog(s1MinComponentsVectorJoin);
                }

                if (findByMax)
                {
                    imageService.outToLog("found by FarynaM1 vector");

                    string m1MaxComponentsVectorJoin = String.Join("; ", classImage.maxMaxComponentsVector);
                    m1MaxComponentsVectorJoin = $"{Text} - FarynaM1MAX - max components: ({m1MaxComponentsVectorJoin})";

                    string m1MinComponentsVectorJoin = String.Join("; ", classImage.maxMinComponentsVector);
                    m1MinComponentsVectorJoin = $"{Text} - FarynaM1MIN - min components: ({m1MinComponentsVectorJoin})";

                    imageService.outToLog(m1MaxComponentsVectorJoin);
                    imageService.outToLog(m1MinComponentsVectorJoin);
                }

                if (findBySum || findByMax)
                {
                    imageService.outToLog($"found class: {classImage.Text}");
                    isFind = true;
                }
            }

            if (!isFind)
            {
                imageService.outToLog("Class not found");
            }
        }

        private void buttonFillSegment_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            openFileDialog.Multiselect = true;
            int threshold = 177;
            int sectorsCnt = 6;

            int.TryParse(sectorsNumber.Text, out sectorsCnt);
            int.TryParse(txtThreshold.Text, out threshold);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Random r = new Random();
                foreach (string fileName in openFileDialog.FileNames.OrderBy(x => r.Next()))
                {
                    Image image = Image.FromFile(fileName);
                    Bitmap bw = imageService.bw(image, threshold);

                    imageService.DrawSectorLines(bw, sectorsCnt);
                    int[] blackPixels = imageService.getBlackPixels(sectorsCnt, bw);

                    string vector = String.Join("; ", blackPixels);
                    vector = $"absolute vector: ({vector})";
                    imageService.outToLog(vector);

                    imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);

                    pictureBox1.Image = image;

                    float[] maxNormalize = getMaxNormalize(bw, threshold, sectorsCnt);

                    string maxNormalizeString = String.Join("; ", Array.ConvertAll(maxNormalize, x => Math.Round(x, 3)));
                    vector = $"normalize by max vector: ({maxNormalizeString})";
                    imageService.outToLog(vector);

                    pictureBox1.Image = bw;

                    int predict = Perceptron.Predict(Array.ConvertAll(maxNormalize, x => (double)x));
                    string className = Perceptron.getClassName(predict);

                    MessageBox.Show($"This is {className}!", "Classify result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //flowLayoutPanel1.Controls.Add(pictureBox);
                    //loadedImages.Add(Image.FromFile(fileName)); // Додаємо зображення в список
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
            train();

            //int numSectors = 0;
            //int threshold;

            //if (!int.TryParse(txtThreshold.Text, out threshold) || threshold < 0 || threshold > 255)
            //{
            //    MessageBox.Show("Please enter a valid threshold value between 0 and 255.");
            //    return;
            //}

            //if (!int.TryParse(sectorsNumber.Text, out numSectors) || numSectors < 2)
            //{
            //    MessageBox.Show("Please enter a valid Number of sectors value greater than 1");
            //    return;
            //}

            //int.TryParse(sectorsNumber.Text, out numSectors);
            //// Викликаємо діалог для введення назви форми

            //InputDialog inputDialog = new InputDialog();
            //if (inputDialog.ShowDialog() == DialogResult.OK)
            //{
            //    string formName = inputDialog.FormName;

            //    //// Створюємо нове додаткове вікно з переданою назвою
            //    //InputClass secondaryForm = new(formName, numSectors, threshold, logRichTextBox);
            //    //secondaryForm.FormClosed += InputClass_FormClosed; // Для відслідковування закриття форми
            //    //classes.Add(secondaryForm); // Додаємо форму в список
            //    //secondaryForm.Show();
            //}
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void learningRateValue_TextChanged(object sender, EventArgs e)
        {
            if (Perceptron != null)
            {
                double learningRate;
                double.TryParse(learningRateValue.Text, out learningRate);
                Perceptron.SetLearningRate(learningRate);
            }

        }

        private void sectorNumbers_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
