using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace UAC
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        public static user us = new user();
        private void main_OnClose(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool isOk = true;

                if (txtUsername.Text == "")
                {
                    isOk = false;
                    lblError.Text = "You must enter a username.";
                    lblError.Visible = true;
                }
                if (txtPassword.Text == "" && isOk == true)
                {
                    isOk = false;
                    lblError.Text = "You must enter a password.";
                    lblError.Visible = true;
                }
                data da = new data();
                us = da.account_login(txtUsername.Text, txtPassword.Text);
                if (us.needs_reset == 1)
                {
                    txtPassword.Text = "";
                    reset freset = new reset();
                    freset.Show();
                    this.Visible = false;
                }
                else if (us.userid > 0 && isOk == true)
                {
                    if (cbSave.Checked)
                    {
                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["Username"].Value = txtUsername.Text;
                        config.AppSettings.Settings["Password"].Value = txtPassword.Text;
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    else
                    {
                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        config.AppSettings.Settings["Username"].Value = "";
                        config.AppSettings.Settings["Password"].Value = "";
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }

                    //Load Main
                    this.Visible = false;
                    main form1 = new main();
                    form1.Visible = true;
                    this.Visible = false;
                    
                }
                else if (isOk == true)
                {
                    lblError.Text = "Invalid username or password.";
                    lblError.Visible = true;
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool isOk = true;

            if (txtUsername.Text == "")
            {
                isOk = false;
                lblError.Text = "You must enter a username.";
                lblError.Visible = true;
            }
            if (txtPassword.Text == "" && isOk == true)
            {
                isOk = false;
                lblError.Text = "You must enter a password.";
                lblError.Visible = true;
            }
            data da = new data();
            us = da.account_login(txtUsername.Text, txtPassword.Text);
            if (us.needs_reset == 1)
            {
                txtPassword.Text = "";
                reset freset = new reset();
                freset.Show();
                this.Visible = false;
            }
            else if (us.userid > 0 && isOk == true)
            {
                if (cbSave.Checked)
                {
                    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["Username"].Value = txtUsername.Text;
                    config.AppSettings.Settings["Password"].Value = txtPassword.Text;
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                else
                {
                    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.AppSettings.Settings["Username"].Value = "";
                    config.AppSettings.Settings["Password"].Value = "";
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                //Load Main
                this.Visible = false;
                main form1 = new main();
                form1.Visible = true;
                this.Visible = false;
            }
            else if (isOk == true)
            {
                lblError.Text = "Invalid username or password.";
                lblError.Visible = true;
            }
        }

        private void login_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["Username"] != "")
            {
                cbSave.Checked = true;
                txtUsername.Text = ConfigurationManager.AppSettings["Username"];
                txtPassword.Text = ConfigurationManager.AppSettings["Password"];
            }

        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            create_clan cc = new create_clan();
            cc.Visible = true;
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            about abt = new about();
            abt.Visible = true;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            create_account ca = new create_account();
            ca.Visible = true;
        }

        private void btnForgot_Click(object sender, EventArgs e)
        {
            forgot ca = new forgot();
            ca.Visible = true;

        }
    }
}
