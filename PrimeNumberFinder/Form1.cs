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
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100; // Update UI every 100 milliseconds
            timer.Tick += Timer_Tick;
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            await FindPrimesAsync(textBox1, listBox1);
        }

        private async void Button2_Click(object sender, EventArgs e)
        {
            await FindPrimesAsync(textBox2, listBox2);
        }

        private async Task FindPrimesAsync(TextBox textBox, ListBox listBox)
        {
            if (int.TryParse(textBox.Text, out int limit))
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;

                try
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = limit;

                    timer.Start(); // Starts the timer

                    var progress = new Progress<int>(percent =>
                    {
                        // Updates progress bar
                        progressBar1.Value = Math.Min(percent, progressBar1.Maximum);
                    });

                    List<int> primes = await Task.Run(() => FindPrimes(limit, progress, cancellationToken), cancellationToken);

                    // Updates UI with prime numbers.
                    listBox.Items.Clear();
                    foreach (int prime in primes)
                    {
                        listBox.Items.Add(prime);
                    }
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("Prime number search was canceled.");
                }
                finally
                {
                    timer.Stop(); // Stops the timer when the operation completes
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number in the textbox.");
            }
        }

        private List<int> FindPrimes(int limit, IProgress<int> progress, CancellationToken cancellationToken)
        {
            List<int> primes = new List<int>();
            for (int i = 2; i <= limit; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (IsPrime(i))
                {
                    primes.Add(i);
                }

                // Report progress.
                int percentComplete = (int)((double)i / limit * 100);
                progress.Report(percentComplete);
            }
            return primes;
        }

        


        private void Form1_Load(int limit, ListBox resultListBox, IProgress<int> progress, CancellationToken cancellationToken)
        {
            List<int> primes = new List<int>();
            for (int i = 2; i <= limit; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (IsPrime(i))
                {
                    primes.Add(i);
                }

                // Reports progress.
                int percentComplete = (int)((double)i / limit * 100);
                progress.Report(percentComplete);
            }

            // Updates UI with prime numbers.
            resultListBox.Invoke((MethodInvoker)delegate {
                resultListBox.Items.Clear();
                foreach (int prime in primes)
                {
                    resultListBox.Items.Add(prime);
                }
            });
        }

        private bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n <= 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;

            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            }
            return true;
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
