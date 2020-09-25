using System;
using System.Threading;//測試

using Server_v1;
using Server_v1.Client;

namespace Server_v1.Network.CallBack {

    public class Packet_CallBackMethods {

        /// <summary>
        /// 註冊不同類型的封包以及其類型的解讀、執行方法
        /// </summary>
        public static void RegisterCallBackMethods() {
            Server.PK_Register(PackageType.Test, Test);

            Server.PK_Register(PackageType.HeartBeat, HeartBeat);          //註冊心跳包回覆方法
            Server.PK_Register(PackageType.Synchronize, Synchronize);      //註冊實時同步回覆方法
            Server.PK_Register(PackageType.Connection, Connection);        //註冊連線回覆方法
            Server.PK_Register(PackageType.Message, Message);
            Server.PK_Register(PackageType.Login, Login);
            Server.PK_Register(PackageType.DataBase, DataBase);            //註冊調用數據庫方法
        }

        //測試
        private static void Test(Player player, byte[] Head, byte[] Body) {
            //Console.WriteLine("##### Testing");
        }

        #region 本地方法
        /// <summary>
        /// 心跳包回覆方法: 判斷玩家是否還處於連線狀態（判斷玩家是否掉線的機制）
        ///     #收到來自Client的心跳封包的處理流程:
        ///         ~確認封包驗證碼(驗證碼可以幫助Server辨識封包來源,避免黑客混入自製封包干預Server) //因為只需要加密驗證碼,所以整體加解密效率提升,無需全部加密.
        ///         ~
        /// </summary>
        /// <param name="player">玩家類結構</param>
        /// <param name="Head">封包Head</param>
        /// <param name="Body">封包Body</param>
        private static void HeartBeat(Player player, byte[] Head, byte[] Body) => player.Responese = true;

        /// <summary>
        /// 玩家離線封包
        /// </summary>
        /// <param name="player"></param>
        /// <param name="Head"></param>
        /// <param name="Body"></param>
        private static void Connection(Player player, byte[] Head, byte[] Body) {
            Unpack Unpack = new Unpack();
            player.IsConnected = Unpack.Body_BoolData(Body);//設置玩家離線資料
        }

        /// <summary>
        /// 登入請求封包(轉發給Database)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="Head"></param>
        /// <param name="Body"></param>
        private static void Login(Player player, byte[] Head, byte[] Body) => DataBase(player, Head, Body);   //轉發給Database處理
        #endregion

        #region 轉發方法
        /// <summary>
        /// 未完成
        /// 實時同步回覆方法: 多人Online,資訊共享同步(角色座標、角色行為、動畫等)
        ///     #收到來自Client的實時同步封包的處理流程:
        ///         ~確認封包驗證碼(驗證碼可以幫助Server or Client辨識封包來源,避免黑客混入自製封包干預Server or Client) //因為只需要加密驗證碼,所以整體加解密效率提升,無需全部加密.
        ///         ~查詢來源Client所處的Room成員訊息
        ///         ~異步轉發來自Client的實時同步封包給Room內的所有成員
        /// </summary>
        /// <param name="player">玩家類結構</param>
        /// <param name="Head">封包Head</param>
        /// <param name="Body">封包Body</param>
        private static void Synchronize(Player player, byte[] Head, byte[] Body) {

            //～確認Room
            //～(Room只有自己) ? 丟掉封包 : 轉發封包;

            //測試同步效果, 轉發給在線玩家
            foreach (Player i in Server.Players) {
                //如果不是自己的話, 就轉發出去
                if (player.SessionID != i.SessionID) {
                    //Network.Transpond_VectorPackage(i.Socket, Head, Body); //重新組合封包並且轉發出去
                }
            }
#if SHOW
        //Network.UnpackData(player.Socket, Head, Body);  //顯示資訊
#endif
        }

        /// <summary>
        /// 調用數據庫方法（將封包轉發給數據庫線程 -> 防止數據庫調用數據過久造成封包阻塞）
        /// </summary>
        /// <param name="player">玩家類結構</param>
        /// <param name="Head">封包Head</param>
        /// <param name="Body">封包Body</param>
        private static void DataBase(Player player, byte[] Head, byte[] Body) {
            Unpack Unpack = new Unpack();
            DataBaseType databaseType = Unpack.Head_DataBaseType(Head); //封包類別

            if (Server.DB_CallBacksDictionary.ContainsKey(databaseType)) {  //確認 合格封包的類別是否存在
                DB_CallBack callBack = new DB_CallBack(player, Head, Body, Server.DB_CallBacksDictionary[databaseType]); //將封包打包成列隊格式(即:添加來源玩家訊息,將封包分類)
                Server.DB_CallBackQueue.Enqueue(callBack);  //將合格的封包丟進數據庫列隊中(回調線程會在列隊中抓取封包解讀,並且根據封包類別去執行不同的方法)
            } else
                Console.WriteLine($"#### 錯誤 未知的封包型態: {(int)databaseType}  數據庫註冊表中未發現此類型態的描述！");  //未知的封包類別提示
        }
        #endregion

        #region 回覆方法


        #endregion

        //測試
        private static void Message(Player player, byte[] Head, byte[] Body) {
            //Network.UnpackData(player.Socket, Head, Body);
        }

    }
}
