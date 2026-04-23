import keyboard
from pynput import mouse

from src.engine import Engine


def set_mainframe(mainframe, tray_icon):
    Engine.activity.setup(mainframe, tray_icon)


def enable_keyboard_listener():
    shortcut = Engine.shortcut
    keyboard.add_hotkey(shortcut.next_window, Engine.team.next_window, suppress=True, timeout=0.5)
    keyboard.add_hotkey(shortcut.toggle_mouse_listener, toggle_mouse_listener, suppress=True, timeout=0.5)


def disable_keyboard_listener():
    shortcut = Engine.shortcut
    print(f"hotkey active list: {keyboard._hotkeys}")
    keyboard.remove_hotkey(shortcut.next_window)
    keyboard.remove_hotkey(shortcut.toggle_mouse_listener)


def start_mouse_listener():
    config = Engine.mouse
    config.listener = mouse.Listener(on_click=lambda *args: config.on_click(*args, Engine.team.window_character))
    config.listener.start()
    Engine.activity.set_active(True)


def stop_mouse_listener():
    config = Engine.mouse
    config.listener.stop()
    Engine.activity.set_active(False)


def toggle_mouse_listener():
    print("Toggle mouse listener")
    config = Engine.mouse
    if config.listener.running:
        stop_mouse_listener()
    else:
        start_mouse_listener()
