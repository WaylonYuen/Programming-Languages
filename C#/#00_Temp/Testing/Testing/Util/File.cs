using System;
using System.IO;
using System.Text;

namespace Testing.Util {

    public class File {


        public static string[] Read(string srcPath) {

            string[] data = null;

            //如果文件不存在
            if (!System.IO.File.Exists(srcPath)) {
                Console.WriteLine($"Cannot found the file. {srcPath}");
                return null;
            }

            FileStream fs = new FileStream(srcPath, FileMode.Open, FileAccess.Read);

            try {

                // Move file pointer to beginning of file.
                fs.Seek(-4, SeekOrigin.End);

                var bys_dataLenght = new byte[4];
                fs.Read(bys_dataLenght);
                int NumOfData = BitConverter.ToInt32(bys_dataLenght, 0);

                data = new string[NumOfData];

                fs.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < NumOfData; i++) {

                    fs.Read(bys_dataLenght);
                    int dataLenght = BitConverter.ToInt32(bys_dataLenght, 0);

                    var bys_data = new byte[dataLenght];
                    fs.Read(bys_data, 0, bys_data.Length - 1);
                    fs.Position++;

                    data[i] = Encoding.UTF8.GetString(bys_data);
                }

            } catch (IOException ex) {
                Console.WriteLine("An IO exception has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadKey();

            } finally {
                fs.Close();
            }

            Console.WriteLine("Read Done!");
            return data;
        }

        public static void Write(string srcPath, string[] strs_data) {

            //如果文件已存在,則刪除文件
            if (System.IO.File.Exists(srcPath)) {
                System.IO.File.Delete(srcPath);
            }

            FileStream fs = new FileStream(srcPath, FileMode.Create, FileAccess.Write);

            try {
                // Move file pointer to beginning of file.
                fs.Seek(0, SeekOrigin.Begin);

                for (int i = 0; i < strs_data.Length; i++) {

                    var bys_data = Encoding.UTF8.GetBytes(strs_data[i] + "\n");
                    var bys_dataLenght = BitConverter.GetBytes(bys_data.Length);

                    fs.Write(bys_dataLenght, 0, bys_dataLenght.Length);
                    fs.Write(bys_data, 0, bys_data.Length);
                }

                var bys_count = BitConverter.GetBytes(strs_data.Length);
                fs.Write(bys_count, 0, bys_count.Length);

            } catch (IOException ex) {
                Console.WriteLine("An IO exception has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadKey();

            } finally {
                fs.Close();
            }

            Console.WriteLine("Write Done!");
        }


        //public static byte[] Read(string srcPath, FileMode fileMode, int Offset, long ReadPosition) {

        //    if (System.IO.File.Exists(srcPath)) {
        //        using (FileStream fileStream = new FileStream(srcPath, fileMode, FileAccess.Read)) {

        //            if (fileStream.CanRead) {
        //                fileStream.Position = ReadPosition;
        //                fileStream.Read()
        //            }
        //        }
        //    }

        //}

        //public static byte[] Write() {

        //}

        ///// <summary>
        ///// 文件讀寫
        ///// </summary>
        ///// <param name="srcPath">讀取路徑</param>
        ///// <param name="Save_Byte">需寫入的內容</param>
        ///// <param name="DataSize">內容大小</param>
        ///// <param name="fileMode">處理模式</param>
        ///// <param name="fileAccess">處理方法</param>
        ///// <param name="Offset">讀寫偏差值</param>
        ///// <param name="ReadPosition">讀寫起點</param>
        ///// <returns>讀取到的資料</returns>
        //public static byte[] FileSet(string srcPath, byte[] Save_Byte, int DataSize, FileMode fileMode, FileAccess fileAccess, int Offset, long ReadPosition) {

        //    byte[] data_Byte = new byte[DataSize];   //創建容器

        //    if (Save_Byte != null) data_Byte = Save_Byte;

        //    if (System.IO.File.Exists(srcPath)) {
        //        using (FileStream fileStream = new FileStream(srcPath, fileMode, fileAccess)) {
        //            fileStream.Position = ReadPosition;
        //            if (fileAccess == FileAccess.Read) fileStream.Read(data_Byte, Offset, data_Byte.Length);
        //            if (fileAccess == FileAccess.Write) fileStream.Write(data_Byte, Offset, data_Byte.Length);
        //        }
        //    } else {
        //        Console.WriteLine($"#  Warning: Cannot found the Path\t Info [Path: {srcPath} ]");
        //    }
        //    return data_Byte;
        //}
    }

}
