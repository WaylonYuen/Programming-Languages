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
import math
import time

import Robot_Reference as robot_ref
import Position as Pos


class Robot:

    # Local values
    Position = object  # 座標位置類別:指定型態

    # Constructor
    # ！參數需指定型態 -> 確保類成員變數的安全性
    def __init__(self, PlayerFrontCenterX: int, PlayerFrontCenterY: int, PlayerBackCenterX: int, PlayerBackCenterY: int):
        # 將位置參數傳遞給Position類
        self.Position = Pos.Position(PlayerFrontCenterX, PlayerFrontCenterY, PlayerBackCenterX, PlayerBackCenterY)  # new物件

    # PlayerVector X PlayerBallVector
    @property
    def PlayerVecCrossBallVec(self):
        return np.cross(self.Position.Get_PlayerVector, self.Position.PlayerBallVector([self._Ball[Pos.X], self._Ball[Pos.Y]]))

    # PlayerVector的長度
    @property
    def PlayerVectorLength(self):
        return math.sqrt(math.pow(self.Position.Get_PlayerVector[Pos.X], 2) + math.pow(self.Position.Get_PlayerVector[Pos.Y], 2))

    # PlayerBallVector的長度
    @property
    def PlayerBallVectorLength(self):
        return math.sqrt(math.pow(self.Position.PlayerBallVector([self._Ball[Pos.X], self._Ball[Pos.Y]])[Pos.X], 2) + math.pow(self.Position.PlayerBallVector([self._Ball[Pos.X], self._Ball[Pos.Y]])[Pos.Y], 2))

    # (機器人面向向量)與(機器人和球的向量)的夾角
    @property
    def Theta(self):
        return math.asin(self.PlayerVecCrossBallVec / self.PlayerVectorLength / self.PlayerBallVectorLength)  # Cross = |A|*|B|*sin(Theta)

    # 踢球
    def KickBall(self):
        print("前踢(右踢)")
        robot_ref.Ser.write('K'.encode())

    # 朝球走過去
    def ToBall(self):
        if self.PlayerBallVectorLength > robot_ref.AREA:  # 人球距離大於安全距離

            if self.PlayerVecCrossBallVec > 0 and math.degrees(self.Theta) > 15:
                print('右轉')
                robot_ref.Ser.write('D'.encode())

            elif self.PlayerVecCrossBallVec < 0 and math.degrees(self.Theta) > 15:
                print('左轉')
                robot_ref.Ser.write('I'.encode())

            else:
                print('直走')
                robot_ref.Ser.write('W'.encode())

        else:
            self.KickBall()

    # 機器人先往球的方向向右或向左移動，再走到直線方程式上的一點
    def ToKick(self):

        if ((self._Ball[Pos.Y] - robot_ref.COOR_TOL) < self.Position.Get_PlayerCenter[Pos.X]
                and self.Position.Get_PlayerCenter[Pos.Y] < (self._Ball[Pos.Y] + robot_ref.COOR_TOL)):  # 機器人和球Y座標幾乎相同

            if ((self.Position.SlopeBallTarget([self._Ball[Pos.X], self._Ball[Pos.Y]]) - robot_ref.SLOPE_TOL) < self.Position.SlopePlayerBall(
                    [self._Ball[Pos.X], self._Ball[Pos.Y]]) and self.Position.SlopePlayerBall([self._Ball[Pos.X], self._Ball[Pos.Y]]) < (
                    self.Position.SlopeBallTarget([self._Ball[Pos.X], self._Ball[Pos.Y]]) + robot_ref.SLOPE_TOL)):  # 機器人和球的向量與球和目標的向量近乎一直線
                print("ToBall")
                self.ToBall()

            else:
                if self.Position.SlopeBallTarget([self._Ball[Pos.X], self._Ball[Pos.Y]]) < 0:  # 球和目標斜率為負
                    print('右側移')
                    robot_ref.Ser.write('G'.encode())

                elif self.Position.SlopeBallTarget([self._Ball[Pos.X], self._Ball[Pos.Y]]) > 0:  # 球和目標斜率為正
                    print('左側移')
                    robot_ref.Ser.write('F'.encode())

                else:
                    robot_ref.Ser.write('W'.encode())  # 往前走

        elif self.Position.Get_PlayerCenter[Pos.Y] > self._Ball[Pos.Y] + robot_ref.COOR_TOL:  # 機器人Y座標 > 球Y座標
            print('右側移')
            robot_ref.Ser.write('G'.encode())

        else:  # 機器人Y座標 < 球Y座標
            print('直走')
            robot_ref.Ser.write('W'.encode())

    def Read_MetaData(self):
        while True:
            try:
                '''讀取數值'''
                # 內存洩漏
                value = np.load('seat.npy')

                # Ball = [value[0], value[1]]
                self.Ball([value[Pos.X], value[Pos.Y]])

                # Defender = [value[2], value[3]]
                self.Defender([value[2], value[3]])

                robotHarry.PlayerBackCenterX = value[4]  # robotHarry.PlayerBackCenter = [value[4], value[5]]
                robotHarry.PlayerBackCenterY = value[5]
                robotHarry.PlayerFrontCenterX = value[6]  # robotHarry.PlayerFrontCenter = [value[6], value[7]]
                robotHarry.PlayerFrontCenterY = value[7]

                # print('a' + str(value[7]))

                print("球座標(", self._Ball[Pos.X], ",", self._Ball[Pos.Y], ")")
                print("守門員座標(", self._Defender[Pos.X], ",", self._Defender[Pos.Y], ")")
                print("RobotHarry背座標(", robotHarry.PlayerBackCenterX, ",", robotHarry.PlayerBackCenterY, ")")
                print("RobotHarry前座標(", robotHarry.PlayerFrontCenterX, ",", robotHarry.PlayerFrontCenterY, ")")

                '''執行'''
                robotHarry.ToKick()

                # print("結束")
                # break
                time.sleep(1)

            except KeyboardInterrupt:
                break


# 主程式判斷：先走到直線方程式上的一點，再朝球走去
robotHarry = Robot(470, 270, 485, 270)
robot_ref.Ser.write('A'.encode())

robotHarry.Read_MetaData()
