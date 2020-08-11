import time
import Menu

localtime = time.asctime(time.localtime(time.time()))
print(localtime + "\n")
Menu.show()

running_flag = True

while running_flag:
    cmd = input('command (enter character) : ')
    running_flag = Menu.switcher(cmd)
