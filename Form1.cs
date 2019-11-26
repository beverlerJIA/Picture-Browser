using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
namespace picture
{
    public partial class Form1 : Form
    {
        FileInfo[] files;
        string path;
        public Form1()
        {
            InitializeComponent();
            InitTreeView();
            treeView1.ExpandAll();
        }
        public void InitTreeView()
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();
            TreeNode computer = new TreeNode("计算机");
            computer.ImageIndex = 0;
            treeView1.Nodes.Add(computer);
            foreach (DriveInfo df in drivers)
            {
                TreeNode node = new TreeNode();
                node.Text = "[本地磁盘(" + df.Name.Substring(0, 2) + ")]";
                node.Tag = df.Name;
                computer.Nodes.Add(node);
            }
        }
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode root = treeView1.SelectedNode;
                path = (string)root.Tag;
                if (path != null)
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    DirectoryInfo[] dirInfos = dir.GetDirectories();
                    foreach (DirectoryInfo d in dirInfos)
                    {
                        TreeNode n = new TreeNode();
                        n.ImageIndex = 0;
                        n.Text = d.Name;
                        n.Tag = d.FullName;
                        root.Nodes.Add(n);
                    }
                    this.flowLayoutPanel1.Controls.Clear();                   
                    files = dir.GetFiles();
                    addPictures();
                    Text = "图片浏览器"+ path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void addPictures()
        {
            foreach (FileInfo file in files)
            {
                string fullname = file.FullName;
                if (fullname.Contains(".jpg") || fullname.Contains(".bmp") || fullname.Contains(".jpeg"))
                {
                    PictureBox box = new PictureBox();
                    FilePath fp = new FilePath(path, fullname);
                    box.Tag = fp;
                    box.Width = 100;
                    box.Height = 100;
                    Image image = Image.FromFile(fullname);
                    Image thumbImage=Image.FromFile(fullname);
                    thumbImage = image.GetThumbnailImage(100, 100, null, IntPtr.Zero); 
                    box.Image = thumbImage;
                    box.Click += this.PictureBox1_Click;                 
                    this.flowLayoutPanel1.Controls.Add(box);
                }
            }
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            FilePath fp = (FilePath)box.Tag;
            Form2 sp = new Form2(fp);
            sp.StartPosition = FormStartPosition.CenterScreen;
            sp.ShowDialog();
        }

        private void FlowLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
