# Toolfus

Toolfus is a desktop companion for managing multiple Dofus game windows on Windows.  
It provides a lightweight interface to detect characters, switch quickly between windows with configurable shortcuts, and replicate mouse clicks to background game windows when needed.

## Main capabilities

- Team detection from running Dofus processes
- Fast window switching with a global hotkey
- Optional mirrored mouse clicks to other team windows
- Tray integration and quick activation state feedback
- Persistent local shortcut configuration

## Build the executable with PyInstaller

### Prerequisites

- Windows environment
- Python 3.10+ recommended
- `ToolfusDll.dll` present at the project root
- Project dependencies installed (`PySide6`, `keyboard`, `pynput`, `psutil`, `platformdirs`, `pythonnet`, `pyinstaller`)

### Build steps

1. Open a terminal in the project root directory (the folder containing `main.py`).
2. Install dependencies: `pip install PySide6 keyboard pynput psutil platformdirs pythonnet pyinstaller`.
3. Build with the spec file:
   - `pyinstaller Toolfus.spec`
4. If the spec file paths are not valid on your machine, build from `main.py` with equivalent data files and icon settings.
5. Retrieve the generated executable from the `dist/Toolfus/` output directory.
