# Toolfus

Toolfus is a desktop companion for managing multiple Dofus game windows on Windows.  
It provides a lightweight interface to detect characters and switch quickly between windows with configurable shortcuts.  
It can also replicate mouse clicks to background game windows when needed.

## Main capabilities

- Team detection from running Dofus processes
- Fast window switching with a global hotkey
- Optional mirrored mouse clicks to other team windows
- Tray integration and quick activation state feedback
- Persistent local shortcut configuration

## Build the executable with PyInstaller

### Prerequisites

- Windows environment
- Python 3.12+ recommended
- [uv](https://docs.astral.sh/uv/) installed
- `ToolfusDll.dll` present at the project root

### Build steps

1. Open a terminal in the project root directory (the folder containing `main.py`).
2. Install dependencies:
   ```
   uv sync
   uv add --dev pyinstaller
   ```
3. Build with the spec file:
   ```
   uv run pyinstaller Toolfus.spec
   ```
4. If the spec file paths are not valid on your machine, build from `main.py` with equivalent data files and icon settings.
5. Retrieve the generated executable from the `dist/Toolfus/` output directory.

## Development

### Prerequisites

- [uv](https://docs.astral.sh/uv/) installed

### Setup

1. Install all dependencies including dev tools:
   ```
   uv sync --group dev
   ```
2. Install the pre-commit hooks:
   ```
   uv run pre-commit install
   ```

The hooks run automatically on each commit. To run them manually against all files:
```
uv run pre-commit run --all-files
```

The pre-commit configuration includes:
- **ruff** – linting
- **ty** – static type checking
