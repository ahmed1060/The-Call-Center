using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;


namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        public static string path_in_form;
        public Form1()
        {
            InitializeComponent();
        }

        private void path_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog v1 = new OpenFileDialog();
            v1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if(v1.ShowDialog() == DialogResult.OK)
            {
                path_box.Text = v1.FileName;
            }
            path_in_form = path_box.Text;
        }

        private void path_box_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
