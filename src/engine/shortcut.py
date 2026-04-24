import os
import json

import platformdirs

CONF_PATH = platformdirs.user_data_dir("Toolfus")
print(CONF_PATH)


class Shortcut:
    next_window = "²"
    __filename = "settings.json"

    def __init__(self):
        self.read_configuration()

    def read_configuration(self):
        if not os.path.exists(CONF_PATH):
            return
        try:
            config = json.load(open(os.path.join(CONF_PATH, self.__filename), "r"))
            self.next_window = config["next_window"]
        except Exception as e:
            print(e)
            self.write_configuration()

    def write_configuration(self):
        if not os.path.exists(CONF_PATH):
            os.makedirs(CONF_PATH)
        json.dump(self.get_configuration(), open(os.path.join(CONF_PATH, self.__filename), "w"))

    def set_configuration(self, shortcut: dict) -> None:
        self.next_window = shortcut["next_window"]
        self.write_configuration()

    def get_configuration(self) -> dict:
        return {"next_window": self.next_window}
