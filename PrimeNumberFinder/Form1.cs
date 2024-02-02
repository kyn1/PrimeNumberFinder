using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeNumberFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Event handling logic
        }


        private void Form1_Load(int limit, ListBox resultListBox)
        {
            List<int> primes = new List<int>();
            for (int num = 2; num <= limit; num++)
            {
                bool isPrime = true;
                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if(num % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(num);
                }
            }

            resultListBox.Invoke(new Action(() =>
            {
                resultListBox.Items.Clear();
                foreach (int prime in primes)
                {
                    resultListBox.Items.Add(prime);
                }
            }));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(int.TryParse(textBox1.Text, out int limit))
            {
                Task.Run(() => Form1_Load(limit, listBox1));
            }
            else
            {
                MessageBox.Show("Please enter a valid number in the first textbox.");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int limit))
            {
                Task.Run(() => Form1_Load(limit, listBox2));
            }
            else
            {
                MessageBox.Show("Please enter a valid number in the second textbox.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
