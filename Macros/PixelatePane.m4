include(`Common.m4')dnl
define(`_NAME', `Pixelate')dnl
_PANE(Pixelate,
_SLIDER(scale, float, 1f, 1024f, 2)
_CHECKBOX(automaticRatio)
_SLIDER(ratio, float, 0f, 100f, 2)
_COMBOBOX(mode, PIXELATE_SIZEMODES, Pixelate.SizeMode)dnl
)dnl
