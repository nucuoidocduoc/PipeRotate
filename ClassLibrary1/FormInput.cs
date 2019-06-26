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
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrEmpty(textBox1.Text)) {
                    MessageBox.Show("Please ! Input angle for rotation.");
                }
                if (double.TryParse(textBox1.Text, out double angle)) {
                    double radianAngle = (angle * Math.PI) / 180;
                    if (Rotate != null) {
                        Rotate.Invoke(radianAngle, radioButton1.Checked);
                    }
                }
            }
            catch (Exception) {
                MessageBox.Show("Error, Please contact nguyenphuongbka@gmail.com for more details.");
            }
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrEmpty(textBox1.Text)) {
                    MessageBox.Show("Please ! Input angle for rotation.");
                }
                if (double.TryParse(textBox1.Text, out double angle)) {
                    double radianAngle = (-1) * (angle * Math.PI) / 180;
                    if (Rotate != null) {
                        Rotate.Invoke(radianAngle, radioButton1.Checked);
                    }
                }
            }
            catch (Exception) {
                MessageBox.Show("Error, Please contact nguyenphuongbka@gmail.com for more details.");
            }
        }

        public Action<double, bool> Rotate { get; set; }

        private void Label2_Click(object sender, EventArgs e)
        {
            try {
                System.Diagnostics.Process.Start("http://fb.com/nucuoidocduocc");
            }
            catch (Exception) {
                MessageBox.Show("Error, Please contact nguyenphuongbka@gmail.com for more details.");
            }
        }
    }
}