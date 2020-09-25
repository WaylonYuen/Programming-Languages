using System;

using Server_v1.Network;

namespace Server_v1 {
    public class ServerUtil {

        private static Random random;

        public static EncryptionType EncrytionMethod { get => EncryptionType.RES256; }   // # 未完成 暫時

        /// <summary>
        /// 隨機產生N位驗證碼
        /// </summary>
        /// <returns>驗證碼</returns>
        /// 
        public static int RandomNum(int LowerBound, int UpperBound) {
            random = new Random();
            int Num = random.Next(LowerBound, UpperBound);   //產生6位隨機數
            return Num;
        }

        /// <summary>
        /// 隨機產生字串KEY = password
        /// </summary>
        /// <returns></returns>
        public static string RandomKey(int Lenght, bool useArray) {

            char[] constant = {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

            string KEY = string.Empty;
            random = new Random();
            for (int i = 0; i < Lenght; i++) {
                KEY += constant[random.Next(62)].ToString();
            }

            if (useArray) return KEY;   //陣列格式(回傳)

            //轉換成非陣列String
            string[] ArrayKEY = { "N/A" };
            ArrayKEY[0] = KEY;
            string Key = ArrayKEY[0];

            return Key;
        }


    }
}
