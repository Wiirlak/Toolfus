from PySide6.QtWidgets import QVBoxLayout, QPushButton, QHBoxLayout, QDialog, QLabel, QLineEdit

from src.engine import Engine
from src.gui.components.shortcut_input import ShortcutInput


class ShortcutWidget(QDialog):
    def __init__(self):
        QDialog.__init__(self)
        self.setWindowTitle("Raccourcis")
        self.setGeometry(100, 100, 400, 200)

        layout = QVBoxLayout()

        shortcuts = Engine.shortcut.get_configuration()
        self.inputs = {}
        for key, value in shortcuts.items():
            row = QHBoxLayout()
            label = QLabel(key)
            button = ShortcutInput(value)
            self.inputs[key] = button
            row.addWidget(label)
            row.addWidget(button)
            layout.addLayout(row)

        btn_save = QPushButton("Enregistrer")
        btn_save.clicked.connect(self.save_shortcuts)
        layout.addWidget(btn_save)

        self.setLayout(layout)

    def save_shortcuts(self):
        new_shortcuts = {action: field.text() for action, field in self.inputs.items()}
        Engine.shortcut.set_configuration(new_shortcuts)
        self.accept()