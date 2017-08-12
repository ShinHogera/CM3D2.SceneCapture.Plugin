include(`Common.m4')dnl
define(`_NAME', `AnalogTV')dnl
_DEF(AnalogTV,
_PROP(automaticPhase, bool, true)dnl
_PROP(phase, float, 0.5f)dnl
_PROP(convertToGrayscale, bool, false)dnl
_PROP(noiseIntensity, float, 0.5f)dnl
_PROP(scanlinesIntensity, float, 2f)dnl
_PROP(scanlinesCount, int, 768)dnl
_PROP(scanlinesOffset, float, 0f)dnl
_PROP(verticalScanlines, bool, false)dnl
_PROP(distortion, float, 0.2f)dnl
_PROP(cubicDistortion, float, 0.6f)dnl
_PROP(scale, float, 0.8f)dnl
)dnl
