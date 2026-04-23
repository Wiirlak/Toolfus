from __future__ import annotations

import os
from dataclasses import dataclass
from enum import Enum

from PySide6.QtGui import QIcon
from PySide6.QtWidgets import QLabel

from src.engine.utils import resource_path


class Job(Enum):
    CRA = 1
    ECAFLIP = 2
    ELIOTROPE = 3
    ENIRIPSA = 4
    ENUTROF = 5
    FECA = 6
    HUPPERMAGE = 7
    IOP = 8
    OSAMODAS = 9
    OUGINAK = 10
    PANDAWA = 11
    ROUBLARD = 12
    SACRIEUR = 13
    SADIDA = 14
    SRAM = 15
    STEAMER = 16
    XELOR = 17
    ZOBAL = 18


@dataclass
class JobIcon:
    job: Job
    icon: QLabel


    @classmethod
    def from_str(cls, job: str) -> JobIcon:
        try:
            job = Job[job.upper()]
            file = resource_path(f"res/images/class/{job.name.lower()}.png")
        except KeyError:
            file = resource_path("res/images/TFon.ico")
        if not os.path.isfile(file):
            file = resource_path("res/images/TFon.ico")
        icon = QIcon(file)
        character_icon = QLabel("Icon")
        character_icon.setPixmap(icon.pixmap(40, 40))
        return cls(job, character_icon)
