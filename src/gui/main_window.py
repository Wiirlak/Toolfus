from PySide6.QtCore import QTimer, Qt
from PySide6.QtGui import QIcon
from PySide6.QtWidgets import QVBoxLayout
from PySide6.QtWidgets import QWidget

from src.engine.utils import resource_path
from src.gui.layouts.settings import SettingsLayout
from src.gui.layouts.team import TeamLayout


class MainWidget(QWidget):
    def __init__(self):
        QWidget.__init__(self)

        self.setObjectName("MainWidget")
        self.setAttribute(Qt.WA_AlwaysShowToolTips, True)
        layout = QVBoxLayout(self)

        self.team_layout = TeamLayout()
        self.settings_layout = SettingsLayout()
        layout.addWidget(self.team_layout)
        layout.addWidget(self.settings_layout)
        self.setLayout(layout)
        self.refresh()

        timer = QTimer(self)
        timer.timeout.connect(self.refresh)
        timer.start(1000)

    def refresh(self):
        self.team_layout.refresh()

    def _change_icon(self, icon: QIcon):
        self.setWindowIcon(icon)

    def _change_background_color(self, color: str):
        self.setStyleSheet(f"background-color: {color}")

    def set_active(self, active: bool):
        if active:
            self._change_icon(QIcon(resource_path("res/images/TFon.ico")))
            # self._change_background_color("#212121")
        else:
            self._change_icon(QIcon(resource_path("res/images/TFOff.ico")))
