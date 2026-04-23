import json
import logging
import os

import platformdirs

CONF_PATH = platformdirs.user_data_dir("Toolfus")


class UiPreferences:
    __filename = "ui_settings.json"

    def __init__(self):
        self.always_on_top = False
        self.read_configuration()

    def read_configuration(self):
        if not os.path.exists(CONF_PATH):
            return
        try:
            with open(os.path.join(CONF_PATH, self.__filename), "r") as file:
                config = json.load(file)
            self.always_on_top = bool(config.get("always_on_top", self.always_on_top))
        except (FileNotFoundError, json.JSONDecodeError, OSError):
            logging.exception("Failed to read UI configuration, creating default settings")
            self.write_configuration()

    def write_configuration(self):
        if not os.path.exists(CONF_PATH):
            os.makedirs(CONF_PATH, exist_ok=True)
        with open(os.path.join(CONF_PATH, self.__filename), "w") as file:
            json.dump(self.get_configuration(), file)

    def set_always_on_top(self, value: bool):
        self.always_on_top = bool(value)
        self.write_configuration()

    def get_configuration(self) -> dict:
        return {"always_on_top": self.always_on_top}
