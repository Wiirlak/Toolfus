import os
from pathlib import Path
import clr

ROOT_DIR = Path(__file__).parent.parent

mainfile_path = os.path.dirname(os.path.abspath(__file__))
path = str(ROOT_DIR.joinpath("ToolfusDll.dll"))
clr.AddReference(path)
