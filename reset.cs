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
    public partial class reset : Form
    {
        public reset()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            bool IsOk = true;

            if (txtConfirm.Text!=txtPass.Text)
            {
                IsOk = false;
                lblError.Text = "Passwords do not match!";
            }

            if (IsOk==true)
            {
                //Reset Password
                data da = new data();
                da.resetpassword(txtPass.Text);
                MessageBox.Show("Password Reset. Please log back in!");
                login frmLogin = new login();
                frmLogin.Show();
                this.Visible = false;
                this.Close();

            }
        }
    }
}
