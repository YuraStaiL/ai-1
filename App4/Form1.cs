namespace App4
{
    public partial class Form1 : Form
    {
        private List<Point> points = new List<Point>(); // Список точок образу
        private List<int> vectorClasses = new List<int>(); // Список класів векторів
        private List<int> savedVectorClasses = null; // Збережений образ
        private Bitmap trainingImage = null; // Для збереження навчального зображення
        private Bitmap testImage = null;     // Для збереження тестового зображення

        public Form1()
        {
            InitializeComponent();
            this.Text = "Vector Quantization Recognition";
            this.DoubleBuffered = true;
            this.MouseMove += MainForm_MouseMove;
            this.MouseDown += MainForm_MouseDown;
            this.MouseUp += MainForm_MouseUp;
        }

        private bool drawing = false;

        // Почати малювання при натисканні миші
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                points.Clear();
                vectorClasses.Clear();
            }
        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing && (points.Count == 0 || (points.Count > 0 && Distance(points[points.Count - 1], e.Location) >= 20)))
            {
                points.Add(e.Location);

                if (points.Count > 1)
                {
                    Point p1 = points[points.Count - 2];
                    Point p2 = points[points.Count - 1];

                    double angle = GetAngle(p1, p2);
                    int vectorClass = ClassifyVector(angle);
                    vectorClasses.Add(vectorClass);

                    // Відображення вектора під час малювання
                    using (Graphics g = CreateGraphics())
                    {
                        Pen pen = new Pen(Color.Blue, 2);
                        g.DrawLine(pen, p1, p2);
                        g.DrawString(vectorClass.ToString(), Font, Brushes.Red, (p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                    }
                }
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawing = false; // Зупиняємо малювання після відпускання кнопки
                Invalidate();    // Перемальовуємо форму
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
            if (angle < 0) angle += 360;
            return angle;
        }

        // Обчислити відстань між двома точками
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        // Збереження поточного образу
        private void SavePattern()
        {
            savedVectorClasses = new List<int>(vectorClasses);
            MessageBox.Show("Образ збережено.");
        }

        // Порівняння поточного образу із збереженим
        private void ClassifyPattern()
        {
            if (savedVectorClasses == null)
            {
                MessageBox.Show("Спочатку збережіть образ.");
                return;
            }

            double matchScore = ComparePatterns(savedVectorClasses, vectorClasses);
            MessageBox.Show($"Схожість із збереженим образом: {matchScore * 100:0.00}%");
        }

        // Функція порівняння образів
        private double ComparePatterns(List<int> pattern1, List<int> pattern2)
        {
            int minLength = Math.Min(pattern1.Count, pattern2.Count);
            int matches = 0;

            for (int i = 0; i < minLength; i++)
            {
                if (pattern1[i] == pattern2[i])
                    matches++;
            }

            return (double)matches / minLength;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void savePattern_Click(object sender, EventArgs e)
        {
            SavePattern();
        }

        private void findPattern_Click(object sender, EventArgs e)
        {
            ClassifyPattern();
        }
    }
}
