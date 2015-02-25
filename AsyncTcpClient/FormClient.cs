using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Security.Cryptography;

namespace AsyncTcpClient
{
    public partial class FormClient : Form
    {
        //是否正常退出
        private bool isExit = false;
        private TcpClient client;
        private BinaryReader br;
        private BinaryWriter bw;
        private string publickey;
        private String[,] userInfo=new String[100,2];
        BackgroundWorker connectWork = new BackgroundWorker();
        private string serverIP = "127.0.0.1";
        public FormClient()
        {
            if (ContainsKey.Create())
            {
                InitializeComponent();

                publickey = ReadKey.publicKey;
                this.StartPosition = FormStartPosition.CenterScreen;
                Random r = new Random((int)DateTime.Now.Ticks);
                txt_UserName.Text = "user" + r.Next(100, 999);
                lst_OnlineUser.HorizontalScrollbar = true;
                connectWork.DoWork += new DoWorkEventHandler(connectWork_DoWork);
                connectWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(connectWork_RunWorkerCompleted);
            }
        }

        /// <summary>
        /// 异步方式与服务器进行连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void connectWork_DoWork(object sender, DoWorkEventArgs e)
        {
            client = new TcpClient();
            IAsyncResult result = client.BeginConnect(serverIP, 8889, null, null);
            while (!result.IsCompleted)
            {
                Thread.Sleep(100);
                AddStatus(".");
            }
            try
            {
                client.EndConnect(result);
                e.Result = "success";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
                return;
            }
        }

        /// <summary>
        /// 异步方式与服务器完成连接操作后的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void connectWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString() == "success")
            {
                AddStatus("连接成功");
                //获取网络流
                NetworkStream networkStream = client.GetStream();
                //将网络流作为二进制读写对象
                br = new BinaryReader(networkStream);
                bw = new BinaryWriter(networkStream);
                AsyncSendMessage("Login," + publickey + txt_UserName.Text);
                Thread threadReceive = new Thread(new ThreadStart(ReceiveData));
                threadReceive.IsBackground = true;
                threadReceive.Start();
            }
            else
            {
                AddStatus("连接失败:" + e.Result);
                btn_Login.Enabled = true;
            }
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            btn_Login.Enabled = false;
            AddStatus("开始连接.");
            connectWork.RunWorkerAsync();
        }

        /// <summary>
        /// 处理接收的服务器收据
        /// </summary>
        private void ReceiveData()
        {
            string receiveString = null;
            while (!isExit)
            {
                ReceiveMessageDelegate d = new ReceiveMessageDelegate(receiveMessage);
                IAsyncResult result = d.BeginInvoke(out receiveString, null, null);
                //使用轮询方式来盘点异步操作是否完成
                while (!result.IsCompleted)
                {
                    if (isExit)
                        break;
                    Thread.Sleep(250);
                }
                //获取Begin方法的返回值所有输入/输出参数
                d.EndInvoke(out receiveString, result);
                if(receiveString == null)
                {
                    if(!isExit)
                        MessageBox.Show("与服务器失去联系");
                    break;
                }
                string[] splitString = receiveString.Split(',');
                string command = splitString[0];
                switch (command)
                {
                    case "Login":   //格式： login,用户名
                        for(int i=0;i<userInfo.Length;i++)
                        {
                            if(userInfo[i,0]!=null){
                                continue;
                            }
                            else{
                                userInfo[i,0]=splitString[1].Substring(245);
                                userInfo[i, 1] = splitString[1];
                                AddOnline(userInfo[i,0]);
                                //MessageBox.Show("dan yonghuming: "+userInfo[i, 0] + "\n wanzhengmingzi: " + userInfo[i, 1]);
                                break;
                                //rtf_SendMessage.Text = userInfo[i, 0] + "\n" + userInfo[i, 1];
                            }
                        }
                        //AddOnline(splitString[1]);
                        break;
                    case "Logout":  //格式： logout,用户名
                        RemoveUserName(splitString[1].Substring(245));
                        break;
                    case "Talk":    //格式： talk,用户名,对话信息
                        AddTalkMessage(splitString[1].Substring(245) + " to " + txt_UserName.Text + ":\r\n");
                        string finalmessage = SRSA.Decrypt(splitString[2]);
                        AddTalkMessage(finalmessage+"\n");

                        //AddTalkMessage(receiveString.Substring(splitString[0].Length + splitString[1].Length + 2));
                        break;
                }
            }
            Application.Exit();
        }

