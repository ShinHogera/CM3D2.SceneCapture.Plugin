include(`Common.m4')dnl
define(`_NAME', `ZFog')dnl
_PANE(ZFog,
_COLORPICKER(color1)
_SLIDER(hdr1, float, 0f, 50f, 2)
_SLIDER(near1, float, 0f, 50f, 2)
_SLIDER(far1, float, 0f, 50f, 2)
_SLIDER(pow1, float, 0f, 50f, 2)
_COLORPICKER(color2)
_SLIDER(hdr2, float, 0f, 50f, 2)
_SLIDER(near2, float, 0f, 50f, 2)
_SLIDER(far2, float, 0f, 50f, 2)
_SLIDER(pow2, float, 0f, 50f, 2)dnl
)dnl
