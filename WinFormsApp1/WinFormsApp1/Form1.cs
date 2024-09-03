using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private string filepath = "";
        private string myIP = "";
        private HttpListener listener;
        private ProgressBar progressBar;
        private string appFolder = Path.Combine(Application.StartupPath, "SharedFiles");
        string copiedFilePath;

        public Form1()
        {
            InitializeComponent();
            InitializeProgressBar();  // Initialize the progress bar
            InitializeStopButton();   // Initialize the stop button
        }

        private void InitializeProgressBar()
        {
            progressBar = new ProgressBar
            {
                Location = new System.Drawing.Point(51, 251),
                Size = new System.Drawing.Size(676, 23),
                Minimum = 0,
                Maximum = 100,
                Step = 1,
                Visible = false // Initially hidden
            };
            this.Controls.Add(progressBar);
        }

        private void InitializeStopButton()
        {
            Button stopButton = new Button
            {
                Text = "Stop Server",
                Location = new System.Drawing.Point(51, 290),
                Size = new System.Drawing.Size(100, 30),
                Enabled = false // Initially disabled until the server starts
            };

            stopButton.Click += StopButton_Click;
            this.Controls.Add(stopButton);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filepath = ofd.FileName;
                filepath = filepath.Replace("\\", "/");
                label1.Text = filepath;
                // Ensure the SharedFiles folder exists
                Directory.CreateDirectory(appFolder);

                // Start copying the file to the app folder in parallel
                copiedFilePath = Path.GetFileName(filepath);
                sendButton.Enabled = true;
            }
        }

        private async void sendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }


            await Task.Run(() => File.Copy(filepath, copiedFilePath, true));

            StartHttpServer(copiedFilePath);
        }

        private string GetLocalIpAddress()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 80);
                    IPEndPoint endPoint = (IPEndPoint)socket.LocalEndPoint;
                    return endPoint?.Address.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting local IP: {ex.Message}");
                return null;
            }
        }

        private void StartHttpServer(string sharedFile)
        {
            if (listener != null && listener.IsListening)
            {
                MessageBox.Show("Server is already running.");
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    listener = new HttpListener();
                    myIP = GetLocalIpAddress();

                    if (myIP == null)
                    {
                        MessageBox.Show("Unable to determine local IP address.");
                        return;
                    }

                    string url = $"http://{myIP}:5000/download/";
                    listener.Prefixes.Add(url);
                    listener.Start();

                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show($"Server started for {Path.GetFileName(sharedFile)}");
                        // Send the relative file name in the URL
                        textBox1.Text = $"{url}{Path.GetFileName(sharedFile)}";
                        sendButton.Enabled = false;
                        stopButton.Enabled = true;
                    }));

                    while (listener.IsListening)
                    {
                        HttpListenerContext context = listener.GetContext();
                        ProcessRequest(context, sharedFile);
                    }
                }
                catch (HttpListenerException hlex)
                {
                    MessageBox.Show($"HttpListener error (Administration run required): {hlex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unexpected error: {ex.Message}");
                }
            });
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StopHttpServer();
        }

        private void StopHttpServer()
        {
            if (listener != null && listener.IsListening)
            {
                listener.Stop();
                listener.Close();
                this.Invoke((Action)(() =>
                {
                    stopButton.Enabled = false; // Disable the stop button
                    sendButton.Enabled = true;  // Enable the send button
                    MessageBox.Show("Server stopped.");
                }));
            }
        }

        private void ProcessRequest(HttpListenerContext context, string sharedFile)
        {
            try
            {
                if (File.Exists(sharedFile))
                {
                    progressBar.Invoke((Action)(() => progressBar.Visible = true));  // Show the progress bar

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.AddHeader("Content-Disposition", $"attachment; filename={Path.GetFileName(sharedFile)}");

                    using (FileStream fs = new FileStream(sharedFile, FileMode.Open, FileAccess.Read))
                    {
                        long fileLength = fs.Length;
                        progressBar.Invoke((Action)(() =>
                        {
                            progressBar.Minimum = 0;
                            progressBar.Maximum = (int)fileLength;
                        }));

                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        long totalBytesSent = 0;

                        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            context.Response.OutputStream.Write(buffer, 0, bytesRead);
                            totalBytesSent += bytesRead;

                            // Update progress bar
                            progressBar.Invoke((Action)(() =>
                            {
                                progressBar.Value = (int)totalBytesSent;
                            }));
                        }
                    }

                    context.Response.StatusCode = (int)HttpStatusCode.OK;

                    // Delete the file after sending
                    Task.Run(() => File.Delete(sharedFile));
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write(ex.Message);
                }
            }
            finally
            {
                context.Response.Close();

                // Hide the progress bar when done
                progressBar.Invoke((Action)(() =>
                {
                    progressBar.Visible = false;
                    progressBar.Value = 0;
                }));

                // Stop server automatically after sending the file
                StopHttpServer();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
