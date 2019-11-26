using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace picture
{
    public partial class Form2 : Form
    {
        private int index = 0;
        private string[] files;
        private Bitmap myBitmap,srcBitmap;
        private Image myImage;
        private string path;
        public Form2(FilePath fp)
        {
            InitializeComponent();
            srcBitmap = new Bitmap(fp.filepath);
            myBitmap = new Bitmap(srcBitmap, pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = myBitmap;
            DirectoryInfo dir = new DirectoryInfo(fp.path);
            FileInfo[] dirInfos = dir.GetFiles();
            files = new string[dirInfos.Length];
            this.Text = "图片" + fp.filepath;
            for (int i = 0; i < dirInfos.Length; i++)
            {
                    if (fp.filepath.Equals(dirInfos[i].FullName))
                    {
                        index = i;
                    }
                    files[i] = dirInfos[i].FullName;
            }
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.AutoMouseWheel);
            myImage = pictureBox1.Image;
            path = Path.GetDirectoryName(fp.filepath);
        }
        private void AutoMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            myImage = pictureBox1.Image;
            if (e.Delta > 0)myBitmap = new Bitmap(myImage, myImage.Width * 2, myImage.Height * 2);
            else myBitmap = new Bitmap(myImage, myImage.Width/2, myImage.Height /2);
            pictureBox1.Image = myBitmap;
        }
        private void Button1_Click放大(object sender, EventArgs e)
        {
            srcBitmap = new Bitmap(files[index]);
            myBitmap = new Bitmap(srcBitmap, pictureBox1.Image.Width * 2, pictureBox1.Image.Height * 2);
            pictureBox1.Image = myBitmap;
        }
        private void Button2_Click缩小(object sender, EventArgs e)
        {
            srcBitmap = new Bitmap(files[index]);
            myBitmap = new Bitmap(srcBitmap, pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2);
            pictureBox1.Image = myBitmap;
        }
        private void Button3_Click下一张(object sender, EventArgs e)
        {
            index = (index + 1) % files.Length;
            srcBitmap = new Bitmap(files[index]);
            if (radioButton1.Checked)
            {
                myBitmap = new Bitmap(srcBitmap, srcBitmap.Size.Width, srcBitmap.Size.Height);
            }
            else myBitmap = new Bitmap(srcBitmap, pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = myBitmap;
            this.Text = "图片" + files[index];
        }
        private void Button4_Click上一张(object sender, EventArgs e)
        {
            if (index == 0) { index = files.Length - 1; }
            else index = (index - 1) % files.Length;
            srcBitmap = new Bitmap(files[index]);
            if (radioButton1.Checked)
            {
                myBitmap = new Bitmap(srcBitmap, srcBitmap.Size.Width, srcBitmap.Size.Height);
            }
            else myBitmap = new Bitmap(srcBitmap, pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = myBitmap;
            this.Text = "图片" + files[index];
        }
        private void Button5_Click逆时针(object sender, EventArgs e)
        {
            myImage = pictureBox1.Image;
            myImage.RotateFlip(RotateFlipType.Rotate90FlipXY); //调用RotateFlip方法将JPG格式图像进行旋转            
            myBitmap = new Bitmap(myImage, myImage.Width , myImage.Height );
            pictureBox1.Image = myBitmap;
        }
        private void Button6_Click顺时针(object sender, EventArgs e)
        {
            myImage = pictureBox1.Image;
            myImage.RotateFlip(RotateFlipType.Rotate270FlipXY); //调用RotateFlip方法将JPG格式图像进行旋转            
            myBitmap = new Bitmap(myImage, myImage.Width, myImage.Height);
            pictureBox1.Image = myBitmap;
        }
        private void Button7_Click幻灯片(object sender, EventArgs e)
        {
            pictureBox1.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;     //设置窗体为无边框样式
            this.WindowState = FormWindowState.Maximized;    //最大化窗体 
            srcBitmap = new Bitmap(files[index]);
            myBitmap = new Bitmap(srcBitmap, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = myBitmap;
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();//开始
            this.panel1.Visible=false;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(image);
            imgGraphics.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            myBitmap = new Bitmap(image, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = myBitmap;
            string dateTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            myBitmap.Save(path +"\\"+ dateTime + ".jpg");
        }
        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            srcBitmap = new Bitmap(files[index]);
            myBitmap = new Bitmap(srcBitmap, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = myBitmap;
        }
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            srcBitmap = new Bitmap(files[index]);
            myBitmap = new Bitmap(srcBitmap, srcBitmap.Size.Width, srcBitmap.Size.Height);
            pictureBox1.Image = myBitmap;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            index = (index + 1) % files.Length;
            srcBitmap = new Bitmap(files[index]);
            myBitmap = new Bitmap(srcBitmap, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = myBitmap;
            timer1.Interval = 2000;//设置timer1控件时间间隔为2000ms即2秒 
        }
        private void form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                timer1.Enabled = false;
                pictureBox1.Dock = DockStyle.None;
                this.FormBorderStyle = FormBorderStyle.Sizable;  //设置窗体为普通样式
                this.WindowState = FormWindowState.Normal;
                srcBitmap = new Bitmap(files[index]);
                myBitmap = new Bitmap(srcBitmap, srcBitmap.Size.Width, srcBitmap.Size.Height);
                pictureBox1.Image = myBitmap;
                this.panel1.Visible = true;
            }
            if (e.KeyCode == Keys.F1)
            {
                timer1.Enabled = false;

            }
            if (e.KeyCode == Keys.F2)
            {
                timer1.Enabled = true;
            }
        }
    }
}
