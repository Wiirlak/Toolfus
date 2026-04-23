from PySide6.QtGui import QMouseEvent
from PySide6.QtWidgets import QVBoxLayout
from PySide6.QtWidgets import QWidget

from src.engine.listener import stop_mouse_listener
from src.gui.components.character import CharacterLayout
from src.engine import Engine
from src.models.window_character import WindowCharacter


class TeamLayout(QWidget):
    team: list[WindowCharacter] = []
    mapped_team = {}

    def __init__(self):
        QWidget.__init__(self)
        self.setAutoFillBackground(True)
        self.layout = QVBoxLayout(self)
        self.update_team()
        self.setLayout(self.layout)

    def refresh(self):
        new_team = Engine.team.scan_team()
        if new_team == self.team:
            return
        if len(new_team) == 0:
            return
        print("Refreshing team")
        print(new_team)
        self.update_team()
        print(self.mapped_team)
        self.team = new_team
        stop_mouse_listener()

    def update_team(self):
        self.team = Engine.team.scan_team()
        Engine.team.selected_character = Engine.team.window_character
        self.clear_layout()
        self.mapped_team = {
            character.name: CharacterLayout("red", character.name, character.icon)
            for character in self.team
        }
        [self.layout.addWidget(layout) for layout in self.mapped_team.values()]

    def clear_layout(self):
        for i in reversed(range(self.layout.count())):
            widget = self.layout.itemAt(i).widget()
            if widget is not None:
                widget.setParent(None)
                widget.deleteLater()
        self.layout.update()
