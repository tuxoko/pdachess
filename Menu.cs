using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace pdachess
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text == "在此輸入使用者名稱") || (textBox1.Text == null) || textBox1.Text.Contains(' '))
            {
                MessageBox.Show("請輸入使用者名稱");
            }
            else
            {
                Form1 gamelocal = new Form1("Network", textBox1.Text);
                try
                {
                    gamelocal.Owner = this;
                    gamelocal.Show();
                    this.Hide();
                }
                catch { }
            }
        }


        private void menuItem3_Click(object sender, EventArgs e)
        {
            Form1 gamelocal = new Form1("Local",null);
            gamelocal.Owner = this;
            gamelocal.Show();
            this.Hide();
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            if (textBox1.Text == "在此輸入使用者名稱")
                textBox1.Text = "";
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "在此輸入使用者名稱";
            }
        }
    }
}