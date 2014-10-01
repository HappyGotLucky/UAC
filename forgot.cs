using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using UAC.UServe;

namespace UAC
{
    public partial class forgot : Form
    {
        public forgot()
        {
            InitializeComponent();
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBox.Show("You must enter your e-mail address.");
            }
            else
            {
                uServiceClient client = new uServiceClient();
                UAC_UserInfo ui = new UAC_UserInfo();
                ui.Email = txtUsername.Text;
                ui = client.UAC_ForgotPassword(ui);
                if (ui.SQLResult=="1")
                {
                    MessageBox.Show("Your details have been sent to your e-mail address.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No account was found for that e-mail address");
                }
            }
        }
    }
}
