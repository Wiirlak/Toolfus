import sys

from PySide6.QtGui import QIcon
from PySide6.QtWidgets import QSystemTrayIcon, QMenu, QWidget


class TrayIcon(QSystemTrayIcon):
    def __init__(self, icon: QIcon, window: QWidget, parent=None):
        QSystemTrayIcon.__init__(self, icon, parent)
        self.setToolTip("Toolfus")
        self.window = window

        menu = QMenu(parent)
        exit_ = menu.addAction("Exit")
        exit_.triggered.connect(lambda: sys.exit())

        self.setContextMenu(menu)
        self.activated.connect(self.trayicon_clicked)

    def trayicon_clicked(self, reason):
        if reason == self.ActivationReason.Trigger:
            print("SysTrayIcon left clicked")
            self.window.activateWindow()
