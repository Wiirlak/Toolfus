from src.engine.shortcut import Shortcut
from src.engine.team import Team
from src.engine.ui_preferences import UiPreferences
from src.models.activation_changer import ActivationChanger


class Engine:
    team = Team()
    shortcut = Shortcut()
    preferences = UiPreferences()
    activity = ActivationChanger()
