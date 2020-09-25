using System;

namespace Server_v1.Network {

    /// <summary>
    /// 封包型態定義，決定此封包的目的（e.g: 實時同步、 Msg、 Login訊息）
    /// </summary>
    public enum PackageType {
        None,           //無         必須定義DataType
        Test,

        /*系統封包*/
        HeartBeat,      //心跳包       統一空包
        Connection,     //連線封包      Bool

        /*玩家請求*/
        DataBase,       //數據庫       DatabaseType

        /*遊戲封包*/
        Message,        //文字消息      無:暫定String
        GameData,       //遊戲資料      Byte[]
        Synchronize,    //資料同步      無

        Login,          //登入封包      String

        Type,           //指定型態      可去除
    }

    /// <summary>
    /// 封包內容型態定義，決定以什麼型態封裝、序列化（e.g: int、 float、 double、 string）//請忽修改次序, 會打亂switch的次序
    /// </summary>
    public enum DataType {  //請忽修改次序, 會打亂switch的次序
        None,           //無            必須定義PackageType
        Test,
        StringData,     //字串型態內容
        VectorData,     //座標型態內容 改
        IntData,        //整數型態內容    Int32   4bytes
        LongData,       //長整型態內容    Int64   8bytess
        FloatData,
        BoolData,       //布爾型態內容
        ByteData,       //字節型態內容

        MapData,        //地圖類型資料
        PlayerData,     //玩家類型資料
    }

    /// <summary>
    /// 資料庫行為定義（需和Client同步資料）
    /// </summary>
    public enum DataBaseType {

        /*一般數據*/
        None,           //無
        Test,

        Check,          //查詢資料（對比資料）    bool（查到:True）
        Insert,         //插入資料（追加資料）    bool（成功:True）   保存指定資料（保存單項資料,屬於覆蓋類型）
        Update,         //更新資料（覆蓋資料）    bool（成功:True）
        Delete,         //刪除資料              bool（成功:True）

        /*獲取 指定數據*/
        Longin,         //登入    封包內容String
        getData,        //獲取指定資料（讀取單項資料,要檢查是否存在）
        getPlayerData,  //獲取帳戶資料（Name、ID、Level）
        getGameData,    //獲取遊戲資料（遊戲紀錄、）
        getMapData,

        /*保存 指定數據*/
        savePlayerData, //保存帳戶資料
        saveGameData,   //保存遊戲資料

        Testing,
    }

    /// <summary>
    /// 加密方式選擇
    /// </summary>
    public enum EncryptionType {
        None,
        Test,

        RES256,
    }

}
