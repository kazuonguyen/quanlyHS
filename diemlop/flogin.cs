using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diemlop
{
    public partial class flogin : Form
    {
        public flogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "chpnh" && textBox2.Text == "3141592654")
            {
                this.Hide();
                var fchinh = new fchinh();
                fchinh.Closed += (s, args) => this.Close();
                fchinh.Show();
            }
            else
            {
                string message = "Bạn chưa nhập đày đủ";
                string title = "Cảnh báo";
                MessageBox.Show(message, title);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                textBox2.Focus();
            }
        }
    }
}
