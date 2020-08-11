import time
import getpass
import Apps

App = Apps.FinancialAPP("61.220.30.176", "127.0.0.1")


# 顯示菜單
def show():
    print("Menu list\t\t\t\t\t   Financial App Project")
    print("=========================================== " + time.strftime("%H:%M:%S", time.localtime()))
    print(" - m -\t ...................   show menu")
    print(" - c -\t ...................   Clear screen")
    print(" - 1 -\t ...................   login")
    print(" - 2 -\t ...................   show user data")
    print(" - 3 -\t ...................   order")
    print(" - 0 -\t ...................   end of run...")
    print("-------------------------------------------------End")
    return True


# 登入
def login():
    print("\nEnter:")
    user = input('User id >>> ')  # "FAPPS06"
    pw = input('Password >>> ')
    # pw = getpass.getpass('Enter password : ')  # "1234"
    print("-------------------------------------------------End")
    print("Checking...\n")
    App.login(user, pw)
    return True


# 獲取用戶資料
def show_user_data():
    App.show_user_data()
    return True


orderMenu = {
    "GMR_IDStr": "",
    "CompCode": "",
    "Price": "",
    "Volume": "",
    "BSAction": "",
    "OrderType": "",
    "IsOddLot": "",
    "Currency": "",
    "OrderNote": "",
    "OCType": "",
    "CombineNo": "",
    "OrderParameter": "",
    "Lang": "",
    "str_ip": ""
}


# 下單
def order():  # "4977.tw", "112", "3000", "B", "LMT"
    print("\nEnter:")
    orderMenu["GMR_IDStr"] = App.get_GMRIDStr()
    orderMenu["CompCode"] = input(" -> 股票代碼.tw : ")
    orderMenu["Price"] = input(" -> 價格 : ")
    orderMenu["Volume"] = input(" -> 數量 : ")

    print("\ndefualt(B), 普通買進(B), 普通賣出(S), 融資買入(MB), 融資賣出(RB), 融券買入(RS), 融券賣出(MS)")
    orderMenu["BSAction"] = input(" -> 請輸入提示碼 : ")

    print("\ndefualt(LMT), 漲停價(LMTU), 跌停價(LMTD), 限價單(LMT), 期權市價單(MKT)")
    orderMenu["OrderType"] = input(" -> 請輸入提示碼 : ")

    orderMenu["IsOddLot"] = "0"  # 預設給零  0: normal (照ASSET CATALOG中的交易時間), 1:for TW 零股交易
    orderMenu["Currency"] = "TWD"
    orderMenu["OrderNote"] = "ROD"

    print("\ndefualt(0), 現貨固定(0), 期貨作多(buy), 做空(sell), 選擇權新倉(0), 平倉(C)")
    orderMenu["OCType"] = input(" -> 請輸入提示碼 : ")

    orderMenu["CombineNo"] = " "

    print("\ndefualt(0), 普通(0), 當沖單(50), 組合單(60)")
    orderMenu["OrderParameter"] = input(" -> 請輸入提示碼 : ")

    orderMenu["Lang"] = "TC"
    orderMenu["str_ip"] = "127.0.0.1"

    App.order(orderMenu)
    return True


def clear():
    index = 0
    while index <= 50:
        print("\n")
        index = index + 1

    return True


# 退出程序
def end_of_run():
    print("\n\n----------------------- Stop")
    print("End of run...\t\t" + time.strftime("%H:%M:%S", time.localtime()))
    return False


# 選擇器
def switcher(command):
    menu = {
        'm': show,
        'c': clear,
        '1': login,
        '2': show_user_data,
        '3': order,
        '0': end_of_run,
        '4': lambda: print('Error')
    }

    func = menu.get(command, '4')  # 从map中取出方法
    return func()  # 执行
