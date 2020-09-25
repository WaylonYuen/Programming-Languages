using System;
using System.Net.Sockets;

using Server_v1.Network.Util;

namespace Server_v1.Client {

    /// <summary>
    /// 關於與玩家的封包資訊協議
    /// </summary>
    public class Player {

        //玩家資料
        public Socket Socket { get; private set; }                              //Player Socket
        public long AccountID { get; set; }                                     //帳戶ID 

        //玩家狀態(動態)
        public bool IsConnected { get; set; }                                   //是否還在線
        public bool Responese { get; set; }                                     //響應回應

        //封包相關
        public int Crccode { get; set; }                                        //封包驗證碼
        public long SessionID { get; set; }                                     //身份ID

        public int EncryptionMethod { get; set; }                               //加密方式
        public EncryptionType EncryptionMethods { get; set; }                   //加密方式

        //Instance
        public Player(Socket socket) {
            Socket = socket;
            IsConnected = true;
            Responese = true;
        }
    }
}
