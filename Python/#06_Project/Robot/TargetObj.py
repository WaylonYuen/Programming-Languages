
class Football:
    _Ball = list

    @property
    def Ball(self):
        return self._Ball

    @Ball.setter
    def Ball(self, value):
        self._Ball = value


class Defender:
    _Defender = list

    @property
    def Defender(self):
        return self._Defender

    @Defender.setter
    def Defender(self, value):
        self._Defender = value
