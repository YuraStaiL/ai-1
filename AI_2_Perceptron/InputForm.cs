using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_2_Perceptron
{
    public partial class InputDialog : Form
    {
        public bool FormResponse { get; private set; }

        public InputDialog(string message)
        {
            InitializeComponent();
            confirmationMsg.Text = message;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FormResponse = true; // Отримуємо введену назву форми
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void classNameLabel_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormResponse = false; // Отримуємо введену назву форми
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        public static DialogResult ShowDialog(string message)
        {
            using (var form = new InputDialog(message))
            {
                return form.ShowDialog();
            }
        }
    }
}
