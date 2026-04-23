import psutil
from PySide6.QtGui import QKeyEvent, Qt
from keyboard import release

from ToolfusDll import WindowUtils
from src.models.window_character import WindowCharacter

window = WindowUtils()


class Team:
    window_character: list[WindowCharacter]
    selected_character: list[WindowCharacter]
    _current_character: int = 0

    def __init__(self, window_character: list[WindowCharacter] = None):
        if window_character is None:
            self.window_character = self.scan_team()
            self.selected_character = self.window_character
        else:
            self.window_character = window_character

    def scan_team(self) -> list[WindowCharacter]:
        team = []
        processes = [p for p in psutil.process_iter() if p.name() == "Dofus.exe"]
        i = 0
        for process in processes:
            window_name = window.GetWindowName(process.pid).split(" - ")
            if len(window_name) < 3:  # Not logged in yet
                team.append(WindowCharacter(process.pid, f"Character {i}", "Dofus"))
                i += 1
            else:
                team.append(WindowCharacter(process.pid, window_name[0], window_name[1]))
        self.window_character = team
        return team

    def switch_window(self, character: WindowCharacter):
        window.SwitchToWindow(character.pid)

    def next_window(self):
        try:
            print("Next window")
            print(self.selected_character)
            if self._current_character + 1 >= len(self.selected_character):
                self._current_character = 0
            else:
                self._current_character += 1
            window.ForceForegroundWindow(self.selected_character[self._current_character].pid)
            release('alt')
        except Exception as e:
            print(e)



