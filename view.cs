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
using System.Drawing.Imaging;
using UAC.UServe;

namespace UAC
{
    public partial class view : Form
    {
        public view()
        {
            InitializeComponent();
        }

        private void view_Load(object sender, EventArgs e)
        {
            try
            {
                UAC_OnlineList ul = new UAC_OnlineList();
                UServe.uServiceClient client = new uServiceClient();
                ul = client.UAC_PlayerList();
                foreach (string itm in ul.UList)
                {
                    cbPlayers.Items.Add((string)itm);
                }

                btnDelete.Visible = false;
            }
            catch
            { MessageBox.Show("There was an error. Try again in a few seconds."); }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            load();
        }

        private bool load()
        {
            //string sql = "";
            try
            {
                int i = 0;
                dgCaptures.Rows.Clear();
                uServiceClient client = new uServiceClient();
                
                UAC_SSList fl = client.UAC_LoadSSList(cbPlayers.Text,login.us.admin);
                foreach (int ssid in fl.SSId)
                {
                    var row = (DataGridViewRow)dgCaptures.RowTemplate.Clone();
                    row.CreateCells(dgCaptures, (object)ssid, (object)fl.SSDate[i]);
                    dgCaptures.Rows.Add(row);
                    i++;
                }
                btnDelete.Visible = false;
                pb1.ImageLocation = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error. Try again in a few seconds.");
            }
            return true;
        }
        private void dgCaptures_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string recordid = dgCaptures.Rows[e.RowIndex].Cells[0].Value.ToString();
                string imagename = "2y.jpg";
                string systemname = "2y.txt";
                lblID.Text = recordid;
                if (login.us.admin)
                {
                    btnDelete.Visible = true;
                }
                else
                {
                    btnDelete.Visible = false;
                }
                FileStream fs; // Writes the BLOB to a file (*.jpg).
                BinaryWriter bw; // Streams the BLOB to the FileStream object.
                int bufferSize = 100; // Size of the BLOB buffer.
                // The BLOB byte[] buffer to be filled by GetBytes.
                byte[] outbyte = new byte[bufferSize];
                //long retval; // The bytes returned from GetBytes.
                //long startIndex = 0; // The starting position in the BLOB output.
                fs = new FileStream(imagename, FileMode.OpenOrCreate, FileAccess.Write);
                bw = new BinaryWriter(fs);
                UAC_Bytes ub = new UAC_Bytes();
                uServiceClient client = new uServiceClient();
                ub = client.UAC_GetSS(Convert.ToInt32(recordid));
                bw.Write(ub.ImageData);
                bw.Flush();
                bw.Close();
                fs.Close();
                pb1.ImageLocation = imagename;

                fs = new FileStream(systemname, FileMode.OpenOrCreate, FileAccess.Write);
                bw = new BinaryWriter(fs);
                bw.Write(ub.SystemData);
                bw.Flush();
                bw.Close();
                fs.Close();
                txtProcesses.Text = File.ReadAllText(systemname);

            }
            catch (Exception ex)
            {
                MessageBox.Show("You must select a user and click View first.");
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cbPlayers.Items.Clear();
            UAC_OnlineList ul = new UAC_OnlineList();
            UServe.uServiceClient client = new uServiceClient();
            ul = client.UAC_PlayerList();
            foreach (string itm in ul.UList)
            {
                cbPlayers.Items.Add((string)itm);
            }
            dgCaptures.Rows.Clear();
            pb1.ImageLocation = "";
            btnDelete.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            uServiceClient client = new uServiceClient();
            client.UAC_DeleteSS(Convert.ToInt32(lblID.Text), login.us.guid, login.us.userid);
            pb1.ImageLocation = "";
            load();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "uacSS.jpg";
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                }
                pb1.Image.Save(sfd.FileName, format);
            }
        }
    }
}
