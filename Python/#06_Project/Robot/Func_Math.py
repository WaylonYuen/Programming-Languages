
# Global values
X = 0
Y = 1


# 斜率 = (Y2-Y1) / (X2-X1)
def Slope(Coordinate_A: list, Coordinate_B: list):
    return Coordinate_A[Y] - Coordinate_B[Y] / Coordinate_A[X] - Coordinate_B[X]
