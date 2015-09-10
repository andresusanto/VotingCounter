using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using OpenPop.Common.Logging;
using Message = OpenPop.Mime.Message;

namespace VotingCounter
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<int, Message> messages = new Dictionary<int, Message>();
		private readonly Pop3Client pop3Client;

        int[] suara = new int[5];
        Label[] lblCalon = new Label[5];

        int totalSuara = 0;

        List<string> kupon = new List<string>();

        String passEMAIL;

        public Form1(String pas_)
        {
            InitializeComponent();
            pop3Client = new Pop3Client();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;

            passEMAIL = pas_;

        }

        private void updateData()
        {
            suara[0] = 0;
            suara[1] = 0;
            suara[2] = 0;
            suara[3] = 0;
            suara[4] = 0;

            lblCalon[0] = lblCalon1;
            //lblCalon[1] = lblCalon2;
            lblCalon[2] = lblCalon3;
            //lblCalon[3] = lblCalon4;
            lblCalon[4] = lblCalon5;


            try
            {
                if (pop3Client.Connected)
                    pop3Client.Disconnect();
                pop3Client.Connect("pop.gmail.com", 995, true);
                pop3Client.Authenticate("ronde2pemiluhmif@gmail.com", passEMAIL);
                int count = pop3Client.GetMessageCount();

                progressBar1.Maximum = count;

                int success = 0;
                int fail = 0;
                for (int i = count; i >= 1; i -= 1)
                {
                    // Check if the form is closed while we are working. If so, abort
                    if (IsDisposed)
                        return;

                    // Refresh the form while fetching emails
                    // This will fix the "Application is not responding" problem
                    Application.DoEvents();

                    try
                    {
                        Message message = pop3Client.GetMessage(i);

                        String pesan;

                        if (message.MessagePart.IsText)
                        {
                            pesan = message.MessagePart.GetBodyAsText();
                        }
                        else
                        {
                            MessagePart plainTextPart = message.FindFirstPlainTextVersion();
                            if (plainTextPart != null)
                            {
                                pesan = plainTextPart.GetBodyAsText();
                            }
                            else
                            {
                                // Try to find a body to show in some of the other text versions
                                List<MessagePart> textVersions = message.FindAllTextVersions();
                                if (textVersions.Count >= 1)
                                    pesan = textVersions[0].GetBodyAsText();
                                else
                                    pesan = "<<OpenPop>> Cannot find a text version body in this message to show <<OpenPop>>";
                            }
                        }

                        if (message.Headers.Subject == "2SUARA KAHIM")
                        {
                            //MessageBox.Show("IBM");
                            String[] komponen = pesan.Split('|');
                            if (komponen.Count() == 4)
                            {
                                if (komponen[0] == "COMBODUO")
                                {
                                    if (!kupon.Contains(komponen[2]))
                                    {
                                        int pilihan = 0;
                                        if (Int32.TryParse(komponen[3], out pilihan))
                                        {
                                            kupon.Add(komponen[2]);
                                            textBox1.AppendText(komponen[2] + " " + komponen[3] + "\n");

                                            suara[pilihan - 1] += 1;
                                            lblCalon[pilihan - 1].Text =  suara[pilihan - 1].ToString();
                                            chart1.Series.Clear();
                                            chart1.Series.Add("");
                                            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                                            chart1.Series[0].Points.Add(0);
                                            chart1.Series[0].Points.Add(0);
                                            chart1.Series[0].Points.Add(0);
                                            //chart1.Series[0].Points.Add(0);
                                            //chart1.Series[0].Points.Add(0);

                                            chart1.Series[0].Points[0].IsValueShownAsLabel = true;
                                            chart1.Series[0].Points[0].Label = "Joshua Belzalel A";
                                            chart1.Series[0].Points[0].SetValueY(suara[0]);

                                            //chart1.Series[0].Points[1].IsValueShownAsLabel = true;
                                            //chart1.Series[0].Points[1].Label = "Farid Fadhil H";
                                            //chart1.Series[0].Points[1].SetValueY(suara[1]);

                                            chart1.Series[0].Points[1].IsValueShownAsLabel = true;
                                            chart1.Series[0].Points[1].Label = "Aryya Dwisatya W";
                                            chart1.Series[0].Points[1].SetValueY( suara[2] );

                                            //chart1.Series[0].Points[3].IsValueShownAsLabel = true;
                                            //chart1.Series[0].Points[3].Label = "Vidia Anindhita";
                                            //chart1.Series[0].Points[3].SetValueY( suara[3] );

                                            chart1.Series[0].Points[2].IsValueShownAsLabel = true;
                                            chart1.Series[0].Points[2].Label = "Abstain";
                                            chart1.Series[0].Points[2].SetValueY( suara[4] );

                                            progressBar1.Value++;
                                        }
                                    }
                                }
                            }
                        }

                        //MessageBox.Show(message.Headers.Subject + "\n\n" + pesan);

                        success++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        DefaultLogger.Log.LogError(
                            "TestForm: Message fetching failed: " + ex.Message + "\r\n" +
                            "Stack trace:\r\n" +
                            ex.StackTrace);
                        fail++;
                    }

                    //progressBar.Value = (int)(((double)(count - i) / count) * 100);
                }

                //MessageBox.Show(this, "Mail received!\nSuccesses: " + success + "\nFailed: " + fail, "Message fetching done");

                if (fail > 0)
                {
                    //MessageBox.Show(this, "");
                }
            }
            catch (InvalidLoginException)
            {
                MessageBox.Show(this, "The server did not accept the user credentials!", "POP3 Server Authentication");
            }
            catch (PopServerNotFoundException)
            {
                MessageBox.Show(this, "The server could not be found", "POP3 Retrieval");
            }
            catch (PopServerLockedException)
            {
                MessageBox.Show(this, "The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", "POP3 Account Locked");
            }
            catch (LoginDelayException)
            {
                MessageBox.Show(this, "Login not allowed. Server enforces delay between logins. Have you connected recently?", "POP3 Account Login Delay");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error occurred retrieving mail. " + ex.Message, "POP3 Retrieval");
            }

            progressBar1.Value = progressBar1.Maximum;
            MessageBox.Show("Selesai, selamat untuk yang menang :)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread th = new Thread(updateData);
            th.Start();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
