using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
namespace RealTime_Chat
{
    public partial class Main : Screen
    {
        //Variables holding user info
        int logId, logStatus;
        string logUsername, logFullname;

        public Main()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage(txtMsg.Text);
        }

        //When the user presses the X button while in Chat Screen
        private void btnExit_Click(object sender, EventArgs e)
        {
            db.Close();
            cmd = new MySqlCommand();
            db.Open();
            cmd.Connection = db;
            //Set users status to offline
            cmd.CommandText = "Update user set status='" + 0 + "' where id=" + logId + "";
            cmd.ExecuteNonQuery();
            //Close the database connection and exit the application
            db.Close();
            Application.Exit();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Store user info in variables
            logId = Properties.Settings.Default.id;
            logUsername = Properties.Settings.Default.username;
            logFullname = Properties.Settings.Default.fullname;
            logStatus = Properties.Settings.Default.status;

            //List online users
            OnlineLists();

            Control.CheckForIllegalCrossThreadCalls = false;

            //Initialize the chat
            Thread chat = new Thread(new ThreadStart(AllMessageLists));
            chat.IsBackground = true;
            chat.Start();
        }

        //Lists all online users
        private void OnlineLists()
        {
            try
            {
                //Query users who are online, store the results in a table and append online users to
                //the lstOnline box
                db.Close();
                db.Open();
                cmd.Connection = db;
                cmd.CommandText = "SELECT * FROM user where status = 1";
                cmd.ExecuteNonQuery();
                dr = cmd.ExecuteReader();
                lstOnline.Items.Clear();

                //Append online users to text box
                while (dr.Read())
                {
                    lstOnline.Items.Add(dr["username"].ToString() + "  (" + dr["fullname"].ToString() +")");
                }

                db.Dispose();
                db.Close();
            }
            catch (Exception ex)
            { MessageBox.Show("lstOnline Error : " + ex.Message.ToString() ); }
        }
    
        //Lists all messages
        private void AllMessageLists()
        {
            while (true)
            {
                try
                {
                    db.Close();
                    db.Open();
                    cmd.Connection = db;

                    //Query all messages
                    cmd.CommandText = "SELECT * FROM message ORDER BY id ASC";
                    cmd.ExecuteNonQuery();
                    dr = cmd.ExecuteReader();
                    txtAll.Text = "";

                    //Append all messages to txtAll
                    while (dr.Read())
                    {
                        txtAll.Text += "(" + dr["time"].ToString() + " / " + dr["fullname"].ToString() + ") :  " + dr["msg"].ToString() +"\n\n";
                    }

                    txtAll.SelectionStart = txtAll.TextLength;
                    txtAll.ScrollToCaret();
                    db.Dispose();
                    db.Close();
                    OnlineLists();
                    Thread.Sleep(500);

                }
                //There was an error during queries
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        //Insert a message into the message table in the database
        private void SendMessage(String mssg)
        {
            try
            {
                db.Close();
                cmd.Connection = db;
                cmd.CommandText = "Insert Into message(time,fullname,msg) Values ('"
                  + DateTime.Now.ToLongTimeString() + "','"
                  + logFullname + "','"
                  + mssg + "')";
                db.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                db.Close();
                txtMsg.Text = "";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString());
            }
        }
    }
}
