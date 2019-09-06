using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDEA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private FileForm FileForm;
        private TextForm TextForm;
        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            TextForm = new TextForm(this);
            TextForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            FileForm = new FileForm(this);
            FileForm.Show();
        }
    }
}
