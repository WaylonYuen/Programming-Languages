using System;
namespace Server_v1 {

    /// <summary>
    /// 項目入口
    /// </summary>
    class Start {

        public static Server server;

        private static void Main() {

            //提示語
            Console.WriteLine("正在啟動服務器...\n");
            Server.isQuit = false;  //關閉服務器退出信號燈

            server = new Server();  //初始化Server (構建所有必要結構)
            server.Start();         //啟動Server  （持續監聽）

            Console.ReadKey();
            Server.isQuit = true;   //開啟服務器退出信號燈
            Console.ReadKey();


            Console.WriteLine("#  Server is Close");

        }
    }

}
