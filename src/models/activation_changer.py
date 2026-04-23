from PySide6.QtGui import QIcon

from src.engine.utils import resource_path


class ActivationChanger:

    def __init__(self):
        self._main_window = None
        self._tray_window = None

    @property
    def main_window(self):
        if self._main_window is None:
            raise ValueError("ActivationChanger has not been setup")
        return self._main_window

    @property
    def tray_window(self):
        if self._tray_window is None:
            raise ValueError("ActivationChanger has not been setup")
        return self._tray_window

    def setup(self, main_window, tray_window):
        self._main_window = main_window
        self._tray_window = tray_window

    def set_active(self, active: bool):
        self.main_window.set_active(active)
        if active:
            self._tray_window.setIcon(QIcon(resource_path("res/images/TFon.ico")))

        else:
            self._tray_window.setIcon(QIcon(resource_path("res/images/TFoff.ico")))
