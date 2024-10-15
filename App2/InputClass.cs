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

        public InputClass(string formName, int sectorsCnt, int threshold, TextBoxBase? logTextBox = null)
        {
            InitializeComponent();
            this.Text = formName; // Встановлюємо назву форми
            this.sectorsCnt = sectorsCnt;
            this.threshold = threshold;
            this.logTextBox = logTextBox;
            imageService = new ImageService(logTextBox);
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

        private void btnSegment_Click(object sender, EventArgs e)
        {
            //List<Bitmap> processedImages = new List<Bitmap>();
            //loadedImages.Select(image => (Image)imageService.ProcessImageAndDrawSectors(sectorsCnt, imageService.bw(image, threshold)));
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

                float[] sumNormalize = imageService.getSumNormalizeVector(blackPixels, sectorsCnt);
                float[] maxNormalize = imageService.getMaxNormalizeVector(blackPixels, sectorsCnt);

                for (int element = 0; element < sectorsCnt; element++)
                {
                    sumVectorsDict[imageIndex, element] = sumNormalize[element];
                    maxVectorsDict[imageIndex, element] = maxNormalize[element];
                }

                imageIndex++;

                string sumNormalizeVector = String.Join("; ", sumNormalize);
                sumNormalizeVector = $"{Text} - FarynaS1 - normalize by sum: ({sumNormalizeVector})";
                imageService.outToLog(sumNormalizeVector);

                string maxNormalizeVector = String.Join("; ", maxNormalize);
                maxNormalizeVector = $"{Text} - FarynaM1 - normalize by max: ({maxNormalizeVector})";
                imageService.outToLog(maxNormalizeVector);

                //Bitmap processedImage = imageService.ProcessImageAndDrawSectors(sectorsCnt, bw, Text);

                PictureBox pictureBox = new PictureBox
                {
                    Image = bw,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 250,
                    Height = 250
                };
                flowLayoutPanel1.Controls.Add(pictureBox);
                //Image bwImage = i;
                //image = imageService.ProcessImageAndDrawSectors(sectorsCnt, image);
            }

            maxMaxComponentsVector = imageService.getMaxComponentsVector(maxVectorsDict);
            sumMaxComponentsVector = imageService.getMaxComponentsVector(sumVectorsDict);

            maxMinComponentsVector = imageService.getMinComponentsVector(maxVectorsDict);
            sumMinComponentsVector = imageService.getMinComponentsVector(sumVectorsDict);

            string m1MaxComponentsVectorJoin = String.Join("; ", maxMaxComponentsVector);
            m1MaxComponentsVectorJoin = $"{Text} - FarynaM1MAX - max components: ({m1MaxComponentsVectorJoin})";

            string s1MaxComponentsVectorJoin = String.Join("; ", sumMaxComponentsVector);
            s1MaxComponentsVectorJoin = $"{Text} - FarynaS1MAX - max components: ({s1MaxComponentsVectorJoin})";

            string m1MinComponentsVectorJoin = String.Join("; ", maxMinComponentsVector);
            m1MinComponentsVectorJoin = $"{Text} - FarynaM1MIN - min components: ({m1MinComponentsVectorJoin})";

            string s1MinComponentsVectorJoin = String.Join("; ", sumMinComponentsVector);
            s1MinComponentsVectorJoin = $"{Text} - FarynaS1MIN - min components: ({s1MinComponentsVectorJoin})";


            imageService.outToLog(m1MaxComponentsVectorJoin);
            imageService.outToLog(s1MaxComponentsVectorJoin);
            imageService.outToLog(m1MinComponentsVectorJoin);
            imageService.outToLog(s1MinComponentsVectorJoin);
        }
    }
}
