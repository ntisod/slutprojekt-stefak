using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Net;

namespace RealTime_Chat
{
    public partial class Login : Screen
    {
        public Login()
        {
            InitializeComponent();
        }

        //Shows register screen and hides login screen
        private void btnRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SignIn(txtUsername.Text, txtPassword.Text);
        }

        #region SignIn
        private void SignIn(String username, String password)
        {

            try
            {
                db.Close();
                db.Open();
                //Find the specified user
                cmd = new MySqlCommand("Select *From user where username  ='" + username + "'", db);
                dr = cmd.ExecuteReader();
                //Make sure that the credentials aren't empty
                if (username.Trim() != "" && password.Trim() != "")
                {

                    Ping ping = new Ping();
                    PingReply pingStatus = ping.Send(IPAddress.Parse("216.58.209.14"));

                    if (pingStatus.Status == IPStatus.Success)
                    {
                        if (dr.Read())
                        {
                            //Authorize user
                            if (username.ToString() == dr["username"].ToString())
                            {
                                if (password.ToString() == dr["password"].ToString())
                                {
                                    //Store user information while logged in (until the program is closed)
                                    String id = dr["id"].ToString();
                                    Properties.Settings.Default.id = Convert.ToInt16(dr["id"]);
                                    Properties.Settings.Default.username = dr["username"].ToString();
                                    Properties.Settings.Default.password = dr["password"].ToString();
                                    Properties.Settings.Default.email = dr["email"].ToString();
                                    Properties.Settings.Default.fullname = dr["fullname"].ToString();
                                    Properties.Settings.Default.secretanswer = dr["secretanswer"].ToString();
                                    Properties.Settings.Default.status = Convert.ToInt16(dr["status"]);
                                    Properties.Settings.Default.Save();

                                    db.Close();
                                    cmd = new MySqlCommand();
                                    db.Open();
                                    cmd.Connection = db;

                                    //Set status to online
                                    cmd.CommandText = "Update user set status='" + 1 + "' where id=" + id + "";
                                    cmd.ExecuteNonQuery();

                                    //Close the database connection and show Chat Screen
                                    db.Close();
                                    Main main = new Main();
                                    main.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MessageBox.Show("Your password is missing or incorrect..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    db.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Your username is missing or incorrect.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                db.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No such user.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            db.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Check your Internet connection.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        db.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        //Shows forgot screen and hides login screen
        private void btnForgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Forgot forgot = new Forgot();
            forgot.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {
        }
    }
}
