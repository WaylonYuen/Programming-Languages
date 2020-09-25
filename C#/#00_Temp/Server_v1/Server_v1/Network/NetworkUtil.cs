using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server_v1.Network {

    public class UBinder : SerializationBinder {
        public override Type BindToType(string assemblyName, string typeName) {
            Assembly ass = Assembly.GetExecutingAssembly();
            return ass.GetType(typeName);
        }
    }

    public static class NetworkUtil {

        /// <summary>
        /// GetIPv4() : 獲取IPv4訊息
        /// </summary>
        /// <returns>IPv4訊息</returns>
        public static string GetLocalIPv4() {
            string hostName = Dns.GetHostName();    //獲取主機名稱
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            for (int i = 0; i < ipEntry.AddressList.Length; i++) {
                //從IP地址列表中篩選出IPv4類型的IP地址
                if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    return ipEntry.AddressList[i].ToString();
            }
            return null;
        }

        /// <summary>
        /// Serialize() : 序列化
        /// </summary>
        /// <param name="obj">序列化物件</param>
        /// <returns>序列化後的資料</returns>
        public static byte[] Serialize(object obj) {

            //物件不為空 且可被序列化
            if (obj == null || !obj.GetType().IsSerializable)
                return null;

            BinaryFormatter formatter = new BinaryFormatter();  //創建物件

            using (MemoryStream stream = new MemoryStream()) {
                formatter.Serialize(stream, obj);
                byte[] data = stream.ToArray();
                return data;
            }
        }

        /// <summary>
        /// Deserialize() : 反序列化
        /// </summary>
        /// <typeparam name="T">序列化</typeparam>
        /// <param name="data">序列化資料</param>
        /// <returns>反序列化後的資料</returns>
        public static T Deserialize<T>(byte[] data) where T : class {

            //數據不為空 且T是可序列化的類型
            if (data == null || !typeof(T).IsSerializable)
                return null;

            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = new UBinder();
            using (MemoryStream stream = new MemoryStream(data)) {
                object obj = formatter.Deserialize(stream);
                return obj as T;
            }
        }

    }
}
