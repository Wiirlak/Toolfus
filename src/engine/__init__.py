from src.engine.mouse import Mouse
from src.engine.shortcut import Shortcut
from src.engine.team import Team
from src.models.activation_changer import ActivationChanger


class Engine:
    team = Team()
    mouse = Mouse()
    shortcut = Shortcut()
    activity = ActivationChanger()