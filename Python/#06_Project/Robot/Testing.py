# 著作權聲明
#   Label #008-Challenge2
#   003- 加上五種狀況的判斷 - 5 kinds of conditional decisions.py
#   Version 3.3
#
#   Created by XuanRen.Hsu on 2020/06/16.
#   Last modified by XuanRen.Hsu on 2020/06/16.
#   Copyright © 2020年 Hsu,Xuan-Ren. All rights reserved.
#
#   說明：以 (1)機器人和球Y座標相同 (2)人和球距離>Area (3)人離框15公分 = 90px \
#           (4)人Y座標沒 > 或 < 球 10cm (5)人球向量和球目標向量相同
#            ！：1cm改3px
#   操作：判斷順序4152523
#   原理：物件導向程式設計
#   優化：X
#

# import cv2
import numpy as np
import serial
import math
import time

# 序列埠
Ser = serial.Serial('COM5', 9600)     #初始化序列通訊埠 (指定埠號名稱, 設定傳輸速率)

# 踢球
KICK = 30                                                         # 踢球距離
AREA = KICK + 15                                                  # 安全靠近球距離：避免機器人誤觸球
SLOPE_TOL = 2                                                     # 斜率之容錯範圍
COOR_TOL = 45                                                     # 座標之容錯範圍 (單位:px) (目前15公分)

Door = [[5, 150],[5, 360]]                                      # 設定球門座標[上,下]
Target = [TargetX,TargetY] = [5,210]                             # 最終目標點
Ball = [BallX, BallY] = [270, 270]                                # 球的位置
Defender = [DefenderX, DefenderY] = [10, 225]                     # 守門員之座標(傳入參數的Value[2], value[3])

