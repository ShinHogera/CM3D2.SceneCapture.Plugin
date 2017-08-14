include(`Common.m4')dnl
define(`_NAME', `RimLight')dnl
_PANE(RimLight,
_COLORPICKER(color)
_SLIDER(intensity, float, 0f, 10f, 4)
_SLIDER(fresnelBias, float, 0f, 10f, 4)
_SLIDER(fresnelScale, float, 0f, 10f, 4)
_SLIDER(fresnelPow, float, 0f, 10f, 4)
_CHECKBOX(edgeHighlighting)
_SLIDER(edgeIntensity, float, 0f, 10f, 4)
_SLIDER(edgeThreshold, float, 0.0f, 9.9f, 4)
_SLIDER(edgeRadius, float, 0.0f, 10f, 4)
_CHECKBOX(mulSmoothness)dnl
)dnl
