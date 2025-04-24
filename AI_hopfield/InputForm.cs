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
    public partial class InputDialog : Form
    {
        public string FormName { get; private set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            FormName = classNameTextBox.Text; // Отримуємо введену назву форми
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
