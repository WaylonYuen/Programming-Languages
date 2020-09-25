

namespace DatabaseTesting {

    class Program {
        static void Main(string[] args) {


            //開啟數據庫連線
            DataBase dataBase = new DataBase(); //創建數據庫連接
            dataBase.Start();  //開啟數據庫

        }
    }

    public static class Demo {

    }
}
