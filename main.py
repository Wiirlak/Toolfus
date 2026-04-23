import sys
import time

from PySide6.QtCore import Qt
from PySide6.QtGui import QIcon
from PySide6.QtWidgets import QApplication

from src.engine.listener import enable_keyboard_listener, disable_keyboard_listener, set_mainframe
from src.engine.utils import resource_path
from src.gui.main_window import MainWidget
from src.gui.widgets.tray_icon import TrayIcon


def run() -> int:
    WIDTH = 200
    HEIGHT = 300

    app = QApplication(sys.argv)
    widget = MainWidget()
    widget.resize(WIDTH, HEIGHT)

    widget.setWindowTitle("Toolfus")
    widget.setWindowIcon(QIcon(resource_path("res/images/TFoff.ico")))
    # widget.setStyleSheet("background-color: #171717")

    tray_icon = TrayIcon(QIcon(resource_path("res/images/TFoff.ico")), widget)
    widget.setWindowFlag(Qt.WindowType.WindowStaysOnTopHint)
    set_mainframe(widget, tray_icon)

    widget.show()
    tray_icon.show()
    return app.exec()


if __name__ == "__main__":
    enable_keyboard_listener()
    try:
        main = run()
    except Exception as e:
        crash = ["Error on line {}".format(sys.exc_info()[-1].tb_lineno), "\n", e]
        print(crash)
        timeX = str(time.time())
        with open("./CRASH-" + timeX + ".txt", "w") as crashLog:
            for i in crash:
                i = str(i)
                crashLog.write(i)
    disable_keyboard_listener()
    sys.exit(main)
