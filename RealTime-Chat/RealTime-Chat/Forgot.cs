using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Net;

namespace RealTime_Chat
{
    public partial class Forgot : Form
    {
        Random r = new Random();
        bool dragging = false;
        Point ilkkonum;
        public MySqlConnection db = new MySqlConnection("Server=localhost;Database=rtc;Uid=root;Pwd='';");
        public MySqlCommand cmd = new MySqlCommand();
        public MySqlDataAdapter adtr;
        public MySqlDataReader dr;
        public DataSet ds;

        public Forgot()
        {
            InitializeComponent();
        }

        #region MouseMove
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging) 
            {
                this.Left = e.X + this.Left - (ilkkonum.X);
                this.Top = e.Y + this.Top - (ilkkonum.Y);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true; 
            this.Cursor = Cursors.SizeAll; 
            ilkkonum = e.Location; 
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false; 
            this.Cursor = Cursors.Default; 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Close();
       
        }
        #endregion

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                db.Close();
                db.Open();
                cmd = new MySqlCommand("Select *From user where username  ='" + txtUsername.Text + "' AND email = '"+txtEmail.Text +"' AND secretanswer ='"+txtSecretanser.Text+"'", db);
                //Map results to table
                dr = cmd.ExecuteReader();
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
                else
                {
                    MessageBox.Show("Check your Internet connection.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    db.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
