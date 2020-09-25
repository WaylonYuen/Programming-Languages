using System;
using System.Net;
using System.Text;

namespace Testing.Converter {

    public class Converter {

        public static byte[] GetBytes(string[] Data) {

            //計算MsgBody的長度
            int DataBodyLenght = 0;

            for (int i = 0; i < Data.Length; i++) {
                if (Data[i] == "")
                    break;
                DataBodyLenght += Encoding.UTF8.GetBytes(Data[i]).Length;   //計算每個字串組中的長度總和 e.g: string[3] Data = {"Hello", "Welcome", "Hi"}; 分別為： 5 + 7 + 2 = DataBodyLenght;
            }

            //定義封包體的字節數組
            byte[] data_Byte = new byte[DataBodyLenght + (Data.Length * 4)];    //保留每個字串組前4Bytes，以保存每組的字串長度。

            //紀錄存入消息體數組的字節數目前的索引位置
            int TempIndex = 0;
            for (int i = 0; i < Data.Length; i++) {

                //單個消息，單個字串組
                byte[] Temp_Bytes = Encoding.UTF8.GetBytes(Data[i]); //將第i個字串組取出

                BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Temp_Bytes.Length)).CopyTo(data_Byte, TempIndex);    //計算第i個字組長度存放到TempIndex中
                TempIndex += 4; //右邊往左邊存， 1個int = 4bytes
                Temp_Bytes.CopyTo(data_Byte, TempIndex);    //存入
                TempIndex += Temp_Bytes.Length; //加上字串組長度，索引至新位置；準備存第二組字串組。
            }

            return data_Byte;
        }

    }
}
