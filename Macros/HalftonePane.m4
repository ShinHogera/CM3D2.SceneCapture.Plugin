include(`Common.m4')dnl
define(`_NAME', `Halftone')dnl
_VEC(center)dnl
_PANE(Halftone,
_SLIDER(scale, float, 0f, 100f, 2)
_SLIDER(dotSize, float, 0f, 100f, 2)
_SLIDER(angle, float, 0f, 180f, 2)
_SLIDER(smoothness, float, 0f, 1, 4)
_VECSLIDER(center, x, 0f, 1f, 4)
_VECSLIDER(center, y, 0f, 1f, 4)
_CHECKBOX(desaturate)dnl
)dnl
