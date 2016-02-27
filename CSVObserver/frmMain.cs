using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace CSVObserver
{
    public partial class frmMain : Form
    {
        FileInfo file;
        string[] content;

        void UpdateDGV() 
        {
            if (file.Exists)
            {
                //Alle Zeilen
                content = File.ReadAllLines(file.FullName);

                //Spalten hinzufügen.
                for (int i = 0; i < content[0].Split(';').Length; i++)
                {
                    dataGridView1.Columns.Add(i.ToString(), i.ToString());
                }

                for (int i = 0; i < content.Length; i++)
                {
                    dataGridView1.Rows.Add(content[i].Split(';'));
                } 
            }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void chkBxFswEnable_CheckedChanged(object sender, EventArgs e)
        {
            fileSystemWatcher.EnableRaisingEvents = chkBxFswEnable.Checked;

            if (chkBxFswEnable.Checked)
            {
                chkBxFswEnable.Text = "ONLINE";
                fileSystemWatcher.Path = file.DirectoryName;
                fileSystemWatcher.Filter = file.Name; 
            }
            else
            {
                chkBxFswEnable.Text = "OFFLINE";
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files|*.csv";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                file = new FileInfo(ofd.FileName);
                lblCurrentFile.Text = file.FullName;

                if (file != null)
                {
                    UpdateDGV();
                    chkBxFswEnable.Enabled = true;
                }
                else
                {
                    chkBxFswEnable.Enabled = false;
                }
                
            }
            else
            {
                chkBxFswEnable.Enabled = false;
            }

            
        }

        private void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            Thread.Sleep(100);
            UpdateDGV();
        }
    }
}
