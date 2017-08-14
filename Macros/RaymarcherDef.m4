include(`Common.m4')dnl
define(`_NAME', `Raymarcher')dnl
_DEF(Raymarcher,
_PROP(screenSpace, bool, true)dnl
_PROP(enableAdaptive, bool, true)dnl
_PROP(enableTemporal, bool, true)dnl
_PROP(dbgShowSteps, bool, false)dnl
_PROP(scene, int, 0)dnl
_PROP(fogColor, Color, new Color(0.16f, 0.13f, 0.20f))dnl
)dnl
