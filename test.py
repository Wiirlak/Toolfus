import os
import time
import psutil

import clr

mainfile_path = os.path.dirname(os.path.abspath(__file__))
clr.AddReference(os.path.join(mainfile_path, "ToolfusDll.dll"))

from ToolfusDll import WindowUtils

window = WindowUtils()


def toolfus():
    window.ForceForegroundWindow(22432)


def find_dofus_processes() -> list:
    return [p for p in psutil.process_iter() if p.name() == "Dofus.exe"]


def find_team(pids: list[int]) -> dict:
    team = {}
    for pid in pids:
        name = " - ".join(window.GetWindowName(pid).split(" - ")[:2])
        team[pid] = name
    return team


def test_all_window(pids: list[int]) -> None:
    for pid in pids:
        window.ForceForegroundWindow(pid)
        time.sleep(0.3)


if __name__ == "__main__":
    # toolfus()
    dofus = find_dofus_processes()
    print(find_team([p.pid for p in dofus]))
    test_all_window([p.pid for p in dofus])

