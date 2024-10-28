namespace App4
{
    public partial class Form1 : Form
    {
        private List<Point> points = new List<Point>();
        private List<int> vectorClasses = new List<int>();
        private List<int> savedVectorClasses = null;
        private List<int> testSavedVectorClasses = null;
        private List<int> normalizedVectorClasses = null;
        private List<int> normalizedTestVectorClasses = null;

        private Bitmap trainingImage;
        private Bitmap testImage;
        private bool drawing = false;
        private int separationDistance = 20;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Lab 4 Vector Quantization Recognition";
            this.DoubleBuffered = true;

            // Ініціалізація зображень для малювання
            trainingImage = new Bitmap(pictureBoxTraining.Width, pictureBoxTraining.Height);
            testImage = new Bitmap(pictureBoxTest.Width, pictureBoxTest.Height);

            // Призначаємо Bitmap для PictureBox для забезпечення відображення
            pictureBoxTraining.Image = trainingImage;
            pictureBoxTest.Image = testImage;

            // Призначення подій для PictureBox
            pictureBoxTraining.MouseDown += PictureBox_MouseDown;
            pictureBoxTraining.MouseMove += PictureBox_MouseMove;
            pictureBoxTraining.MouseUp += PictureBox_MouseUp;

            pictureBoxTest.MouseDown += PictureBox_MouseDown;
            pictureBoxTest.MouseMove += PictureBox_MouseMove;
            pictureBoxTest.MouseUp += PictureBox_MouseUp;
            textBoxSeparationDistance.TextChanged += TextBoxSeparationDistance_TextChanged;
        }

        private List<int> normalizeVector(List<int> vector)
        {
            if (vector.Count <= 1)
            {
                return vector;
            }

            List<int> normalized = new List<int>();
            normalized.Add(vector[0]);
            
            for (int i = 1; i < vector.Count; i++)
            {
                int previous = vector[i - 1];
                if (vector[i] != previous) {
                    normalized.Add(vector[i]);
                }
            }

            return normalized;
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                points.Clear();
                vectorClasses.Clear();
                if (sender == pictureBoxTraining)
                {
                    using (Graphics g = Graphics.FromImage(trainingImage))
                    {
                        g.Clear(Color.White);
                    }
                    pictureBoxTraining.Invalidate();
                }
                else if (sender == pictureBoxTest)
                {
                    using (Graphics g = Graphics.FromImage(testImage))
                    {
                        g.Clear(Color.White);
                    }
                    pictureBoxTest.Invalidate();
                }
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing && (points.Count == 0 || (points.Count > 0 && Distance(points[points.Count - 1], e.Location) >= separationDistance)))
            {
                points.Add(e.Location);

                if (points.Count > 1)
                {
                    Point p1 = points[points.Count - 2];
                    Point p2 = points[points.Count - 1];
                    double angle = GetAngle(p1, p2);
                    //outToLog(angle.ToString());
                    int vectorClass = ClassifyVector(angle);
                    vectorClasses.Add(vectorClass);

                    if (sender == pictureBoxTraining)
                    {
                        using (Graphics g = Graphics.FromImage(trainingImage))
                        {
                            DrawVector(g, p1, p2, vectorClass);
                        }
                        pictureBoxTraining.Invalidate();
                    }
                    else if (sender == pictureBoxTest)
                    {
                        using (Graphics g = Graphics.FromImage(testImage))
                        {
                            DrawVector(g, p1, p2, vectorClass);
                        }
                        pictureBoxTest.Invalidate();
                    }
                }
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawing = false;

                if (sender == pictureBoxTraining)
                {
                    savedVectorClasses = new List<int>(vectorClasses);
                    normalizedVectorClasses = normalizeVector(vectorClasses);
                    MessageBox.Show("Навчальне зображення збережено.");
                    outToLog($"Оригінальний навчальний вектор - ({string.Join(", ", vectorClasses)})");
                    outToLog($"Нормалізований навчальний вектор - ({string.Join(", ", normalizedVectorClasses)})");
                }
                else if (sender == pictureBoxTest)
                {
                    testSavedVectorClasses = new List<int>(vectorClasses);
                    normalizedTestVectorClasses = normalizeVector(vectorClasses);
                    double matchScore = ComparePatterns(normalizedVectorClasses, normalizedTestVectorClasses);
                    outToLog($"Оригінальний тестовий вектор - ({string.Join(", ", vectorClasses)})");
                    outToLog($"Нормалізований тестовий вектор - ({string.Join(", ", normalizedTestVectorClasses)})");
                    MessageBox.Show($"Схожість із збереженим образом: {matchScore * 100:0.00}%");
                }
            }
        }

        // Функція для класифікації векторів за кутом
        private int ClassifyVector(double angle)
        {
            
            return (int)(angle / 45) + 1;
        }

        // Отримати кут вектора в градусах
        private double GetAngle(Point p1, Point p2)
        {
            double deltaY = p2.Y - p1.Y;
            double deltaX = p2.X - p1.X;
            double angle = Math.Atan2(deltaY, deltaX) * 180 / Math.PI;
            angle += 90;
            if (angle < 0) angle += 360;

            return angle;
        }

        // Обчислити відстань між двома точками
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        // Функція для малювання вектора та його класу
        private void DrawVector(Graphics g, Point p1, Point p2, int vectorClass)
        {
            Pen pen = new Pen(Color.Blue, 2);
            g.DrawLine(pen, p1, p2);
            g.DrawString(vectorClass.ToString(), Font, Brushes.Red, (p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        // Функція порівняння образів
        private double ComparePatterns(List<int> pattern1, List<int> pattern2)
        {
            if (pattern1 == null || pattern2 == null)
                return 0;

            int minLength = Math.Min(pattern1.Count, pattern2.Count);
            int matches = 0;

            for (int i = 0; i < minLength; i++)
            {
                if (pattern1[i] == pattern2[i])
                    matches++;
            }

            return (double)matches / minLength;
        }

        // Обробка зміни тексту в textBoxSeparationDistance
        private void TextBoxSeparationDistance_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxSeparationDistance.Text, out int newDistance) && newDistance > 0)
            {
                separationDistance = newDistance;
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректне число більше нуля.");
                textBoxSeparationDistance.Text = separationDistance.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void savePattern_Click(object sender, EventArgs e)
        {
            //SavePattern();
        }

        private void findPattern_Click(object sender, EventArgs e)
        {
            double matchScore = ComparePatterns(normalizedVectorClasses, normalizedTestVectorClasses);
            MessageBox.Show($"Схожість із збереженим образом: {matchScore * 100:0.00}%");
        }

        public void outToLog(string output)
        {
            logRichTextBox.AppendText("\r\n" + output);
            logRichTextBox.ScrollToCaret();
        }

    }
}
