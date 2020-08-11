

class CmdURL:

    _targetIP = "N/A"
    _getIP = "N/A"

    def __init__(self, targetIP: str, getIP: str):
        self._targetIP = targetIP
        self._getIP = getIP

    def get_login_cmd_url(self, user: str, pw: str):
        return "http://" + self._targetIP + "/weborder/autotrade.asmx/LoginIP?" \
                "UserName=" + user + \
                "&Password=" + pw + \
                "&IPAddr=" + self._getIP

    userInfo = {
        "GMRID": "",
        "GMRIDStr": "",
        "UserName": "",
        "NickName": "",
        "FirstName": "",
        "LastName": "",
        "GameCode": "",
        "LoginTime": "",
        "FromIP": ""
    }

    def get_user_data_cmd_url(self, token: str):
        return "http://" + self._targetIP + "/weborder/autotrade.asmx/TokenFactory?" \
                "TokenString=" + token

    order = {
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

    order_ptr = dict

    def get_order_cmd_url(self, order: dict):
        self.order_ptr = order
        return "http://" + self._targetIP + "/weborder/autotrade.asmx/PutOrderXML3?" \
                "GMRIDStr=" + order["GMR_IDStr"] + \
                "&CompCode=" + order["CompCode"] + \
                "&Price=" + order["Price"] + \
                "&Volume=" + order["Volume"] + \
                "&BSAction=" + order["BSAction"] + \
                "&OrderType=" + order["OrderType"] + \
                "&IsOddLot=" + order["IsOddLot"] + \
                "&Currency=" + order["Currency"] + \
                "&OrderNote=" + order["OrderNote"] + \
                "&OCType=" + order["OCType"] + \
                "&CombineNo=" + order["CombineNo"] + \
                "&OrderParameter=" + order["OrderParameter"] + \
                "&Lang=" + order["Lang"] + \
                "&str_ip=" + order["str_ip"]
