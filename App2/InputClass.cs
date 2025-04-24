namespace App2
{
    public partial class InputClass : Form
    {
        private List<Image> loadedImages = new List<Image>(); // Зберігаємо всі завантажені зображення
        private int sectorsCnt = 0;
        private int threshold = 0;
        private TextBoxBase? logTextBox = null;
        private ImageService imageService;
        private ArrayComparer arrayComparer;
        private float[] sumNormalize;
        private float[] maxNormalize;

        public float[,] sumVectorsDict;
        public float[,] maxVectorsDict;

        public float[] maxMaxComponentsVector;
        public float[] sumMaxComponentsVector;

        public float[] maxMinComponentsVector;
        public float[] sumMinComponentsVector;
        public float[] FarynaS1Centr;
        public float[] FarynaM1Centr;

        public int[] binarized;

        public float binarizeRegulator = 0;

        public InputClass(string formName, int sectorsCnt, int threshold, TextBoxBase? logTextBox = null, float binarizeRegulator = 0)
        {
            InitializeComponent();
            this.Text = formName; // Встановлюємо назву форми
            this.sectorsCnt = sectorsCnt;
            this.threshold = threshold;
            this.logTextBox = logTextBox;
            this.binarizeRegulator = binarizeRegulator;
            imageService = new ImageService(logTextBox);
        }

        private int[] Binarize(int[] features)
        {
            int avg = (int)features.Average();
            int[] binary = new int[features.Length];

            for (int i = 0; i < features.Length; i++)
                binary[i] = features[i] >= avg ? 1 : -1;

            return binary;
        }

        private void btnUploadMultiple_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Image = Image.FromFile(fileName),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Width = 250,
                        Height = 250
                    };
                    flowLayoutPanel1.Controls.Add(pictureBox);
                    loadedImages.Add(Image.FromFile(fileName)); // Додаємо зображення в список
                }
            }
        }

        // Повертаємо всі завантажені зображення
        public List<Image> GetLoadedImages()
        {
            return loadedImages;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private int[] BinarizeFloat(float[] features, float regulator)
        {
            regulator = features.Average();
            int[] binary = new int[features.Length];

            for (int i = 0; i < features.Length; i++)
                binary[i] = features[i] >= regulator ? 1 : -1;

            return binary;
        }

        private void btnSegment_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            int imageIndex = 0;
            sumVectorsDict = new float[loadedImages.Count, sectorsCnt];
            maxVectorsDict = new float[loadedImages.Count, sectorsCnt];

            foreach (Image image in loadedImages)
            {
                Bitmap bw = imageService.bw(image, threshold);
                imageService.DrawSectorLines(bw, sectorsCnt);
                int[] blackPixels = imageService.getBlackPixels(sectorsCnt, bw);

                string vector = String.Join("; ", blackPixels);
                vector = $"{Text} - vector: ({vector})";

                //float[] maxNormalize = imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);
                //string maxNormalizeVector = String.Join("; ", maxNormalize);
                //maxNormalizeVector = $"{Text} - normalize by max: ({maxNormalizeVector})";
                //imageService.outToLog(maxNormalizeVector);

                float[] sumNormalize = imageService.getSumNormalizeVector(blackPixels, sectorsCnt);
                string sumNormalizeVector = String.Join("; ", sumNormalize);
                sumNormalizeVector = $"Loaded image - normalize by sum: ({sumNormalizeVector})";
                imageService.outToLog(sumNormalizeVector);

                binarized = BinarizeFloat(sumNormalize, binarizeRegulator);


                string binarizedJoin = String.Join("; ", binarized);
                string binarizedText = $"{Text} - binarized: ({binarizedJoin})";

                imageService.outToLog(vector);
                imageService.outToLog(binarizedText);



                //float[] sumNormalize = imageService.getSumNormalizeVector(blackPixels, sectorsCnt);
                //float[] maxNormalize = imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);

                //for (int element = 0; element < sectorsCnt; element++)
                //{
                //    sumVectorsDict[imageIndex, element] = sumNormalize[element];
                //    maxVectorsDict[imageIndex, element] = maxNormalize[element];
                //}

                //imageIndex++;

                //string sumNormalizeVector = String.Join("; ", sumNormalize);
                //sumNormalizeVector = $"{Text} - FarynaS1 - normalize by sum: ({sumNormalizeVector})";
                //imageService.outToLog(sumNormalizeVector);

                //string maxNormalizeVector = String.Join("; ", maxNormalize);
                //maxNormalizeVector = $"{Text} - FarynaM1 - normalize by max: ({maxNormalizeVector})";
                //imageService.outToLog(maxNormalizeVector);

                PictureBox pictureBox = new PictureBox
                {
                    Image = bw,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 250,
                    Height = 250
                };
                flowLayoutPanel1.Controls.Add(pictureBox);
            }

            //maxMaxComponentsVector = imageService.getMaxComponentsVector(maxVectorsDict);
            //sumMaxComponentsVector = imageService.getMaxComponentsVector(sumVectorsDict);

            //maxMinComponentsVector = imageService.getMinComponentsVector(maxVectorsDict);
            //sumMinComponentsVector = imageService.getMinComponentsVector(sumVectorsDict);

            //FarynaM1Centr = imageService.getAvgComponentsVector(maxVectorsDict);
            //FarynaS1Centr = imageService.getAvgComponentsVector(sumVectorsDict);

            //string m1MaxComponentsVectorJoin = String.Join("; ", maxMaxComponentsVector);
            //m1MaxComponentsVectorJoin = $"{Text} - FarynaM1MAX - max components: ({m1MaxComponentsVectorJoin})";

            //string s1MaxComponentsVectorJoin = String.Join("; ", sumMaxComponentsVector);
            //s1MaxComponentsVectorJoin = $"{Text} - FarynaS1MAX - max components: ({s1MaxComponentsVectorJoin})";

            //string m1MinComponentsVectorJoin = String.Join("; ", maxMinComponentsVector);
            //m1MinComponentsVectorJoin = $"{Text} - FarynaM1MIN - min components: ({m1MinComponentsVectorJoin})";

            //string s1MinComponentsVectorJoin = String.Join("; ", sumMinComponentsVector);
            //s1MinComponentsVectorJoin = $"{Text} - FarynaS1MIN - min components: ({s1MinComponentsVectorJoin})";
            //string M1CentrJoin = String.Join("; ", FarynaM1Centr);
            //M1CentrJoin = $"{Text} - FarynaM1Centr - ({M1CentrJoin})";

            //string S1CentrJoin = String.Join("; ", FarynaS1Centr);
            //S1CentrJoin = $"{Text} - FarynaS1Centr - ({S1CentrJoin})";

            //imageService.outToLog(m1MaxComponentsVectorJoin);
            //imageService.outToLog(s1MaxComponentsVectorJoin);
            //imageService.outToLog(m1MinComponentsVectorJoin);
            //imageService.outToLog(s1MinComponentsVectorJoin);

            //imageService.outToLog(M1CentrJoin);
            //imageService.outToLog(S1CentrJoin);
        }
    }
}
