from PySide6.QtWidgets import QPushButton, QHBoxLayout, QLabel, QVBoxLayout
from PySide6.QtWidgets import QWidget

from src.engine.listener import enable_keyboard_listener, disable_keyboard_listener
from src.gui.widgets.shortcut import ShortcutWidget


class SettingsLayout(QWidget):
    def __init__(self):
        QWidget.__init__(self)
        self.setObjectName("SettingsLayout")
        self.setAutoFillBackground(True)

        layout = QVBoxLayout(self)
        layout.setContentsMargins(10, 10, 10, 10)
        layout.setSpacing(8)

        title = QLabel("Paramètres")
        title.setObjectName("SectionTitle")
        layout.addWidget(title)

        row = QHBoxLayout()
        row.setContentsMargins(0, 0, 0, 0)
        row.setSpacing(6)
        btn_shortcuts = QPushButton("Settings")
        btn_shortcuts.clicked.connect(self.open_shortcut_window)

        row.addWidget(btn_shortcuts)
        layout.addLayout(row)
        self.setLayout(layout)

    def open_shortcut_window(self):
        disable_keyboard_listener()
        dialog = ShortcutWidget()
        if dialog.exec():
            pass
        enable_keyboard_listener()
