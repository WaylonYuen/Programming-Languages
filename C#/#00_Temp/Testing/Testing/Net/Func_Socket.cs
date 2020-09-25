using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Testing.Net {

    public class Socket_wrapper {

        //委託
        private delegate void delSocketDataArrival(byte[] data);
        //static delSocketDataArrival socketDataArrival = socketDataArrivalHandler;

        private delegate void delSocketDisconnected();
        //static delSocketDisconnected socketDisconnected = socketDisconnectedHandler;

        public static Socket theSocket = null;
        private static string remoteHost = "192.168.1.71";
        private static int remotePort = 6666;

        private static String SockErrorStr = null;

        private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        private static Boolean IsconnectSuccess = false; //非同步連線情況，由非同步連接回調函式置位
        private static object lockObj_IsConnectSuccess = new object();

        /// 建構函式
        public Socket_wrapper(string strIp, int iPort) {
            remoteHost = strIp;
            remotePort = iPort;
        }

        /// 設定心跳
        private static void SetXinTiao() {

            //byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 };// 首次探測時間20 秒, 間隔偵測時間2 秒
            byte[] inValue = new byte[] { 1, 0, 0, 0, 0x88, 0x13, 0, 0, 0xd0, 0x07, 0, 0 };// 首次探測時間5 秒, 間隔偵測時間2 秒

            theSocket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
        }

        /// 建立套接字+非同步連線函式
        private static bool socket_create_connect() {

            //連線設定
            IPAddress ipAddress = IPAddress.Parse(remoteHost);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, remotePort);
            theSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            theSocket.SendTimeout = 1000;   //超時設定

            SetXinTiao();//設定心跳引數

            //Reset阻塞
            TimeoutObject.Reset(); //復位timeout事件

            //連線
            try {
                //theSocket.BeginConnect(remoteEP, connectedCallback, theSocket);
            } catch (Exception err) {
                SockErrorStr = err.ToString();
                return false;
            }

            //直到timeout，或者TimeoutObject.set()
            if (TimeoutObject.WaitOne(10000, false)) {
                return IsconnectSuccess;
            } else {
                SockErrorStr = "Time Out";
                return false;
            }
        }

    }
}