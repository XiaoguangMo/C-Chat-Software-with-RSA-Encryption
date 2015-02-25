using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace AsyncTcpServer
{
    public partial class FormServer : Form
    {
        /// <summary>
        /// 保存连接的所有用户
        /// </summary>
        private List<User> userList = new List<User>();
        /// <summary>
        /// 使用的本机IP地址
        /// </summary>
        IPAddress localAddress = IPAddress.Parse("127.0.0.1");
        /// <summary>
        /// 监听端口
        /// </summary>
        private const int port = 8889;
        private TcpListener myListener;
        /// <summary>
        /// 是否正常退出所有接收线程
        /// </summary>
        bool isExit = false;
        public FormServer()
        {
            InitializeComponent();
            lst_Status.HorizontalScrollbar = true;
            btn_Stop.Enabled = false;
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            myListener = new TcpListener(localAddress, port);
            myListener.Start();
            AddItemToListBox(string.Format("开始在{0}:{1}监听客户端", localAddress, port));
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
            btn_Start.Enabled = false;
            btn_Stop.Enabled = true;
        }

        /// <summary>
        /// 监听客户端请求
        /// </summary>
        private void ListenClientConnect()
        {
            TcpClient newClient = null;
            while (true)
            {
                ListenClientDelegate d = new ListenClientDelegate(ListenClient);
                IAsyncResult result = d.BeginInvoke(out newClient, null, null);
                //使用轮询方式来判断异步操作是否完成
                while (result.IsCompleted == false)
                {
                    if (isExit)
                        break;
                    Thread.Sleep(250);
                }
                //获取Begin 方法的返回值和所有输入/输出参数
                d.EndInvoke(out newClient, result);
                if (newClient != null)
                {
                    //每接受一个客户端连接，就创建一个对应的线程循环接收该客户端发来的信息
                    User user = new User(newClient);
                    Thread threadReceive = new Thread(ReceiveData);
                    threadReceive.Start(user);
                    userList.Add(user);
                    AddItemToListBox(string.Format("[{0}]进入", newClient.Client.RemoteEndPoint));
                    AddItemToListBox(string.Format("当前连接用户数：{0}", userList.Count));
                }
                else
                {
                    break;
                }
            }
        }

        private void ReceiveData(object userState)
        {
            User user = (User)userState;
            TcpClient client = user.client;
            while (!isExit)
            {
                string receiveString = null;
                ReceiveMessageDelegate d = new ReceiveMessageDelegate(ReceiveMessage);
                IAsyncResult result = d.BeginInvoke(user, out receiveString, null, null);
                //使用轮询方式来判断异步操作是否完成
                while (!result.IsCompleted)
                {
                    if (isExit)
                        break;
                    Thread.Sleep(250);
                }
                //获取Begin方法的返回值和所有输入/输出参数
                d.EndInvoke(out receiveString, result);
                if (receiveString == null)
                {
                    if (!isExit)
                    {
                        AddItemToListBox(string.Format("与{0}失去联系，已终止接收该用户信息", client.Client.RemoteEndPoint));
                        RemoveUser(user);
                    }
                    break;
                }
                //AddItemToListBox(string.Format("来自[{0}]:{1}", user.client.Client.RemoteEndPoint, receiveString.Remove(5, 245)));
                string[] splitString = receiveString.Split(',');
                switch (splitString[0])
                {
                    case "Login":
                        AddItemToListBox(string.Format("来自[{0}]:{1}", user.client.Client.RemoteEndPoint, receiveString.Remove(6, 245)));
                        user.userName = splitString[1];
                        AsyncSendToAllClient(user, receiveString);
                        break;
                    case "Logout":
                        AddItemToListBox(string.Format("来自[{0}]:{1}", user.client.Client.RemoteEndPoint, receiveString.Remove(7, 245)));
                        AsyncSendToAllClient(user, receiveString);
                        RemoveUser(user);
                        return;
                    case "Talk":
                        AddItemToListBox(string.Format("来自[{0}]:{1}", user.client.Client.RemoteEndPoint, receiveString.Remove(5, 245)));
                        string talkString = receiveString.Substring(splitString[0].Length + splitString[1].Length + 2);
                        
                        AddItemToListBox(string.Format("{0}对{1}说：{2}", user.userName.Substring(245), splitString[1].Substring(245), talkString));
                        foreach (User target in userList)
                        {
                            if (target.userName == splitString[1])
                            {
                                AsyncSendToClient(target, "Talk," + user.userName + "," + talkString);
                                break;
                            }
                        }
                        break;
                    default:
                        AddItemToListBox("什么意思啊：" + receiveString);
                        AddItemToListBox(splitString[0]);

                        break;
                }
            }
        }

        /// <summary>
        /// 异步发送信息给所有客户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        private void AsyncSendToAllClient(User user, string message)
        {
            string command = message.Split(',')[0];
            if (command == "Login")
            {
                for (int i = 0; i < userList.Count; i++)
                {
                    AsyncSendToClient(userList[i], message);
                    if (userList[i].userName != user.userName)
                        AsyncSendToClient(user, "Login," + userList[i].userName);
                }
            }
            else if (command == "Logout")
            {
                for (int i = 0; i < userList.Count; i++)
                {
                    if (userList[i].userName != user.userName)
                        AsyncSendToClient(userList[i], message);
                }
            }
        }

        /// <summary>
        /// 异步发送message给user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        private void AsyncSendToClient(User user, string message)
        {
            SendToClientDelegate d = new SendToClientDelegate(SendToClient);
            IAsyncResult result = d.BeginInvoke(user, message, null, null);
            while (result.IsCompleted == false)
            {
                if (isExit)
                    break;
                Thread.Sleep(250);
            }
            d.EndInvoke(result);
        }

        private delegate void SendToClientDelegate(User user, string message);
        /// <summary>
        /// 发送message给user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        private void SendToClient(User user, string message)
        {
            try
            {
                //将字符串写入网络流，此方法会自动附加字符串长度前缀
                user.bw.Write(message);
                user.bw.Flush();
                //AddItemToListBox(string.Format("向[{0}]发送：{1}", user.userName.Substring(245), message.Remove(6,245)));
                string[] splitString = message.Split(',');
                switch (splitString[0])
                {
                    case "Login":
                        AddItemToListBox(string.Format("向[{0}]发送：{1}", user.userName.Substring(245), message.Remove(6, 245)));
                        break;
                    case "Logout":
                        AddItemToListBox(string.Format("向[{0}]发送：{1}", user.userName.Substring(245), message.Remove(7, 245)));
                        return;
                    case "Talk":
                        AddItemToListBox(string.Format("向[{0}]发送：{1}", user.userName.Substring(244), message.Remove(5, 245)));
                        break;
                    default:
                        AddItemToListBox("什么意思啊：" + message);
                        break;
                }
            }
            catch
            {
                AddItemToListBox(string.Format("向[{0}]发送信息失败", user.userName.Substring(245)));
            }
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="user"></param>
        private void RemoveUser(User user)
        {
            userList.Remove(user);
            user.Close();
            AddItemToListBox(string.Format("当前连接用户数：{0}", userList.Count));
        }

        delegate void ReceiveMessageDelegate(User user, out string receiveMessage);
        /// <summary>
        /// 接收客户端发来的信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="receiveMessage"></param>
        private void ReceiveMessage(User user, out string receiveMessage)
        {
            try
            {
                receiveMessage = user.br.ReadString();
            }
            catch (Exception ex)
            {
                AddItemToListBox(ex.Message);
                receiveMessage = null;
            }
        }

        private delegate void ListenClientDelegate(out TcpClient client);
        /// <summary>
        /// 接受挂起的客户端连接请求
        /// </summary>
        /// <param name="newClient"></param>
        private void ListenClient(out TcpClient newClient)
        {
            try
            {
                newClient = myListener.AcceptTcpClient();
            }
            catch
            {
                newClient = null;
            }
        }

        delegate void AddItemToListBoxDelegate(string str);
        /// <summary>
        /// 在ListBox中追加状态信息
        /// </summary>
        /// <param name="str">要追加的信息</param>
        private void AddItemToListBox(string str)
        {
            if (lst_Status.InvokeRequired)
            {
                AddItemToListBoxDelegate d = AddItemToListBox;
                lst_Status.Invoke(d, str);
            }
            else
            {
                lst_Status.Items.Add(str);
                if (!File.Exists("Log.txt"))
                {
                    FileStream fs1 = new FileStream("Log.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(str);//开始写入值
                    sw.Close();
                    fs1.Close();
                }
                else
                {
                    //FileStream fs = new FileStream("Log.txt", FileMode.Open, FileAccess.Write);
                    FileStream fs = File.Open("Log.txt", FileMode.Append, FileAccess.Write);
                    StreamWriter sr = new StreamWriter(fs);
                    sr.WriteLine(str);//开始写入值
                    sr.Close();
                    fs.Close();

                }
                lst_Status.SelectedIndex = lst_Status.Items.Count - 1;
                lst_Status.ClearSelected();
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            AddItemToListBox("开始停止服务，并依次使用户退出！");
            isExit = true;
            for (int i = userList.Count - 1; i >= 0; i--)
            {
                RemoveUser(userList[i]);
            }
            //通过停止监听让myListener.AcceptTcpClient()产生异常退出监听线程
            myListener.Stop();
            btn_Start.Enabled = true;
            btn_Stop.Enabled = false;
        }

        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myListener != null)
            {
                btn_Stop.PerformClick();
            }
        }

        private void lst_Status_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
