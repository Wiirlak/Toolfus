from PySide6.QtWidgets import QPushButton, QHBoxLayout
from PySide6.QtWidgets import QWidget

from src.engine.listener import enable_keyboard_listener, disable_keyboard_listener
from src.gui.widgets.shortcut import ShortcutWidget


class SettingsLayout(QWidget):
    def __init__(self):
        QWidget.__init__(self)
        self.setAutoFillBackground(True)

        layout = QHBoxLayout(self)
        btn_ok = QPushButton("Activer Multi clic")
        btn_shortcuts = QPushButton("Raccourcis")
        btn_shortcuts.clicked.connect(self.open_shortcut_window)

        # layout.addWidget(btn_ok)
        layout.addWidget(btn_shortcuts)
        self.setLayout(layout)

    def open_shortcut_window(self):
        disable_keyboard_listener()
        dialog = ShortcutWidget()
        if dialog.exec():  # Si l'utilisateur clique sur "Enregistrer"
            print("Raccourcis mis à jour !")
        enable_keyboard_listener()
