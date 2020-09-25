using System;
using MySql.Data.MySqlClient;
using MySql.Data;


namespace DatabaseTesting {

    /// <summary>
    /// 連接參數設定
    /// </summary>
    public static class RefConnection {

        //服務器
        public const string sHost = "192.168.0.129"; //192.168.0.129"; //NetworkUtil.GetLocalIPv4();                 //連接地址
        public const int sPort = 8088;                                          //連接端口

        //資料庫
        public const string dbHost = "127.0.0.1";                               //連接地址
        public const string dbUser = "root";                                    //用戶ID
        public const string dbName = "DigDeeperGameDB";                         //數據庫名稱
        public const string dbPort = "3306";                                    //連接端口
        public const string dbFormat = "utf8;";                                 //字型協議
        private const string dbPass = "Waylon943734";                           //數據庫密碼 

        //數據庫登陸字串
        public const string DBConnStr =
            "server=" + dbHost +
            ";user=" + dbUser +
            ";database=" + dbName +
            ";port=" + dbPort +
            ";password=" + dbPass +
            ";CharSet=" + dbFormat;
    }

    public class DataBase {

        //屬性
        public static MySqlConnection ConnDB { get; private set; }

        //Instance
        public DataBase() => ConnDB = new MySqlConnection(RefConnection.DBConnStr);

        public void Start() {

            try {
                ConnDB.Open();  //開啟數據庫
                Console.WriteLine($"數據庫已連接\t Info [Gate {RefConnection.dbHost}:{RefConnection.dbPort} | DataBase Name: {RefConnection.dbName} ]");
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("一切準備就緒...\n\n");
            } catch (MySqlException ex) {
                switch (ex.Number) {
                    case 0:
                        Console.WriteLine("@ Warning: 無法連線到資料庫,找不到資料庫.");
                        break;
                    case 1045:
                        Console.WriteLine("@ Warning: 使用者帳號或密碼錯誤,請再試一次.");
                        break;
                    default:
                        Console.WriteLine("@ Warning: 未開啟目標數據庫.");
                        break;
                }
            }
        }

        public void Close() => ConnDB.Close();


    }
}
