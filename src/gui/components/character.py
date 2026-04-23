from PySide6.QtGui import QColor, QIcon, QMouseEvent
from PySide6.QtWidgets import QHBoxLayout, QLabel, QCheckBox
from PySide6.QtWidgets import QWidget
from src.models.job_icon import JobIcon


class CharacterLayout(QWidget):
    def __init__(self, color: str, name: str, icon: str):
        QWidget.__init__(self)
        self.setAutoFillBackground(True)

        layout = QHBoxLayout(self)

        self.character_name = QLabel(name)
        # self.checkbox = QCheckBox()
        self.character_icon = JobIcon.from_str(icon).icon
        self.is_checked = False

        # layout.addWidget(self.checkbox)
        layout.addWidget(self.character_icon)
        layout.addStretch()
        layout.addWidget(self.character_name)

        self.palette_base = self.palette()
        # self.palette_base.setColor(self.backgroundRole(), QColor(color))
        self.setPalette(self.palette_base)

    # def mousePressEvent(self, event: QMouseEvent):
    #     self.checkbox.setChecked(not self.checkbox.isChecked())
    #     self.is_checked = self.checkbox.isChecked()
