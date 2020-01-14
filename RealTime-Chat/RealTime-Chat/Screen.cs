using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealTime_Chat
{
    public class Screen : Form
    {
        //Is the windows being dragged
        protected bool dragging = false;
        //Drag position
        protected Point ilkkonum;
        
        //MySql related variables
        public MySqlConnection db = new MySqlConnection("Server=localhost;Database=rtc;Uid=root;Pwd='';");
        public MySqlCommand cmd = new MySqlCommand();
        public MySqlDataAdapter adtr;
        public MySqlDataReader dr;
        public DataSet ds;

        
        #region MouseMove
        //Change window position based on mouse position
        protected void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                this.Left = e.X + this.Left - (ilkkonum.X);
                this.Top = e.Y + this.Top - (ilkkonum.Y);
            }
        }

        //Set dragging to true if the use is holding a mouse button on the programs top panel
        //and change the cursor style
        protected void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            this.Cursor = Cursors.SizeAll;
            ilkkonum = e.Location;
        }

        //If the mouse button is released during dragging, set dragging to false and change
        //mouse style
        protected void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            this.Cursor = Cursors.Default;
        }

        //Exit program when X is pressed (glitchy)
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
