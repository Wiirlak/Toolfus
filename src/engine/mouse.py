from typing import Literal

from pynput import mouse
from pynput.mouse import Button

from ToolfusDll import ClickUtils, WindowUtils
from src.models.window_character import WindowCharacter

window = WindowUtils()
click = ClickUtils()

WindowButtonType = Literal["l","r","m"]


class Mouse:
    listener: mouse.Listener

    def __init__(self):
        self.listener = mouse.Listener()

    def _transform_click_type(self, button: Button) -> WindowButtonType:
        print(f"button pressed: {button}")
        if button == Button.left:
            return "l"
        if button == mouse.Button.right:
            return "r"
        if button == mouse.Button.middle:
            return "m"
        return "l"

    def on_click(self, x: int, y: int, button: Button, pressed: bool, window_character: list[WindowCharacter]):
        if pressed:
            return
        foreground = window.GetActiveProcess()
        all_pid = [c.pid for c in window_character]
        if foreground not in all_pid:
            return
        for pid in all_pid:
            if pid == foreground:
                continue
            click.ClickBackgroundWindow(pid, self._transform_click_type(button))
