
def LMTU():
    return "漲停價"


def LMTD():
    return "跌停價"


def LMT():
    return "限價單"


def MKT():
    return "期權市價單"


# 選擇器
def switcher(command):
    menu = {
        'LMTU': LMTU,
        'LMTD': LMTD,
        'LMT': LMT,
        'MKT': MKT,
        '4': lambda: print('Error')
    }

    func = menu.get(command, '4')  # 从map中取出方法
    return func()  # 执行
