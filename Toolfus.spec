# -*- mode: python ; coding: utf-8 -*-


a = Analysis(
    ['D:\\ESGI\\Python\\Toolfus\\main.py'],
    pathex=[],
    binaries=[],
    datas=[('D:\\ESGI\\Python\\Toolfus\\res', 'res/'), ('D:\\ESGI\\Python\\Toolfus\\ToolfusDll.dll', '.'), ('D:\\ESGI\\Python\\Toolfus\\src', 'src/'), ('D:\\ESGI\\Python\\Toolfus\\ToolfusDll.pyi', '.')],
    hiddenimports=[],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    noarchive=False,
    optimize=0,
)
pyz = PYZ(a.pure)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.datas,
    [],
    name='Toolfus',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=False,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
    icon=['D:\\ESGI\\Python\\Toolfus\\res\\images\\TFon.ico'],
)
