using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GorselProgramlamaFinal2
{
    public partial class CircularProgressBarTestForm : Form
    {
        public CircularProgressBarTestForm()
        {
            InitializeComponent();
        }

        private void CircularProgressBarTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
        }

        private void button_Click(object sender, EventArgs e)
        {
            myCircularProgressBar1.Value = 1;
            for (int i = 0; i < 100; i++)
            {
                    myCircularProgressBar1.Increment(1);
                    myCircularProgressBar1.Wait(100);
                    if (myCircularProgressBar1.Value == 100)
                    {
                        break;
                    }
            }
        }
    }
}
