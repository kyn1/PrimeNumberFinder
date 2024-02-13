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

namespace PrimeNumberFinder
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;

        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            InitializeTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // Update UI every 100 milliseconds
            };
            timer.Tick += Timer_Tick;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FindPrimes(textBox1, listBox1);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            FindPrimes(textBox2, listBox2);
        }

        private void FindPrimes(TextBox textBox, ListBox listBox)
        {
            if (int.TryParse(textBox.Text, out int limit))
            {
                progressBar1.Value = 0;
                progressBar1.Maximum = limit;

                timer.Start(); // Starts the timer



                var thread = new Thread(() => FindPrimesWorker(limit, listBox));
                thread.Start();
            }
            else
            {
                MessageBox.Show("Please enter a valid number in the textbox.");
            }
        }

        private void FindPrimesWorker(int limit, ListBox listBox)
        {
            bool[] primes = new bool[limit + 1];
            for (int i = 2; i <= limit; i++)
            {
                primes[i] = true;
            }

            for (int p = 2; p * p <= limit; p++)
            {
                if (primes[p] == true)
                {
                    for (int i = p * p; i <= limit; i += p)
                    {
                        primes[i] = false;
                    }
                }
                // Calculate progress and update the progress bar
                int percentComplete = (int)(((double)p * p / limit) * 100);
                progressBar1.Invoke((MethodInvoker)delegate {
                    progressBar1.Value = Math.Min(percentComplete, progressBar1.Maximum);
                });
            }



            // Updates UI with prime numbers.
            listBox.Invoke((MethodInvoker)delegate {
                listBox.Items.Clear();
                for (int i = 2; i <= limit; i++)
                {
                    if (primes[i])
                    {
                        listBox.Items.Add(i);
                    }
                }
            });

            timer.Stop(); // Stops the timer when the operation completes
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            // Updates any additional UI elements here if needed
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
