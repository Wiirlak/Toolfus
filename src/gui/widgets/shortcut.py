from PySide6.QtWidgets import (
    QVBoxLayout,
    QPushButton,
    QHBoxLayout,
    QDialog,
    QLabel,
    QCheckBox,
)
import logging

from src.engine import Engine
from src.gui.components.shortcut_input import ShortcutInput


class ShortcutWidget(QDialog):
    def __init__(self):
        QDialog.__init__(self)
        self.setWindowTitle("Settings")
        self.setGeometry(100, 100, 420, 240)

        layout = QVBoxLayout()
        layout.setSpacing(10)

        # Always on top toggle
        aot_row = QHBoxLayout()
        aot_label = QLabel("Always on top")
        self.aot_checkbox = QCheckBox()
        self.aot_checkbox.setChecked(Engine.preferences.always_on_top)
        aot_row.addWidget(aot_label)
        aot_row.addStretch()
        aot_row.addWidget(self.aot_checkbox)
        layout.addLayout(aot_row)

        # Separator label
        shortcuts_label = QLabel("Keyboard shortcuts")
        shortcuts_label.setObjectName("SectionTitle")
        layout.addWidget(shortcuts_label)

        # Shortcut key bindings
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

        btn_save = QPushButton("Save")
        btn_save.clicked.connect(self.save_settings)
        layout.addWidget(btn_save)

        self.setLayout(layout)

    def save_settings(self):
        # Save always-on-top preference
        new_aot = self.aot_checkbox.isChecked()
        if new_aot != Engine.preferences.always_on_top:
            Engine.preferences.set_always_on_top(new_aot)
            if hasattr(Engine.activity, "main_window"):
                try:
                    Engine.activity.main_window.set_always_on_top(new_aot)
                except (ValueError, AttributeError):
                    logging.debug("main_window not yet available for always-on-top update")

        # Save shortcuts
        new_shortcuts = {action: field.text() for action, field in self.inputs.items()}
        Engine.shortcut.set_configuration(new_shortcuts)
        self.accept()