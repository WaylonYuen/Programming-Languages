using System;
using System.Threading;

using Server_v1;
using Server_v1.Client;

namespace Server_v1.Network.CallBack {
    public class Database_CallBackMethods {

        public void RegisterCallBackMethods() {
            Server.DB_Register(DataBaseType.Longin, Login); //用戶登陸 
            Server.DB_Register(DataBaseType.Check, Check);  //查詢資料
            Server.DB_Register(DataBaseType.getGameData, GetGameData);  //獲取遊戲記錄
            Server.DB_Register(DataBaseType.Testing, Testing);//測試
        }

        //測試
        private void Testing(Player player, byte[] Head, byte[] Body) {

            Thread.Sleep(3000);
            Console.WriteLine("Test Complete");
        }

        #region 一般數據
        private void Check(Player player, byte[] Head, byte[] Body) {
            Console.WriteLine("成功創建 數據庫線程");    //測試
        }
        #endregion

        #region 獲取指定數據

        /// <summary>
        /// 用戶查詢（登入）
        /// </summary>
        /// <param name="player"></param>
        /// <param name="Head"></param>
        /// <param name="Body"></param>
        private void Login(Player player, byte[] Head, byte[] Body) {

            Unpack Unpack = new Unpack();
            Send Send = new Send();
            BuildAccount buildAccount = new BuildAccount();                         //GameData

            long ID = Unpack.Head_SessionID(Head);                                  //解析用戶ID
            bool isNewPlayer = MySqlCmd.Login(ID, Unpack.Body_StringData(Body));    //數據庫命令（查詢帳戶是否存在）

            #region 確認玩家身份（新玩家 or 老玩家）
            byte[] LoginPackage;

            if (!isNewPlayer) {
                LoginPackage = buildAccount.Execute();                              //為新玩家建立資料表
                ID = buildAccount.NewID;
            } else {
                MySqlCmd.UpdateLoginTime(ID);                                       //更新登陸時間
                LoginPackage = new byte[0];                                         //創建長度為0的內容   
            }
            #endregion

            Send.BytePacket(player, PackageType.Login, LoginPackage);               //發送封包
            player.AccountID = ID;                                                  //設置本地玩家ID
            Console.WriteLine($"玩家上線: {ID}");
        }

        /// <summary>
        /// 獲取用戶遊戲資料包
        /// </summary>
        /// <param name="player"></param>
        /// <param name="Head"></param>
        /// <param name="Body"></param>
        private void GetGameData(Player player, byte[] Head, byte[] Body) {
            GetData getData = new GetData(player);
            getData.Execute();
        }

        #endregion

    }
}
