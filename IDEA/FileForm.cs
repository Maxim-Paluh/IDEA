using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdeaClass;
namespace IDEA
{
    public partial class FileForm : Form
    {
        private Form1 _formControl;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog SaveFileDialog;
        private BackgroundWorker workerEncryption;
        private BackgroundWorker workerDecryption;
        
        public FileForm(Form1 control)
        {
            workerEncryption = new BackgroundWorker();
            workerDecryption = new BackgroundWorker();
            workerEncryption.DoWork += workerEncryption_DoWork;
            workerDecryption.DoWork += workerDecryption_DoWork;


            workerEncryption.RunWorkerCompleted += workerEncryption_RunWorkerCompleted;
            workerDecryption.RunWorkerCompleted += workerDecryption_RunWorkerCompleted;
            _formControl = control;
            openFileDialog1 = new OpenFileDialog();
            SaveFileDialog = new SaveFileDialog();
            InitializeComponent();
            button3.BringToFront();
            label1.BringToFront();
        }

        void workerDecryption_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            checkBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        void workerEncryption_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            checkBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

      
       

        private void FileForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _formControl.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
               openFileDialog1.Filter = @"Idea files (*.idea)|*.idea";
            }
            else
            {
                openFileDialog1.Filter = @"All files (*.*)|*.*";
            }
                
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        textBox1.Text = openFileDialog1.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"error:" + ex.Message);
                    }
                }
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
              if (checkBox1.Checked)
                {
                    SaveFileDialog.Filter = @"all files (*.*)|*.*";
                    var str = openFileDialog1.FileName.Replace(".idea", "");
                    SaveFileDialog.FileName = str;
                }
                else
                {
                    SaveFileDialog.Filter = @"Idea files (*.idea)|*.idea";
                    SaveFileDialog.FileName = openFileDialog1.FileName + ".idea";
                }
                SaveFileDialog.FilterIndex = 1;
                SaveFileDialog.RestoreDirectory = true;
                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        textBox2.Text = SaveFileDialog.FileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"error:" + ex.Message);
                    }
                }

       
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (Encoding.UTF8.GetBytes(textBox3.Text).Length==16)
            {
                if (!workerEncryption.IsBusy)
                {
                    checkBox1.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    workerEncryption.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(@"Зачекайте іде обробка файлу");
                }
            }
            else
            {
                MessageBox.Show(@"невірна довжина ключа");
            }
          
           
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (Encoding.UTF8.GetBytes(textBox3.Text).Length == 16)
            {
                if (!workerDecryption.IsBusy)
                {
                    checkBox1.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    workerDecryption.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(@"Зачекайте іде обробка файлу");
                }
            }
            else
            {
                MessageBox.Show(@"невірна довжина ключа");
            }

        }
        void workerEncryption_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(() => progressBar1.Visible = true));
                Invoke(new MethodInvoker(() => progressBar1.Minimum = 0));
                Invoke(new MethodInvoker(() => progressBar1.Value = 0));
                int[] lengs = Helper.GetLengs(textBox1.Text);
                Invoke(new MethodInvoker(() => progressBar1.Maximum = lengs[0] / 8));
                byte[] byFileData = Helper.ReadLocalFile(textBox1.Text);
                if (lengs[1] == 0)
                {
                    lengs[0]++;
                    Array.Resize(ref byFileData, lengs[0]);
                }
                else
                {
                    Array.Resize(ref byFileData, lengs[0]);
                    byFileData[lengs[0] - 1] = Convert.ToByte(lengs[1]);
                }
                var ideaclass = new Idea(textBox3.Text);
                DateTime localDate = DateTime.Now;
                for (int i = 0; i < lengs[0] / 8; i++)
                {
                    var temp = new byte[8];
                    Array.Copy(byFileData, i * 8, temp, 0, 8);
                    ideaclass.SetData(temp);
                    ideaclass.Encryption();
                    var t = ideaclass.RetBin();
                    Array.Copy(t, 0, byFileData, i * 8, 8);
                    if (checkBox2.Checked)
                    {
                        if (i%1000==0)
                        {
                            Invoke(new MethodInvoker(() => progressBar1.Value = i));
                        }
                    }

                }
                Invoke(new MethodInvoker(() => progressBar1.Value = 0));
                DateTime localDate2 = DateTime.Now;
                System.TimeSpan date = localDate2.Subtract(localDate);
                MessageBox.Show(String.Format("Шифрування завершено, швидкість шифрування: {0:0.00} MB/sec", ((double)lengs[0] / 1048576) / date.TotalSeconds));
                Helper.retFile(byFileData, textBox2.Text);
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
            
        }

        void workerDecryption_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Invoke(new MethodInvoker(() => progressBar1.Visible = true));
                Invoke(new MethodInvoker(() => progressBar1.Minimum = 0));
                Invoke(new MethodInvoker(() => progressBar1.Value = 0));
                int[] lengs = Helper.GetLengs(textBox1.Text);
                lengs[0] = lengs[0] - lengs[1];
                Invoke(new MethodInvoker(() => progressBar1.Maximum = lengs[0] / 8));
                byte[] byFileData = Helper.ReadLocalFile(textBox1.Text);
                var ideaclass = new Idea(textBox3.Text);
                DateTime localDate = DateTime.Now;
                for (int i = 0; i < lengs[0] / 8; i++)
                {
                    var temp = new byte[8];
                    Array.Copy(byFileData, i * 8, temp, 0, 8);
                    ideaclass.SetData(temp);
                    ideaclass.Decryption();
                    var t = ideaclass.RetBin();
                    Array.Copy(t, 0, byFileData, i * 8, 8);
                    if (checkBox2.Checked)
                    {
                        if (i % 1000 == 0)
                        {
                            Invoke(new MethodInvoker(() => progressBar1.Value = i));
                        }
                    }

                }
                Invoke(new MethodInvoker(() => progressBar1.Value = 0));
                DateTime localDate2 = DateTime.Now;
                System.TimeSpan date = localDate2.Subtract(localDate);
                MessageBox.Show(string.Format("Дешифрування завершено, швидкість дешифрування:  {0:0.00} MB/sec", ((double)lengs[0] / 1048576) / date.Seconds));

                if (byFileData.Length%8==1)
                {
                    Array.Resize(ref byFileData,byFileData.Length-1);
                }
                else
                {
                    int temp = byFileData[byFileData.Length - 1];
                    Array.Resize(ref byFileData, byFileData.Length - temp);
                }
                Helper.retFile(byFileData, textBox2.Text);
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
                if (checkBox1.Checked)
                {
                    button3.Visible = false;
                    label1.Visible = false;
                }
                else
                {
                    button3.Visible = true;
                    label1.Visible = true;
                }
            SaveFileDialog.FileName = "";
            openFileDialog1.FileName = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }
 
        }

       

       
    }
