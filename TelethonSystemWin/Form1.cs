using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ETSLib.Classes;

namespace TelethonSystemWin
{
    public partial class login : Form
    {
        int counter = 0;
        public login()
        {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if((usernameBox.Text == "" && passwordBox.Text == "") || (usernameBox.Text == "" || passwordBox.Text == "")){
                MessageBox.Show("You need to enter a username and password!");
                counter++;
            }
            else if(usernameBox.Text != "ETS" || passwordBox.Text != "admin")
            {
                MessageBox.Show("Wrong username and/or password!");
                counter++;
            }
            else if(usernameBox.Text == "ETS" && passwordBox.Text == "admin")
            {
                ETSTelethon ETSTelethon = new ETSTelethon();
                ETSTelethon.Visible = true;
                ETSTelethon.Activate();
            }
            if(counter == 3)
            {
                MessageBox.Show("Sorry user, you had your chance and you blew it, see ya next time!");
                Application.Exit();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
