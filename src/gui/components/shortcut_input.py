from PySide6.QtCore import Qt
from PySide6.QtGui import QKeySequence, QKeyEvent
from PySide6.QtWidgets import QLineEdit


class ShortcutInput(QLineEdit):
    _specials = (Qt.Key.Key_Control, Qt.Key.Key_Shift, Qt.Key.Key_Alt, Qt.Key.Key_Meta)

    def __init__(self, key: str):
        super().__init__(key)
        self.key = key
        self.setReadOnly(True)
        self.setPlaceholderText("Appuyez sur une touche")

    def keyPressEvent(self, event: QKeyEvent):
        key = event.key()

        if key in self._specials:
            event.accept()
            return

        shortcut = QKeySequence(event.keyCombination()).toString()
        self.setText(shortcut)
        event.accept()

    def keyReleaseEvent(self, event: QKeyEvent):
        key = event.key()

        if key in self._specials:
            event.accept()
            return
        event.accept()
