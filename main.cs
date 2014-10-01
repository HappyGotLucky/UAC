using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using UAC.UServe;

namespace UAC
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = false;
            backgroundWorker1.WorkerSupportsCancellation = false;
            backgroundWorker2.WorkerReportsProgress = false;
            backgroundWorker2.WorkerSupportsCancellation = false;
            // To report progress from the background worker we need to set this property
            backgroundWorker3.WorkerReportsProgress = true;
            // This event will be raised when we call ReportProgress
            backgroundWorker3.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker3_ProgressChanged);
        }
        public int ss = 0;
        public int sss = 0;
        public string[] sstime = new string[1000];
        public int exitapp = 0;

        void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            timer1.Stop();
            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < ss; i++)
            {
                // Report progress to 'UI' thread
                backgroundWorker3.ReportProgress(i+1);
                // Upload Shot
                endsession(i);
            }
        }
        // Back on the 'UI' thread so we can update the progress bar
        void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProcessing.Text = "Uploading " + e.ProgressPercentage.ToString() + " of " + ss.ToString();
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown. 
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                // Finally, handle the case where the operation  
                // succeeded.
                ss=0;
                lblCap.Text = "Screens Pending: 0";
                lblProcessing.Text = "Upload Status: Waiting";
                Cursor.Current = Cursors.Default;
                if (exitapp == 1)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                else
                {
                    timer1.Start();
                }
            }
        }

        private void main_OnClose(object sender, EventArgs e)
        {
            if (ss > 0)
            {
                timer1.Stop();
                exitapp = 1;
                if (backgroundWorker3.IsBusy != true)
                {
                    backgroundWorker3.RunWorkerAsync();
                }
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            if (ss > 0)
            {
                timer1.Stop();
                exitapp = 1;
                if (backgroundWorker3.IsBusy != true)
                {
                    backgroundWorker3.RunWorkerAsync();
                }
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        public bool stop()
        {
            timer1.Stop();
            lblStatus.Text = "Waiting for game to start...";
            return true;
        }
        public bool start()
        {
            timer1.Start();
            lblStatus.Text = "Game running - Anti-Cheat On!";
            return true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int rndnum = rnd.Next(1, 20);
            timer1.Interval = 10000 * rndnum;
            // capture this window, and save it
            if (backgroundWorker2.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker2.RunWorkerAsync();
            }
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = take_ss();
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                ss++;
                lblCap.Text = "Screens Pending: " + ss.ToString();
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            view vw = new view();
            vw.Visible = true;
        }
        //private static Bitmap bmpScreenshot;
        //private static Graphics gfxScreenshot;

        bool take_ss()
        {
            string ssfile = ss.ToString() + ".jpg";
            string prfile = ss.ToString() + ".txt";
            sstime[ss] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ScreenCapture sc = new ScreenCapture();
            sc.CaptureScreenToFile(ssfile, ImageFormat.Jpeg);
            sc = null;

            FileStream fs1 = new FileStream(prfile, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs1);
            writer.WriteLine("Process list for user: " + login.us.username);
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                writer.WriteLine(theprocess.ProcessName);
            }
            writer.Close();
            fs1.Close();

            return true;
        }

        bool endsession(int i)
        {
            FileStream fs;
            BinaryReader br;
            string ssfile = ss.ToString() + ".jpg";
            string prfile = ss.ToString() + ".txt";
            try
            {
                ssfile = i.ToString() + ".jpg";
                prfile = i.ToString() + ".txt";
                byte[] ImageData;
                byte[] SystemData;
                fs = new FileStream(ssfile, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                ImageData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                fs = new FileStream(prfile, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                SystemData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                uServiceClient client = new uServiceClient();
                UAC_Bytes ub = new UAC_Bytes();
                ub.ImageData = ImageData;
                ub.SystemData = SystemData;
                ub.Time = sstime[i];
                ub.User_Id = login.us.userid;
                client.UAC_UploadSS(ub);
            }
            catch (Exception ex)
            { return false; }
            return true;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ss == 0 && exitapp == 1)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                string procname = "";
                bool game_running = false;
                Process[] processlist = Process.GetProcesses();
                foreach (Process theprocess in processlist)
                {
                    procname = theprocess.ProcessName;
                    if (procname == "bf4" || procname == "bf4_x86")
                    {
                        game_running = true;
                    }
                    else if (game_running == false)
                    {
                        game_running = false;
                    }
                }
                if (game_running == true && lblStatus.Text == "Waiting for game to start...")
                {
                    ss = 0;
                    start();
                }
                else if (game_running == false)
                {
                    stop();
                    if (ss > 0)
                    {
                        if (ss > 0)
                        {
                            if (backgroundWorker3.IsBusy != true)
                            {
                                backgroundWorker3.RunWorkerAsync();
                            }
                        }
                    }
                }
                if (backgroundWorker1.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch
            {
                //Bad
            }
        }

        private void main_Load(object sender, EventArgs e)
        {
            timer2.Start();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = update_online_list();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown. 
            if (e.Error != null)
            {
                //MessageBox.Show(e.Error.Message);
            }
            else
            {
                // Finally, handle the case where the operation  
                // succeeded.
                lbOnline.Items.Clear();
                foreach (string admin in (List<string>)e.Result)
                {
                    lbOnline.Items.Add(admin);
                }
                lblOnline.Text = "Who's Online: (" + lbOnline.Items.Count.ToString() + ")";
            }
        }

        List<string> update_online_list()
        {
            List<string> result = new List<string>();
            data da = new data();
            da.iamonline();
            da = null;
            result.Clear();
            uServiceClient client = new uServiceClient();
            UAC_OnlineList ol = client.UAC_RetrieveOnlineUsers();
            //ol = client.UAC_RetrieveOnlineUsers(ol);
            foreach (string itm in ol.UList)
            {
                result.Add((string)itm);
            }
            return result;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (ss > 0)
            {
                backgroundWorker3.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("There is nothing to upload.");
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }
    }
}