        /// <summary>
        /// 发送信息状态的数据结构
        /// </summary>
        private struct SendMessageStates
        {
            public SendMessageDelegate d;
            public IAsyncResult result;
        }

        #region RSAEnCrypto
        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="DataToEnCrypto"></param>
        /// <param name="RSAKeyInfo"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        static public byte[] RSAEnCrypto(byte[] DataToEnCrypto, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.ImportParameters(RSAKeyInfo);
                return RSA.Encrypt(DataToEnCrypto, DoOAEPPadding);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 异步向服务器发送数据
        /// </summary>
        /// <param name="message"></param>
        private void AsyncSendMessage(string message)
        {
            SendMessageDelegate d = new SendMessageDelegate(SendMessage);
            IAsyncResult result = d.BeginInvoke(message, null, null);
            while (!result.IsCompleted)
            {
                if (isExit)
                    return;
                Thread.Sleep(50);
            }
            SendMessageStates states = new SendMessageStates();
            states.d = d;
            states.result = result;
            Thread t = new Thread(FinishAsyncSendMessage);
            t.IsBackground = true;
            t.Start(states);
        }

        /// <summary>
        /// 处理接收的服务端数据
        /// </summary>
        /// <param name="obj"></param>
        private void FinishAsyncSendMessage(object obj)
        {
            SendMessageStates states = (SendMessageStates)obj;
            states.d.EndInvoke(states.result);
        }

        delegate void SendMessageDelegate(string message);
        /// <summary>
        /// 向服务端发送数据
        /// </summary>
        /// <param name="message"></param>
        private void SendMessage(string message)
        {
            try
            {
                bw.Write(message);
                bw.Flush();
            }
            catch
            {
                AddStatus("发送失败");
            }
        }

        private void btn_SendeMessage_Click(object sender, EventArgs e)
        {
            if (lst_OnlineUser.SelectedIndex != -1)
            {
                string wholename = null;
                string publickeyfromuser = null;
                for (int i = 0; i < userInfo.Length; i++)
                {
                    //MessageBox.Show(""+(userInfo[i,0]));
                    if (lst_OnlineUser.SelectedItem.Equals(userInfo[i, 0]))
                    {
                        wholename = userInfo[i, 1];
                        publickeyfromuser = userInfo[i, 1].Substring(0, 245);
                        break;
                    }
                }
                //string publickeyfromuser = lst_OnlineUser.SelectedItem + "";
                //publickeyfromuser = publickeyfromuser.Substring(0, 245);
                string encrypteddata;
                try
                {
                    UnicodeEncoding ByteConverter = new UnicodeEncoding();


                    byte[] encrypteData;
                    byte[] plainttext;
                    plainttext = ByteConverter.GetBytes(rtf_SendMessage.Text);

                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    RSA.FromXmlString(publickeyfromuser);
                    encrypteData = RSAEnCrypto(plainttext, RSA.ExportParameters(false), false);

                    encrypteddata = Convert.ToBase64String(encrypteData);
                }
                catch (Exception f)
                {
                    Console.WriteLine(f);
                    encrypteddata = null;
                }
                /*
                abc = rtf_SendMessage.Text;
                string aaa = SRSA.Encrypt(abc);
                */
                string bbb = SRSA.Decrypt(encrypteddata);

                AsyncSendMessage("Talk," + wholename + "," + encrypteddata + "\r\n");
                AddTalkMessage(txt_UserName.Text+" to "+lst_OnlineUser.SelectedItem+":\r\n");
                AddTalkMessage(rtf_SendMessage.Text + "\n");
                if(rtf_SendMessage.Text.Equals("Bye"))
                {
                    Application.Exit();
                }
                rtf_SendMessage.Clear();
                //AddTalkMessage(splitString[1].Substring(245) + " to " + txt_UserName.Text + ":\r\n");
                //string finalmessage = SRSA.Decrypt(splitString[2]);
                //AddTalkMessage(finalmessage + "\n");
            }
            else
                MessageBox.Show("请先在[当前在线]中选择一个对话者");
        }

