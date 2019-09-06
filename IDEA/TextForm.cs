using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdeaClass;

namespace IDEA
{
    public partial class TextForm : Form
    {
        private Form1 _formControl;
        public TextForm(Form1 control)
        {
            _formControl = control;
            InitializeComponent();
        }

        private void TextForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formControl.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button2.Visible = false;
                label1.Text = @"Дешифрування";
            }
            else
            {
                button2.Visible = true;
                label1.Text = @"Шифрування"; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Encoding.UTF8.GetBytes(textBox1.Text).Length==16)
            {
                int lengs = Helper.GetLengsText(richTextBox1.Text);
                byte[] byFileData = Encoding.UTF8.GetBytes(richTextBox1.Text);
                Array.Resize(ref byFileData, lengs);
                var ideaclass = new Idea(textBox1.Text);
                for (int i = 0; i < lengs/8; i++)
                {
                    var temp = new byte[8];
                    Array.Copy(byFileData, i*8, temp, 0, 8);
                    ideaclass.SetData(temp);
                    ideaclass.Encryption();
                    var t = ideaclass.RetBin();
                    Array.Copy(t, 0, byFileData, i*8, 8);
                }
                string d = Convert.ToBase64String(byFileData);
                richTextBox2.Text = d;
            }
            else
            {
                MessageBox.Show(@"невірна довжина ключа");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Encoding.UTF8.GetBytes(textBox1.Text).Length == 16)
            {
                byte[] byFileData;
                try
                {
                   byFileData = Convert.FromBase64String(richTextBox1.Text);
                   var ideaclass = new Idea(textBox1.Text);
                   for (int i = 0; i < byFileData.Length / 8; i++)
                   {
                       var temp = new byte[8];
                       Array.Copy(byFileData, i * 8, temp, 0, 8);
                       ideaclass.SetData(temp);
                       ideaclass.Decryption();
                       var t = ideaclass.RetBin();
                       Array.Copy(t, 0, byFileData, i * 8, 8);
                   }
                   richTextBox2.Text = Encoding.UTF8.GetString(byFileData);
                }
                catch (FormatException Ex)
                {
                    MessageBox.Show(@"невірна строка даних" );
                }
                
                
            }
            else
            {
                MessageBox.Show(@"невірна довжина ключа");
            }
        }
    }
}
