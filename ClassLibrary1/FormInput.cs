using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary1
{
    public partial class FormInput : System.Windows.Forms.Form
    {
        public FormInput()
        {
            InitializeComponent();
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) && (e.KeyChar == (char)Keys.Back)))

                e.Handled = false;
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) {
                MessageBox.Show("Please ! Input angle for rotation.");
            }
            if (double.TryParse(textBox1.Text, out double angle)) {
                double radianAngle = (angle * Math.PI) / 180;
                if (Rotate != null) {
                    Rotate.Invoke(radianAngle);
                }
            }
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) {
                MessageBox.Show("Please ! Input angle for rotation.");
            }
            if (double.TryParse(textBox1.Text, out double angle)) {
                double radianAngle = (-1) * (angle * Math.PI) / 180;
                if (Rotate != null) {
                    Rotate.Invoke(radianAngle);
                }
            }
        }

        public Action<double> Rotate { get; set; }
    }
}