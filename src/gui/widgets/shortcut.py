import logging

from PySide6.QtCore import Qt
from PySide6.QtWidgets import (
    QCheckBox,
    QDialog,
    QFrame,
    QHBoxLayout,
    QLabel,
    QPushButton,
    QVBoxLayout,
    QWidget,
)

from src.engine import Engine
from src.gui.components.shortcut_input import ShortcutInput

_LABEL_MIN_WIDTH = 120

_DIALOG_STYLESHEET = """
QDialog {
    background-color: #12141b;
    color: #e8ebf2;
}
QWidget#Section {
    background-color: rgba(26, 31, 46, 0.94);
    border: 1px solid #334063;
    border-radius: 12px;
}
QLabel#SectionTitle {
    font-size: 11px;
    font-weight: 700;
    letter-spacing: 0.08em;
    color: #7ea8ff;
    text-transform: uppercase;
}
QLabel#RowLabel {
    font-size: 13px;
    color: #c8d3f0;
}
QCheckBox {
    spacing: 6px;
    color: #c8d3f0;
    font-size: 13px;
}
QCheckBox::indicator {
    width: 18px;
    height: 18px;
    border-radius: 5px;
    border: 2px solid #4d79ff;
    background-color: #1a1f2e;
}
QCheckBox::indicator:checked {
    background-color: #4d79ff;
    border: 2px solid #72a3ff;
    image: none;
}
QCheckBox::indicator:hover {
    border: 2px solid #72a3ff;
}
QLineEdit {
    background-color: #1a1f2e;
    color: #e8ebf2;
    border: 1px solid #334063;
    border-radius: 7px;
    padding: 5px 9px;
    font-size: 13px;
    selection-background-color: #4d79ff;
}
QLineEdit:focus {
    border: 1px solid #4d79ff;
    background-color: #1e2438;
}
QPushButton#SaveBtn {
    background-color: qlineargradient(
        x1: 0, y1: 0, x2: 0, y2: 1,
        stop: 0 #72a3ff,
        stop: 1 #4d79ff
    );
    color: #ffffff;
    border: 1px solid #7ea8ff;
    border-radius: 9px;
    padding: 9px 0px;
    font-size: 13px;
    font-weight: 700;
}
QPushButton#SaveBtn:hover {
    background-color: #5f88ff;
    border: 1px solid #9fc2ff;
}
QPushButton#SaveBtn:pressed {
    background-color: #3f66d8;
    border: 1px solid #6b8fe0;
}
QFrame#Divider {
    color: #2a3150;
    background-color: #2a3150;
}
"""


def _section(title: str, inner: QWidget) -> QWidget:
    """Wrap *inner* in a rounded card with a section title."""
    container = QWidget()
    container.setObjectName("Section")
    vbox = QVBoxLayout(container)
    vbox.setContentsMargins(14, 10, 14, 14)
    vbox.setSpacing(10)

    lbl = QLabel(title)
    lbl.setObjectName("SectionTitle")
    vbox.addWidget(lbl)

    divider = QFrame()
    divider.setObjectName("Divider")
    divider.setFrameShape(QFrame.Shape.HLine)
    divider.setFixedHeight(1)
    vbox.addWidget(divider)

    vbox.addWidget(inner)
    return container


class ShortcutWidget(QDialog):
    def __init__(self):
        QDialog.__init__(self)
        self.setWindowTitle("Settings")
        self.setFixedSize(460, 300)
        self.setStyleSheet(_DIALOG_STYLESHEET)

        root = QVBoxLayout(self)
        root.setContentsMargins(16, 16, 16, 16)
        root.setSpacing(12)

        # ── General section ─────────────────────────────────────────────────
        general_body = QWidget()
        gen_layout = QVBoxLayout(general_body)
        gen_layout.setContentsMargins(0, 0, 0, 0)
        gen_layout.setSpacing(0)

        aot_row = QHBoxLayout()
        aot_row.setContentsMargins(0, 0, 0, 0)
        aot_label = QLabel("Always on top")
        aot_label.setObjectName("RowLabel")
        self.aot_checkbox = QCheckBox()
        self.aot_checkbox.setChecked(Engine.preferences.always_on_top)
        aot_row.addWidget(aot_label)
        aot_row.addStretch()
        aot_row.addWidget(self.aot_checkbox)
        gen_layout.addLayout(aot_row)

        root.addWidget(_section("General", general_body))

        # ── Keyboard shortcuts section ───────────────────────────────────────
        shortcuts_body = QWidget()
        sc_layout = QVBoxLayout(shortcuts_body)
        sc_layout.setContentsMargins(0, 0, 0, 0)
        sc_layout.setSpacing(8)

        shortcuts = Engine.shortcut.get_configuration()
        self.inputs: dict[str, ShortcutInput] = {}
        for key, value in shortcuts.items():
            row = QHBoxLayout()
            row.setContentsMargins(0, 0, 0, 0)
            lbl = QLabel(key)
            lbl.setObjectName("RowLabel")
            lbl.setMinimumWidth(_LABEL_MIN_WIDTH)
            inp = ShortcutInput(value)
            inp.setFixedHeight(32)
            self.inputs[key] = inp
            row.addWidget(lbl)
            row.addWidget(inp)
            sc_layout.addLayout(row)

        root.addWidget(_section("Keyboard shortcuts", shortcuts_body))

        root.addStretch()

        # ── Save button ──────────────────────────────────────────────────────
        btn_save = QPushButton("Save")
        btn_save.setObjectName("SaveBtn")
        btn_save.setFixedHeight(40)
        btn_save.setCursor(Qt.CursorShape.PointingHandCursor)
        btn_save.clicked.connect(self.save_settings)
        root.addWidget(btn_save)

    def save_settings(self):
        new_aot = self.aot_checkbox.isChecked()
        if new_aot != Engine.preferences.always_on_top:
            Engine.preferences.set_always_on_top(new_aot)
            if hasattr(Engine.activity, "main_window"):
                try:
                    Engine.activity.main_window.set_always_on_top(new_aot)
                except (ValueError, AttributeError):
                    logging.debug("main_window not yet available for always-on-top update")

        new_shortcuts = {action: field.text() for action, field in self.inputs.items()}
        Engine.shortcut.set_configuration(new_shortcuts)
        self.accept()