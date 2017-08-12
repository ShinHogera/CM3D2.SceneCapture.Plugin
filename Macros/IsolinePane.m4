include(`Common.m4')dnl
define(`_NAME', `Isoline')dnl
_VEC(_axis)dnl
_VEC(_offset)dnl
_VEC(_modulationAxis)dnl
_VEC(_direction)dnl
_PANE(Isoline,
_COLORPICKER(_lineColor)
_SLIDER(_luminanceBlending, float, 0f, 1f, 4)
_SLIDER(_fallOffDepth, float, 0f, 500f, 2)
_COLORPICKER(_backgroundColor)
_VECSLIDER(_axis, x, 0f, 100f, 4)
_VECSLIDER(_axis, y, 0f, 100f, 4)
_VECSLIDER(_axis, z, 0f, 100f, 4)
_SLIDER(_interval, float, 0f, 100f, 4)
_VECSLIDER(_offset, x, 0f, 100f, 4)
_VECSLIDER(_offset, y, 0f, 100f, 4)
_VECSLIDER(_offset, z, 0f, 100f, 4)
_SLIDER(_distortionFrequency, float, 0f, 100f, 4)
_SLIDER(_distortionAmount, float, 0f, 100f, 4)
_COMBOBOX(_modulationMode, ISOLINE_MODULATION_MODES, Isoline.ModulationMode)
_VECSLIDER(_modulationAxis, x, 0f, 100f, 4)
_VECSLIDER(_modulationAxis, y, 0f, 100f, 4)
_VECSLIDER(_modulationAxis, z, 0f, 100f, 4)
_SLIDER(_modulationFrequency, float, 0f, 100f, 4)
_SLIDER(_modulationSpeed, float, 0f, 100f, 4)
_SLIDER(_modulationExponent, float, 0f, 50f, 4)
_VECSLIDER(_direction, x, 0f, 100f, 4)
_VECSLIDER(_direction, y, 0f, 100f, 4)
_VECSLIDER(_direction, z, 0f, 100f, 4)
_SLIDER(_speed, float, 0f, 100f, 4)dnl
)dnl
