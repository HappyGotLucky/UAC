using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UAC
{
    public partial class create_clan : Form
    {
        public create_clan()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            int error = 0;
            if (txtName.Text == "")
            {
                error = 1;
                lblError.Text = "Organization Name is required";
            }
            if (error == 0 && txtLeader.Text == "")
            {
                error = 1;
                lblError.Text = "An owner/leader must be entered.";
            }
            if (error == 0 && txtWeb.Text == "")
            {
                error = 1;
                lblError.Text = "Group website must be entered.";
            }
            if (!txtEmail.Text.Contains("@") && error == 0)
            {
                error = 1;
                lblError.Text = "Must enter a valid email address!";
            }
            if (!txtEmail.Text.Contains(".") && error == 0)
            {
                error = 1;
                lblError.Text = "Must enter a valid email address!";
            }
            if (error == 0 && txtPassword.Text == "")
            {
                error = 1;
                lblError.Text = "A password must be entered.";
            }
            if (error == 0 && txtEmail.Text != txtConfirm.Text)
            {
                error = 1;
                lblError.Text = "E-mails do not match!";
            }
            if (error == 0 && txtPassword.Text != txtConfirmPass.Text)
            {
                error = 1;
                lblError.Text = "Passwords do not match!";
            }

            if (error == 0)
            {
                data da = new data();
                //da.create_clan(txtName.Text, txtEmail.Text, txtLeader.Text, txtWeb.Text, txtPassword.Text);
                MessageBox.Show("Organization has been registered. Please contact HappyGotLucky on TeamSpeak (ts.urockgamers.org) for activation.");
                this.Close();
            }
        }

        private void create_clan_Load(object sender, EventArgs e)
        {

        }
    }
}
