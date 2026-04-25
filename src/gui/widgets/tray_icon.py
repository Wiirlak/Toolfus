import sys

from PySide6.QtGui import QIcon
from PySide6.QtWidgets import QSystemTrayIcon, QMenu, QWidget

from src.engine import Engine


class TrayIcon(QSystemTrayIcon):
    def __init__(self, icon: QIcon, window: QWidget, parent=None):
        QSystemTrayIcon.__init__(self, icon, parent)
        self.window = window

        menu = QMenu(parent)
        self.top_action = menu.addAction("Fenêtre épinglée")
        self.top_action.setCheckable(True)
        self.top_action.setChecked(Engine.preferences.always_on_top)
        self.top_action.triggered.connect(self.toggle_always_on_top)

        menu.addSeparator()
        exit_ = menu.addAction("Exit")
        exit_.triggered.connect(lambda: sys.exit())

        self.setContextMenu(menu)
        self.activated.connect(self.trayicon_clicked)
        self.refresh_tooltip()

    def refresh_tooltip(self):
        pinned = "ON" if Engine.preferences.always_on_top else "OFF"
        self.setToolTip(f"Toolfus • Fenêtre épinglée: {pinned}")

    def toggle_always_on_top(self, checked: bool):
        Engine.preferences.set_always_on_top(checked)
        if hasattr(self.window, "set_always_on_top"):
            self.window.set_always_on_top(checked)
        self.refresh_tooltip()

    def trayicon_clicked(self, reason):
        if reason == self.ActivationReason.Trigger:
            print("SysTrayIcon left clicked")
            self.window.activateWindow()
