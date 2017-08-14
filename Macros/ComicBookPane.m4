include(`Common.m4')dnl
define(`_NAME', `ComicBook')dnl
_VEC2(stripLimits)dnl
_PANE(ComicBook,
_SLIDER(stripAngle, float, 0f, 180f, 2)
_SLIDER(stripDensity, float, 0f, 100f, 2)
_SLIDER(stripThickness, float, 0f, 1f, 4)
_VECSLIDER(stripLimits, x, 0f, 10f, 4)
_VECSLIDER(stripLimits, y, 0f, 10f, 4)
_COLORPICKER(stripInnerColor)
_COLORPICKER(stripOuterColor)
_COLORPICKER(fillColor)
_COLORPICKER(backgroundColor)
_CHECKBOX(edgeDetection)
_SLIDER(edgeThreshold, float, 0.01f, 50f, 3)
_COLORPICKER(edgeColor)
_SLIDER(amount, float, 0f, 1f, 4)dnl
)dnl
