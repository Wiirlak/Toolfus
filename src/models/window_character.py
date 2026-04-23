from __future__ import annotations
from dataclasses import dataclass


@dataclass
class WindowCharacter:
    pid: int
    name: str
    icon: str

    def __str__(self):
        return f"{self.name} - {self.icon} ({self.pid})"