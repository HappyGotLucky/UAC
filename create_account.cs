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
    public partial class create_account : Form
    {
        public create_account()
        {
            InitializeComponent();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            int okToSubmit = 1;
            if (txtPlayer.Text == "")
            {
                okToSubmit = 0;
                lblError.Text = "Must enter player name!";
            }
            if (txtPassword.Text == "" && okToSubmit == 1)
            {
                okToSubmit = 0;
                lblError.Text = "Must enter a password!";
            }
            if (txtPassword.Text.Length < 6)
            {
                okToSubmit = 0;
                lblError.Text = "Password must be at least 6 characters";
            }
            if (!txtEmail.Text.Contains("@") && okToSubmit == 1)
            {
                okToSubmit = 0;
                lblError.Text = "Must enter a valid email address!";
            }
            if (!txtEmail.Text.Contains(".") && okToSubmit == 1)
            {
                okToSubmit = 0;
                lblError.Text = "Must enter a valid email address!";
            }

            if (chkAgree.Checked == false)
            {
                okToSubmit = 0;
                lblError.Text = "You must agree to the terms and conditions prior to registering your account.";
            }

            data da = new data();
            if (da.account_exists(txtEmail.Text) && okToSubmit == 1)
            {
                okToSubmit = 0;
                lblError.Text = "Account with this e-mail address already exists.";
            }

            if (okToSubmit == 0)
            {
                MessageBox.Show("There were errors with your submission. Please check what you've entered and try again.");
            }
            else
            {
                int clan = 1;
                da.account_create(txtPlayer.Text, txtPassword.Text, txtEmail.Text, clan);
                MessageBox.Show("Account created. Please log in!");
                this.Visible = false;
            }
        }

        private void create_account_Load(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPlayer_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void chkAgree_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblError_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
