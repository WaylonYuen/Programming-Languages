using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Testing.Net {

    public class SocketTest {

        public Socket Socket;
        public IPEndPoint IPEndPoint;

        private static readonly string remoteHost = "127.0.0.1";
        private static readonly int remotePort = 8808;

        private static ManualResetEvent TimeoutObj = new ManualResetEvent(false);

        public void Start() {
            IPEndPoint = new IPEndPoint(IPAddress.Parse(remoteHost), remotePort);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IOControlTest();
            AsyncConnect();
        }

        //IO控制
        //範例：確認連線狀態超時設定
        private void IOControlTest() {

            //超時設定
            Socket.SendTimeout = 1000;

            // 首次探測時間5 秒, 間隔偵測時間2 秒
            byte[] inValue = new byte[] { 1, 0, 0, 0, 0x88, 0x13, 0, 0, 0xd0, 0x07, 0, 0 };

            //設定心跳引數
            Socket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
        }

        //異步連線
        private bool AsyncConnect() {

            //訊號燈復位(關閉訊號燈)
            TimeoutObj.Reset();

            try {
                //異步連線調用
                Socket.BeginConnect(IPEndPoint, ConnectCallback, Socket);
            } catch (Exception err) {
                Console.WriteLine(err.ToString());
                return false;
            }

            Console.WriteLine("Connect");
            //等待訊號燈亮起才放行（超時上限）
            return TimeoutObj.WaitOne(3000, false);
        }

        //回調
        private static void ConnectCallback(IAsyncResult ar) {

            //開啟訊號燈
            TimeoutObj.Set();
            Console.WriteLine("Callback");

            Socket s = (Socket)ar.AsyncState;

            //異步處理(此處為連線)

            //異步連接結束
            s.EndConnect(ar);
        }

    }

}