        delegate void ConnectServerDelegate();
        /// <summary>
        /// 连接服务器
        /// </summary>
        private void ConnectServer()
        {
            client = new TcpClient(serverIP, 8889);
        }

        delegate void ReceiveMessageDelegate(out string receiveMessage);
        /// <summary>
        /// 读取服务器发过来的信息
        /// </summary>
        /// <param name="receiveMessage"></param>
        private void receiveMessage(out string receiveMessage)
        {
            receiveMessage = null;
            try
            {
                receiveMessage = br.ReadString();
            }
            catch (Exception ex)
            {
                AddStatus(ex.Message);
            }
        }

        private delegate void AddTalkMessageDelegate(string message);
        /// <summary>
        /// 向 rtf 中添加聊天记录
        /// </summary>
        /// <param name="message"></param>
        private void AddTalkMessage(string message)
        {
            if (rtf_MessageInfo.InvokeRequired)
            {
                AddTalkMessageDelegate d = new AddTalkMessageDelegate(AddTalkMessage);
                rtf_MessageInfo.Invoke(d, new object[] { message });
            }
            else
            {
                rtf_MessageInfo.AppendText(message);

                if (!File.Exists("Log.txt"))
                {
                    FileStream fs1 = new FileStream("Log.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(message);//开始写入值
                    sw.Close();
                    fs1.Close();
                }
                else
                {
                    //FileStream fs = new FileStream("Log.txt", FileMode.Open, FileAccess.Write);
                    FileStream fs = File.Open("Log.txt", FileMode.Append, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine(message);//开始写入值
                    sr.Close();
                    fs.Close();

                }

                rtf_MessageInfo.ScrollToCaret();
            }
        }

        private delegate void AddStatusDelegate(string message);
        /// <summary>
        /// 向 rtf 中添加状态信息
        /// </summary>
        /// <param name="message"></param>
        private void AddStatus(string message)
        {
            if (rtf_StatusInfo.InvokeRequired)
            {
                AddStatusDelegate d = new AddStatusDelegate(AddStatus);
                rtf_StatusInfo.Invoke(d, new object[] { message });
            }
            else
            {
                rtf_StatusInfo.AppendText(message);
            }
        }

        private delegate void AddOnlineDelegate(string message);
        /// <summary>
        /// 向 lst_Online 添加在线用户
        /// </summary>
        /// <param name="message"></param>
        private void AddOnline(string message)
        {
            if (lst_OnlineUser.InvokeRequired)
            {
                AddOnlineDelegate d = new AddOnlineDelegate(AddOnline);
                lst_OnlineUser.Invoke(d, new object[] { message });
            }
            else
            {
                lst_OnlineUser.Items.Add(message);
                lst_OnlineUser.SelectedIndex = lst_OnlineUser.Items.Count - 1;
                lst_OnlineUser.ClearSelected();
            }
        }

        private delegate void RemoveUserNameDelegate(string userName);
        /// <summary>
        /// 从 listBoxOnline 删除离线用户
        /// </summary>
        /// <param name="userName"></param>
        private void RemoveUserName(string userName)
        {
            if (lst_OnlineUser.InvokeRequired)
            {
                RemoveUserNameDelegate d = RemoveUserName;
                lst_OnlineUser.Invoke(d, userName);
            }
            else
            {
                lst_OnlineUser.Items.Remove(userName);
                lst_OnlineUser.SelectedIndex = lst_OnlineUser.Items.Count - 1;
                lst_OnlineUser.ClearSelected();
            }
        }

        private void FormClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                AsyncSendMessage("Logout," + publickey + txt_UserName.Text);
                isExit = true;
                br.Close();
                bw.Close();
                client.Close();
            }
        }

    }
}
