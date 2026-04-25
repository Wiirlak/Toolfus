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
                background-color: qlineargradient(
                    x1: 0, y1: 0, x2: 1, y2: 1,
                    stop: 0 #12141b,
                    stop: 1 #1f2740
                );
                color: #e8ebf2;
            }
            QWidget#TeamLayout, QWidget#SettingsLayout {
                background-color: rgba(26, 31, 46, 0.94);
                border: 1px solid #334063;
                border-radius: 12px;
            }
            QWidget#CharacterCard {
                background-color: #273149;
                border: 1px solid #3c4a6e;
                border-radius: 10px;
                padding: 6px 8px;
            }
            QWidget#CharacterCard:hover {
                background-color: #304062;
                border: 1px solid #5a76c0;
            }
            QPushButton {
                background-color: qlineargradient(
                    x1: 0, y1: 0, x2: 0, y2: 1,
                    stop: 0 #72a3ff,
                    stop: 1 #4d79ff
                );
                color: #ffffff;
                border: 1px solid #7ea8ff;
                border-radius: 8px;
                padding: 7px 11px;
                font-weight: 600;
            }
            QPushButton:hover {
                background-color: #5f88ff;
                border: 1px solid #9fc2ff;
            }
            QPushButton:pressed {
                background-color: #3f66d8;
                border: 1px solid #6b8fe0;
            }
            QLabel#SectionTitle {
                font-size: 12px;
                font-weight: 700;
                letter-spacing: 0.04em;
                color: #d4e0ff;
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
