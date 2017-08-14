include(`Common.m4')dnl
define(`_NAME', `Raymarcher')dnl
_PANE(Raymarcher,
_CHECKBOX(screenSpace)
_CHECKBOX(enableAdaptive)
_CHECKBOX(enableTemporal)
_CHECKBOX(enableGlowline)
_CHECKBOX(dbgShowSteps)
_SLIDER(scene, int, 0f, 5f, 0)
_COLORPICKER(fogColor)dnl
)dnl
