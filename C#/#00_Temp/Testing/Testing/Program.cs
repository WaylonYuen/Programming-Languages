using System;
using System.IO;
using System.Text;
using DatabaseTesting;
using Testing.Net;

namespace Testing {

    class Program {
        static void Main(string[] args) {

            //發布者
            Headquarters headquarters = new Headquarters();

            //訂閱者
            Inspector inspector1 = new Inspector("Greg Lestrade");
            Inspector inspector2 = new Inspector("Sherlock Holmes");
      
            //訂閱
            inspector1.Subscribe(headquarters);
            inspector2.Subscribe(headquarters);

            //發布消息
            headquarters.SendMessage(new Message("Catch Moriarty!"));
            headquarters.EndTransmission();

            Console.ReadKey();

        }
    }

    public static class Demo {

        public static void SocketTest() {
            var client = new SocketTest();
            client.Start();
        }

        public static void FileTest() {
            string[] data = new string[10];

            for (int i = 0; i < data.Length; i++) {
                data[i] = $"Line {i} : Data";
            }

            Util.File.Write("Temp.dat", data);

            var result = Util.File.Read("Temp.dat");

            for (int i = 0; i < result.Length; i++) {
                Console.WriteLine(result[i]);
            }
        }

        public static void DatabaseTest() {
            //開啟數據庫連線
            DataBase dataBase = new DataBase(); //創建數據庫連接
            dataBase.Start();  //開啟數據庫
        }


    }
}
