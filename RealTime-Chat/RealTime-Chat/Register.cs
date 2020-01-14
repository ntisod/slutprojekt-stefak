using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Net;

namespace RealTime_Chat
{
    public partial class Register : Screen
    {
        public Register()
        {
            //winforms required
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //adds a new entry to the database
            Create(txtUsername.Text, txtPassword.Text, txtEmail.Text, txtFullname.Text, txtSecretanswer.Text);
        }
        #region RegisterForm
        private void Create(String username, String password, String email, String fullname, String secretanswer)
        {
            //check credential validity
            if (txtUsername.Text.Trim() != "" &&
             txtPassword.Text.Trim() != "" &&
             txtEmail.Text.Trim() != "" &&
             txtFullname.Text.Trim() != "" &&
             txtSecretanswer.Text.Trim() != "")
            {

                Ping ping = new Ping();
                // ping connectiın google
                PingReply pingStatus = ping.Send(IPAddress.Parse("216.58.209.14")); 
                //Check whether the ping was successful (not needed for local db but why not
                if (pingStatus.Status == IPStatus.Success)
                {
                    try
                    {
                        //If all went ok, try inserting a new user entry into the database
                        db.Close();
                        db.Open();
                        cmd = new MySqlCommand("Insert Into user(username,password,email,fullname,secretanswer,status) Values ('"
                           + username.Trim() + "','"
                           + password.Trim() + "','"
                           + email.Trim() + "','"
                           + fullname.Trim() + "','"
                           + secretanswer.Trim() + "','"
                           + 0 + "')", db);


                        object result = null;
                        result = cmd.ExecuteNonQuery();
                        db.Close();
                        //If the result object isn't null, the process was successfull
                        if (result != null)
                        {
                            MessageBox.Show("successful", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUsername.Text = "";
                            txtPassword.Text = "";
                            txtEmail.Text = "";
                            txtFullname.Text = "";
                            txtSecretanswer.Text = "";
                            this.Close();
                            Login log = new Login();
                            log.Show();
                            db.Close();
                        }
                        else
                        {
                            //The result object was null, the process was not sucessfull
                            MessageBox.Show("Could not add to add.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            db.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Take another email or username", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(ex.Message.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        db.Close();
                    }
                }
                else
                {
                    //If the pinging failed, display this
                    MessageBox.Show("Check your internet connection.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    db.Close();
                }
            }
            else
            {
                //If any of the provided credentials were empty
                MessageBox.Show("can not be empty.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                db.Close();
            }

        }
        #endregion
    }
}