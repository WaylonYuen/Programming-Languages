# 說明：保存了關於Robot必要的參數值

import serial

# 序列埠
# Ser = serial.Serial('COM5', 9600)  # 初始化序列通訊埠 (指定埠號名稱, 設定傳輸速率) -> 有BUG

# 踢球
KICK = 30  # 踢球距離
AREA = KICK + 15  # 安全靠近球距離：避免機器人誤觸球
SLOPE_TOL = 2  # 斜率之容錯範圍
COOR_TOL = 45  # 座標之容錯範圍 (單位:px) (目前15公分)

# ？
Door = [[5, 150], [5, 360]]  # 設定球門座標[上,下]
Target = [TargetX, TargetY] = [5, 210]  # 最終目標點
Ball = [BallX, BallY] = [270, 270]  # 球的位置
Defender = [DefenderX, DefenderY] = [10, 225]  # 守門員之座標(傳入參數的Value[2], value[3])
