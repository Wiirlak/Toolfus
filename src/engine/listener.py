import keyboard

from src.engine import Engine


def set_mainframe(mainframe, tray_icon):
    Engine.activity.setup(mainframe, tray_icon)


def enable_keyboard_listener():
    shortcut = Engine.shortcut
    keyboard.add_hotkey(shortcut.next_window, Engine.team.next_window, suppress=True, timeout=0.5)


def disable_keyboard_listener():
    shortcut = Engine.shortcut
    print(f"hotkey active list: {keyboard._hotkeys}")
    keyboard.remove_hotkey(shortcut.next_window)
