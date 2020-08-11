import numpy as np
import Robot_Reference as robot_ref
import Func_Math as math
import math

# Global values 可能要換成常數寫法
X = 0
Y = 1


class Position:

    # Local Values
    _PlayerFrontCenter: list
    _PlayerBackCenter: list

    # constructor
    def __init__(self, PlayerFrontCenter: list, PlayerBackCenter: list):
        self._PlayerFrontCenter = PlayerFrontCenter
        self._PlayerBackCenter = PlayerBackCenter

    @property
    def Get_PlayerFrontCenter(self):
        return self._PlayerFrontCenter  # 頭頂為27*27 pixel

    @property
    def Get_PlayerBackCenter(self):
        return self._PlayerBackCenter

    @property
    def Get_PlayerCenter(self):
        # 如果需要調用 CenterX or CenterY 可以直接取得此Prop通過 np.array[index]取得.
        # example -> PlayerCenter[X] //即取得CenterX, [X]的X為index = 0; Y = 1. 於Global values 處定義 & 可直接調用
        _PlayerCenterX = (self._PlayerFrontCenter[X] + self._PlayerBackCenter[X]) / 2
        _PlayerCenterY = (self._PlayerFrontCenter[Y] + self._PlayerBackCenter[Y]) / 2

        # 頭頂中點 = (PlayerFrontCenter + PlayerBackCenter) / 2
        return np.array([_PlayerCenterX, _PlayerCenterY])  # return object (參數) [list]

    @property
    def Get_PlayerVector(self):
        # 如果需要調用 VectorX or VectorY 可以直接取得此Prop通過 np.array[index]取得.
        # example -> PlayerCenter[X] //即取得VectorX, [X]的X為index = 0; Y = 1. 於Global values 處定義 & 可直接調用
        _PlayerVectorX = self._PlayerBackCenter[X] - self._PlayerFrontCenter[X]
        _PlayerVectorY = self._PlayerBackCenter[Y] - self._PlayerFrontCenter[Y]

        # Player的面相向量：PlayerBackCentor - PlayerFrontCentor = [13.5, 0]
        return np.array([_PlayerVectorX, _PlayerVectorY])  # return object

    @property
    def Get_SlopePlayer(self):
        return self.Get_PlayerVector[Y] / self.Get_PlayerVector[X]

    def PlayerBallVector(self, Ball: list):
        # 如果需要調用 VectorX or VectorY 可以直接取得此Prop通過 np.array[index]取得.
        # example -> PlayerCenter[X] //即取得VectorX, [X]的X為index = 0; Y = 1. 於Global values 處定義 & 可直接調用
        _PlayerBallVectorX = Ball[X] - self.PlayerBackCenterX
        _PlayerBallVectorY = Ball[Y] - self.PlayerBackCenterY

        # 人到球的向量：Ball - PlayerBackCentor = [-870, -440]
        return [_PlayerBallVectorX, _PlayerBallVectorY]

    def SlopeBallTarget(self, Ball: list):
        # Ball到Target的斜率
        return math.Slope([robot_ref.TargetY, robot_ref.TargetX], [Ball])

    def SlopePlayerBall(self, Ball: list):
        # Player到Ball的斜率
        return math.Slope([self.Get_PlayerVector[X], self.Get_PlayerVector[Y]], Ball)






