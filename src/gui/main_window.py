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
        self.setMinimumWidth(260)
        layout = QVBoxLayout(self)
        layout.setContentsMargins(12, 12, 12, 12)
        layout.setSpacing(10)

        self.team_layout = TeamLayout()
        self.settings_layout = SettingsLayout()
        layout.addWidget(self.team_layout)
        layout.addWidget(self.settings_layout)
        self.setLayout(layout)
        self.apply_theme()
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

    def set_always_on_top(self, enabled: bool):
        is_top_most = bool(self.windowFlags() & Qt.WindowType.WindowStaysOnTopHint)
        if is_top_most == enabled:
            return
        self.setWindowFlag(Qt.WindowType.WindowStaysOnTopHint, enabled)
        if self.isVisible():
            self.show()

    def apply_theme(self):
        self.setStyleSheet("""
            QWidget#MainWidget {
                background-color: #16181f;
                color: #e8ebf2;
            }
            QWidget#TeamLayout, QWidget#SettingsLayout {
                background-color: #1e2230;
                border: 1px solid #2a3144;
                border-radius: 10px;
            }
            QWidget#CharacterCard {
                background-color: #262d40;
                border: 1px solid #343f59;
                border-radius: 8px;
            }
            QPushButton {
                background-color: #4d79ff;
                color: #ffffff;
                border: none;
                border-radius: 7px;
                padding: 6px 10px;
                font-weight: 600;
            }
            QPushButton:hover {
                background-color: #5f88ff;
            }
            QPushButton:pressed {
                background-color: #3f66d8;
            }
            QLabel#SectionTitle {
                font-size: 12px;
                font-weight: 700;
                color: #b9c4de;
            }
            QLabel#CharacterName {
                font-size: 13px;
                font-weight: 600;
            }
        """)

    def set_active(self, active: bool):
        if active:
            self._change_icon(QIcon(resource_path("res/images/TFon.ico")))
            # self._change_background_color("#212121")
        else:
            self._change_icon(QIcon(resource_path("res/images/TFOff.ico")))