class Robot:
    def __init__(self, PlayerFrontCenterX, PlayerFrontCenterY, PlayerBackCenterX, PlayerBackCenterY):
        '所有機器人的基類'
        self.PlayerFrontCenterX = PlayerFrontCenterX
        self.PlayerFrontCenterY = PlayerFrontCenterY
        self.PlayerBackCenterX = PlayerBackCenterX
        self.PlayerBackCenterY = PlayerBackCenterY

    @property
    def Position(self):
        self.PlayerFrontCenter = [self.PlayerFrontCenterX, self.PlayerFrontCenterY]       # 頭頂為27*27 pixel
        self.PlayerBackCenter = [self.PlayerBackCenterX, self.PlayerBackCenterY]

        self.PlayerCenterX = (self.PlayerFrontCenterX + self.PlayerBackCenterX) / 2
        self.PlayerCenterY = (self.PlayerFrontCenterY + self.PlayerBackCenterY) / 2
        self.PlayerCenter = np.array([self.PlayerCenterX,  self.PlayerCenterY])                        # 頭頂中點 = (PlayerFrontCenter + PlayerBackCenter) / 2

        self.PlayerVectorX = self.PlayerBackCenterX - self.PlayerFrontCenterX
        self.PlayerVectorY = self.PlayerBackCenterY - self.PlayerFrontCenterY
        self.PlayerVector = np.array([self.PlayerVectorX, self.PlayerVectorY])                         # Player的面相向量：PlayerBackCentor - PlayerFrontCentor = [13.5, 0]
        self.SlopePlayer = self.PlayerVectorY / self.PlayerVectorX

        self.PlayerBallVectorX = BallX - self.PlayerBackCenterX
        self.PlayerBallVectorY = BallY - self.PlayerBackCenterY
        self.PlayerBallVector = [self.PlayerBallVectorX, self.PlayerBallVectorY]                       # 人到球的向量：Ball - PlayerBackCentor = [-870, -440]

        # Equation
        self.SlopeBallTarget = (TargetY - BallY) / (TargetX - BallX)                                   # Ball到Target的斜率 = (Y2-Y1) / (X2-X1)
        self.SlopePlayerBall = (self.PlayerVectorY - BallY) / (self.PlayerVectorX - BallX)             # Player到Ball的斜率

    # (機器人面向向量)與(機器人和球的向量)的夾角
    def Theta(self):
        self.PlayerVecCrossBallVec = np.cross(self.PlayerVector, self.PlayerBallVector)                        # PlayerVector X PlayerBallVector
        self.PlayerVectLength = math.sqrt(math.pow(self.PlayerVectorX, 2) + math.pow(self.PlayerVectorY, 2))               # PlayerVector的長度
        self.PlayerBallVectorLength = math.sqrt(math.pow(self.PlayerBallVectorX, 2) + math.pow(self.PlayerBallVectorY, 2)) # PlayerBallVector的長度
        self.Theta = math.asin(self.PlayerVecCrossBallVec / self.PlayerVectLength / self.PlayerBallVectorLength)                # Cross = |A|*|B|*sin(Theta)
        return self.Theta

    # 踢球
    def KickBall(self):
        print("前踢(右踢)")
        Ser.write('K'.encode())

    # 朝球走過去
    def ToBall(self):
        if(self.PlayerBallVectorLength > AREA) :     #人球距離大於安全距離
            if (self.PlayerVecCrossBallVec > 0 and math.degrees(self.Theta()) > 15) :
                print('右轉')
                Ser.write('D'.encode())

            elif (self.PlayerVecCrossBallVec < 0 and math.degrees(self.Theta()) > 15) :
                print('左轉')
                Ser.write('I'.encode())

            else :
                print('直走')
                Ser.write('W'.encode())
        else :
            self.KickBall()

    # 機器人先往球的方向向右或向左移動，再走到直線方程式上的一點
    def ToKick(self):
        if((BallY - COOR_TOL) < self.PlayerCenterY and self.PlayerCenterY < (BallY + COOR_TOL)) :     # 機器人和球Y座標幾乎相同
            if((self.SlopeBallTarget - SLOPE_TOL) < self.SlopePlayerBall and self.SlopePlayerBall < (self.SlopeBallTarget + SLOPE_TOL)) :     # 機器人和球的向量與球和目標的向量近乎一直線
                print("ToBall")
                self.ToBall()
            else :
                if(self.SlopeBallTarget < 0) :             #球和目標斜率為負
                        print('右側移')
                        Ser.write('G'.encode())
                elif(self.SlopeBallTarget > 0) :           #球和目標斜率為正
                        print('左側移')
                        Ser.write('F'.encode())
                else:
                    Ser.write('W'.encode())                 #往前走
        elif(self.PlayerCenterY > BallY + COOR_TOL):       # 機器人Y座標 > 球Y座標
            print('右側移')
            Ser.write('G'.encode())
        else :                                              # 機器人Y座標 < 球Y座標
            print('直走')
            Ser.write('W'.encode())


# 主程式判斷：先走到直線方程式上的一點，再朝球走去
robotHarry = Robot(470, 270, 485, 270)
Ser.write('A'.encode())
while True :
    try:
        '''讀取數值'''
        value = np.load('seat.npy')

        BallX = value[0]                                    # Ball = [value[0], value[1]]
        BallY = value[1]
        DefenderX = value[2]                                # Defender = [value[2], value[3]]
        DefenderY = value[3]
        robotHarry.PlayerBackCenterX = value[4]             # robotHarry.PlayerBackCenter = [value[4], value[5]]
        robotHarry.PlayerBackCenterY = value[5]
        robotHarry.PlayerFrontCenterX = value[6]            # robotHarry.PlayerFrontCenter = [value[6], value[7]]
        robotHarry.PlayerFrontCenterY = value[7]

        print('a' + str(value[7]))

        print("球座標(", BallX, ",",  BallY, ")")
        print("守門員座標(", DefenderX, ",", DefenderY, ")")
        print("RobotHarry背座標(", robotHarry.PlayerBackCenterX, ",", robotHarry.PlayerBackCenterY, ")")
        print("RobotHarry前座標(", robotHarry.PlayerFrontCenterX, ",", robotHarry.PlayerFrontCenterY, ")")


        '''執行'''
        robotHarry.ToKick()

        # print("結束")
        # break
        time.sleep(1)
    except KeyboardInterrupt:
        break
    except:
        continue
