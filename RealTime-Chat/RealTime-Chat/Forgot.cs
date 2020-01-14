using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Net;

namespace RealTime_Chat
{
    public partial class Forgot : Screen
    {
        public Forgot()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                db.Close();
                db.Open();
                cmd = new MySqlCommand("Select *From user where username  ='" + txtUsername.Text + "' AND email = '"+txtEmail.Text +"' AND secretanswer ='"+txtSecretanser.Text+"'", db);

                //Maps query results to table
                dr = cmd.ExecuteReader();

                //Ping googles ip
                Ping ping = new Ping();
                PingReply pingStatus = ping.Send(IPAddress.Parse("216.58.209.14"));

                if (pingStatus.Status == IPStatus.Success)
                {
                    //If any users were found
                    if (dr.Read())
                    {
                        //Check credentials
                        if (txtUsername.Text.ToString() == dr["username"].ToString())
                        {
                            if (txtEmail.Text.ToString() == dr["email"].ToString())
                            {
                                if (txtSecretanser.Text.ToString() == dr["secretanswer"].ToString())
                                {
                                    //Show password if correct
                                    MessageBox.Show("Your Password : " + dr["password"].ToString());
                                    db.Close();
                                    Login log = new Login();
                                    log.Show();
                                    this.Close();
                                    
                                }
                            }
                           
                        }
                        
                    }
                }
                //If the pinging failed, tell the user to check their internet connection
                else
                {
                    MessageBox.Show("Check your Internet connection.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    db.Close();
                }
            }
            //Some other error
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
