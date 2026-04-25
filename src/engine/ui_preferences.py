import json
import logging
import os

import platformdirs

CONF_PATH = platformdirs.user_data_dir("Toolfus")
SETTINGS_FILE = "settings.json"


class UiPreferences:
    def __init__(self):
        self.always_on_top = False
        self.read_configuration()

    def read_configuration(self):
        os.makedirs(CONF_PATH, exist_ok=True)
        config_path = os.path.join(CONF_PATH, SETTINGS_FILE)
        try:
            with open(config_path, "r") as file:
                config = json.load(file)
            self.always_on_top = bool(config.get("always_on_top", self.always_on_top))
        except FileNotFoundError:
            logging.info("Settings file missing, creating default settings")
            self.write_configuration()
        except (json.JSONDecodeError, OSError):
            logging.exception("Failed to read settings, restoring defaults")
            self.write_configuration()

    def write_configuration(self):
        os.makedirs(CONF_PATH, exist_ok=True)
        config_path = os.path.join(CONF_PATH, SETTINGS_FILE)
        existing: dict = {}
        try:
            with open(config_path, "r") as file:
                existing = json.load(file)
        except (FileNotFoundError, json.JSONDecodeError):
            pass
        existing.update(self.get_configuration())
        with open(config_path, "w") as file:
            json.dump(existing, file, indent=2)

    def set_always_on_top(self, value: bool):
        self.always_on_top = bool(value)
        self.write_configuration()

    def get_configuration(self) -> dict:
        return {"always_on_top": self.always_on_top}
