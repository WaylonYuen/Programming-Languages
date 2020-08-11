def zero():
    return "普通"


def fifty():
    return "當沖單"


def sixty():
    return "組合單"


# 選擇器
def switcher(command):
    menu = {
        '0': zero,
        '50': fifty,
        '60': sixty,
        '4': lambda: print('Error')
    }

    func = menu.get(command, '4')  # 从map中取出方法
    return func()  # 执行
