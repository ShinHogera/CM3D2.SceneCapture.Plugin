include(`Common.m4')dnl
define(`_NAME', `RimLight')dnl
_DEF(RimLight,
_PROP(color, Color, new Color(0.75f, 0.75f, 1.0f, 0.0f))dnl
_PROP(intensity, float, 1.0f)dnl
_PROP(frenelBias, float, 0.0f)dnl
_PROP(frenelScale, float, 5.0f)dnl
_PROP(frenelPow, float, 5.0f)dnl
_PROP(edgeHighlighting, bool, true)dnl
_PROP(edgeIntensity, float, 0.3f)dnl
_PROP(edgeThreshold, float, 0.8f)dnl
_PROP(edgeRadius, float, 1.0f)dnl
_PROP(mulSmoothness, bool, true)dnl
)dnl
