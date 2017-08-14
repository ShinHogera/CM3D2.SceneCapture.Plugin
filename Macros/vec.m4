include(`Common.m4')dnl
define(`_NAME', `Isoline')dnl
_VEC(axis)dnl
_PANE(Isoline,
_COLORPICKER(lineColor)
_SLIDER(luminanceBlending, float, 0f, 1f, 4)
_SLIDER(fallOffDepth, float, 0f, 1f, 4)
_COLORPICKER(backgroundColor)
_VECSLIDER(axis, x, 0f, 180f, 4)
_VECSLIDER(axis, y, 0f, 180f, 4)
_VECSLIDER(axis, z, 0f, 180f, 4)
)dnl
