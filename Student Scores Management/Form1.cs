using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using MaterialSkin.Animations;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Runtime.InteropServices;

namespace Student_Scores_Management
{
    public partial class Form1 : MaterialForm
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "YOUR_KEY",
            BasePath = "YOUR_ADDRESS"
        };

        IFirebaseClient client;

        //checks for an internet connection
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            return InternetGetConnectedState(out int description, 0);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsInternetAvailable())
            {
                MessageBox.Show("Connection Established!");
            }
            else
            {
                MessageBox.Show("Cannot connect to the Database , please check your connection");
            }
            client = new FireSharp.FirebaseClient(config);

            //this sets the colors for material skin UI
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private async void materialFlatButton3_Click(object sender, EventArgs e)
        {
            //creates an object instance for data to be inserted to the database
            var data = new Data
            {
                Id = textBox1.Text,
                Name = textBox2.Text,
                Faculty = textBox3.Text,
                Examscore = textBox4.Text
            };
            //inserts the data into the database
            SetResponse response = await client.SetAsync(textBox1.Text + "/", data);
            MessageBox.Show("Data Inserted/Updated");
        }

        private async void materialFlatButton5_Click(object sender, EventArgs e)
        {
            //gets the data from the database
            FirebaseResponse response = await client.GetAsync(textBox5.Text);
            Data obj = response.ResultAs<Data>();

            textBox5.Text = obj.Id;
            textBox6.Text = obj.Name;
            textBox7.Text = obj.Faculty;
            textBox8.Text = obj.Examscore;

            MessageBox.Show("Data Retrieved");

        }
       
        private async void materialFlatButton1_Click(object sender, EventArgs e)
        {
            //deletes an entry from the DB
            FirebaseResponse response = await client.DeleteAsync(textBox9.Text);
            MessageBox.Show("Data Deleted");
        }
    }

    internal class Data
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Examscore { get; set; }
    }
}
