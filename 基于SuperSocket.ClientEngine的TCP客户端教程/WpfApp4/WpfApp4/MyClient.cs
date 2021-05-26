using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WpfApp4
{
    class A
    {
        EasyClient EasyClient;

        public event EventHandler Connected
        {
            add
            {
                EasyClient.Connected += value;
            }
            remove
            {
                EasyClient.Connected -= value;
            }
        }
    }


    class MyClient
    {
        static void Main()
        {
            // 实例化
            EasyClient easyClient = new EasyClient();
            // 连接成功触发事件
            easyClient.Connected += EasyClient_Connected;
            // 关闭成功触发事件
            easyClient.Closed += EasyClient_Closed;
            // 异常触发事件（连接失败，通讯异常）
            easyClient.Error += EasyClient_Error;
            // 设置过滤器
            // 设置消息处理程序
            easyClient.Initialize<MessagePackageInfo>(new MyReceiveFilter(), new Action<MessagePackageInfo>((l) => { Console.WriteLine(l); }));

            var task = easyClient.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2020));

            Console.ReadLine();
            easyClient.Close();
        }

        private static void EasyClient_Connected(object sender, EventArgs e)
        {
            EasyClient easyClient = sender as EasyClient;
        }

        private static void EasyClient_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Client has Closed.");
        }

        private static void EasyClient_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        }
    }

    // 假设消息格式:以||表示一条完整消息的结束
    // 如 ADD 1 2||
    class MyReceiveFilter : TerminatorReceiveFilter<MessagePackageInfo>
    {
        public MyReceiveFilter() : base(Encoding.ASCII.GetBytes("\r\n")) { }

        public override MessagePackageInfo ResolvePackage(IBufferStream bufferStream)
        {
           string msg = bufferStream.ReadString((int)bufferStream.Length, Encoding.ASCII);
           return new MessagePackageInfo();
        }
    }


    public class ResponseBuff
    {
        private object locker = new object();
        private Dictionary<Guid, AutoResetEvent> autoResetEvents = new Dictionary<Guid, AutoResetEvent>();
        
        public void Add(Guid guid, AutoResetEvent autoResetEvent)
        {
            lock (locker)
            {
                autoResetEvents[guid] = autoResetEvent;
            }
        }

        public AutoResetEvent Get(Guid guid)
        {
            lock (locker)
            {
                return autoResetEvents[guid];
            }
        }
    }




    public enum Command
    {
        ADD,
        SUB,
        MUL,
        DIV
    }

    public class Calculator
    {
        private EasyClient _easyClient;

        public void Initialize()
        {
            _easyClient = new EasyClient();
            _easyClient.Initialize(new MyReceiveFilter(), new Action<MessagePackageInfo>((msg) =>
             {

             }));
        }




        public double Calculate(MessagePackageInfo messagePackageInfo)
        {
            _easyClient.Send(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(messagePackageInfo) + "\r\n"));
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            bool waited = autoResetEvent.WaitOne(TimeSpan.FromSeconds(5));
            if (!waited)
            {
                throw new TimeoutException();
            }

        }



        public void Release()
        {

        }

    }

    // 请求消息
    // Add 1.1 2.2 Double.NaN

    // 响应消息
    // Add 1.1 2.2 3.3


    public class MessagePackageInfo : IPackageInfo
    {
        public MessagePackageInfo()
        {

        }

        public MessagePackageInfo(string command,double parameter1,double parameter2,double result = Double.NaN)
        {
            this.Command = command;
            this.Parameter1 = parameter1;
            this.Parameter2 = parameter2;
            this.Result = result;
        }


        public string Command { get; set; }
        public double Parameter1 { get; set; }
        public double Parameter2 { get; set; }
        public double Result { get; set; }
    }
}
