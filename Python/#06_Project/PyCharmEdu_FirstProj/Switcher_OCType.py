def buy():
    return "期貨作多"


def sell():
    return "做空"


def zero():
    return "選擇權新倉"


def C():
    return "平倉"


# 選擇器
def switcher(command):
    menu = {
        'buy': buy,
        'sell': sell,
        '0': zero,
        'C': C,
        '4': lambda: print('Error')
    }

    func = menu.get(command, '4')  # 从map中取出方法
    return func()  # 执行
