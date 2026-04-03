using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyAuth;


namespace Aryan_panel
{
    public partial class Form2 : Form
    {
        public static api KeyAuthApp = new api(
    name: "Minographyshorts's Application", // App name
    ownerid: "HBwrlbFTXv", // Account ID
    version: "1.0" // Application version. Used for automatic downloads see video here https://www.youtube.com/watch?v=kW195PLCBKs
                   //path: @"Your_Path_Here" // (OPTIONAL) see tutorial here https://www.youtube.com/watch?v=I9rxt821gMk&t=1s
);
        public Form2()
        {
            InitializeComponent();
            //KeyAuthApp.init();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();


        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            //Globals.KeyAuthApp.init();
            //MessageBox.Show("Login Confirmation telegram - @txpff ");
            //Globals.KeyAuthApp.login(User.Text, Pass.Text);

            //if (Globals.KeyAuthApp.response.success)
            //{
            //    MessageBox.Show("✓ Login Success!join our telegram for forever free free fire panel @txpff ");
            //    this.Hide();
            //    new Form1().Show();
            //    MessageBox.Show("✓ @txpff telegram channel");
            //}
            //else
            //{
            //    MessageBox.Show("✘ Login Failed: " + Globals.KeyAuthApp.response.message);
            //}

            await KeyAuthApp.login(username.Text, pass.Text);
            if (KeyAuthApp.response.success)
            {
                Form1 main = new Form1();
                main.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Status: " + KeyAuthApp.response.message);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1_LinkClicked(sender, e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/txpFF");
            linkLabel1.LinkVisited = true;
        }

        private void Pass_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

