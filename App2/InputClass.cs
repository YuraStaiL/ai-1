using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App2
{
    public partial class InputClass : Form
    {
        private List<Image> loadedImages = new List<Image>(); // Зберігаємо всі завантажені зображення
        private int sectorsCnt = 0;
        private int threshold = 0;
        private TextBoxBase? logTextBox = null;
        private ImageService imageService;

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
            //pictureBox1.Image = imageService.ProcessImageAndDrawSectors(numSectors, pictureBox1.Image);
            //imageService.bw(pictureBox1.Image, threshold);
        }
    }
}
