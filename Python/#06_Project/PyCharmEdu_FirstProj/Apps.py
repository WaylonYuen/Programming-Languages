import requests
import time
import cmdURL
import xml.etree.ElementTree as ElementTree

import Switcher_BSAction
import Switcher_OrderType
import Switcher_OCType
import Switcher_OrderParameter


class FinancialAPP:
    # 成員變數
    _usrName = "N/A"
    _password = "N/A"
    _tokenString = "N/A"

    # 成員類別
    _CmdURL = object

    # 構造函數 -> Constructor
    def __init__(self, targetIP: str, getIP: str):
        self._CmdURL = cmdURL.CmdURL(targetIP, getIP)

    # 用戶登入
    def login(self, user: str, pw: str):
        self._usrName = user
        self._password = pw
        self._login_checking()

    # 取得用戶資料
    def _get_user_data(self):

        response = requests.get(self._CmdURL.get_user_data_cmd_url(self._tokenString.text))
        data = ElementTree.fromstring(response.text)

        # test
        index = -1
        for value in self._CmdURL.userInfo:
            index = index + 1
            self._CmdURL.userInfo[value] = data[index].text

    # 用戶登入檢查
    def _login_checking(self):

        # 獲取登入訊息
        response = requests.get(self._CmdURL.get_login_cmd_url(self._usrName, self._password))
        self._tokenString = ElementTree.fromstring(response.text)
        # print(self._tokenString)  # Show

        # 核對訊息
        if self._tokenString.text == "WS1020":
            print("Warning: 用戶不存在！")

        elif self._tokenString.text == "WS1021":
            print("Warning: 帳號or密碼錯誤！")

        elif self._tokenString.text == "WS1035":
            print("Warning: 使用者未加入任何一組競賽！")

        else:
            self._get_user_data()
            print("登入成功...")
            print("-------------------------------------------------End")

    def show_user_data(self):

        # Show
        # print("\n user data:\n" + str(self._CmdURL.userInfo))
        print("\nUSER DATA")
        print("=========================================== " + time.strftime("%H:%M:%S", time.localtime()))
        print("GMR ID\t\t = \t\t" + str(self._CmdURL.userInfo["GMRID"]))
        print("GMR ID Str\t = \t\t" + str(self._CmdURL.userInfo["GMRIDStr"]))
        print("User Name\t = \t\t" + str(self._CmdURL.userInfo["UserName"]))
        print("Nick Name\t = \t\t" + str(self._CmdURL.userInfo["NickName"]))
        print("First Name\t = \t\t" + str(self._CmdURL.userInfo["FirstName"]))
        print("Last Name\t = \t\t" + str(self._CmdURL.userInfo["LastName"]))
        print("Game Code\t = \t\t" + str(self._CmdURL.userInfo["GameCode"]))
        print("Login Time\t = \t\t" + str(self._CmdURL.userInfo["LoginTime"]))
        print("From IP\t\t = \t\t" + str(self._CmdURL.userInfo["FromIP"]))
        print("-------------------------------------------------End")

    # 下單
    def order(self, order: dict):

        response = requests.get(self._CmdURL.get_order_cmd_url(order))
        root = ElementTree.fromstring(response.text)

        if "Success" in root.text:
            print("\n明細")
            print("=====================================")
            print("股票代碼\t\t" + str(self._CmdURL.order_ptr["CompCode"]))
            print("價格\t\t\t" + str(self._CmdURL.order_ptr["Price"]))
            print("數量\t\t\t" + str(self._CmdURL.order_ptr["Volume"]))
            print("BS行為\t\t" + str(self._CmdURL.order_ptr["BSAction"]) + "\t\t\t" + Switcher_BSAction.switcher(str(self._CmdURL.order_ptr["BSAction"])))
            print("訂單類型\t\t" + str(self._CmdURL.order_ptr["OrderType"]) + "\t\t\t" + Switcher_OrderType.switcher(str(self._CmdURL.order_ptr["OrderType"])))
            print("OC類型\t\t" + str(self._CmdURL.order_ptr["OCType"]) + "\t\t\t" + Switcher_OCType.switcher(str(self._CmdURL.order_ptr["OCType"])))
            print("訂單參數\t\t" + str(self._CmdURL.order_ptr["OrderParameter"]) + "\t\t\t" + Switcher_OrderParameter.switcher(str(self._CmdURL.order_ptr["OrderParameter"])))
            print("-------------------------------------")
            print("下單時間 :\t" + time.asctime(time.localtime(time.time())))
            print("訂單狀態 :\t成功下單")

        elif "Failure" in root.text:
            print(root.text)

        print("-------------------------------------------------End\n")

    # Show token string
    def show_token(self):
        print("Token: " + self._tokenString.text)

    def get_GMRIDStr(self):
        return self._CmdURL.userInfo["GMRIDStr"]
