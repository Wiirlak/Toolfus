import json
import os

import platformdirs

CONF_PATH = platformdirs.user_data_dir("Toolfus")


class UiPreferences:
    always_on_top = False
    __filename = "ui_settings.json"

    def __init__(self):
        self.read_configuration()

    def read_configuration(self):
        if not os.path.exists(CONF_PATH):
            return
        try:
            config = json.load(open(os.path.join(CONF_PATH, self.__filename), "r"))
            self.always_on_top = bool(config.get("always_on_top", self.always_on_top))
        except Exception as e:
            print(e)
            self.write_configuration()

    def write_configuration(self):
        if not os.path.exists(CONF_PATH):
            os.makedirs(CONF_PATH)
        json.dump(self.get_configuration(), open(os.path.join(CONF_PATH, self.__filename), "w"))

    def set_always_on_top(self, value: bool):
        self.always_on_top = bool(value)
        self.write_configuration()

    def get_configuration(self) -> dict:
        return {"always_on_top": self.always_on_top}
