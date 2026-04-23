import logging
from typing import Literal

from pynput import mouse
from pynput.mouse import Button

from ToolfusDll import ClickUtils, WindowUtils
from src.models.window_character import WindowCharacter

logger = logging.getLogger(__name__)

window = WindowUtils()
click = ClickUtils()

WindowButtonType = Literal["l","r","m"]


class Mouse:
    listener: mouse.Listener

    def __init__(self):
        self.listener = mouse.Listener()
        self._pressed_foreground_pid: int = 0
        self._pressed_x: int = 0
        self._pressed_y: int = 0

    def _transform_click_type(self, button: Button) -> WindowButtonType:
        if button == Button.left:
            return "l"
        if button == mouse.Button.right:
            return "r"
        if button == mouse.Button.middle:
            return "m"
        return "l"

    def on_click(self, x: int, y: int, button: Button, pressed: bool, window_character: list[WindowCharacter]):
        all_pid = [c.pid for c in window_character]
        if pressed:
            foreground = window.GetActiveProcess()
            self._pressed_foreground_pid = foreground if foreground in all_pid else 0
            self._pressed_x = x
            self._pressed_y = y
            logger.debug(f"Mouse press at ({x},{y}) button={button} foreground_pid={self._pressed_foreground_pid}")
            return
        foreground = self._pressed_foreground_pid
        if foreground == 0:
            return
        click_type = self._transform_click_type(button)
        logger.info(f"Dispatching click ({x},{y}) button={click_type} from pid={foreground} to {[p for p in all_pid if p != foreground]}")
        for pid in all_pid:
            if pid == foreground:
                continue
            logger.debug(f"  -> sending click to pid={pid}")
            click.ClickBackgroundWindowPosition(pid, x, y, click_type)
