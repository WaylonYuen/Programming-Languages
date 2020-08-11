def B():
    return "普通買進"


def S():
    return "普通賣出"


def MB():
    return "融資買入"


def RB():
    return "融資賣出"


def RS():
    return "融券買入"


def MS():
    return "融券賣出"


# 選擇器
def switcher(command):
    menu = {
        'B': B,
        'S': S,
        'MB': MB,
        'RB': RB,
        'RS': RS,
        'MS': MS,
        '4': lambda: print('Error')
    }

    func = menu.get(command, '4')  # 从map中取出方法
    return func()  # 执行
