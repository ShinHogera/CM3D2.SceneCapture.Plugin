include(`Common.m4')dnl
define(`_NAME', `AnalogTV')dnl
_PANE(AnalogTV,
_CHECKBOX(automaticPhase)
_SLIDER(phase, float, 0f, 180f, 2)
_CHECKBOX(convertToGrayscale)
_SLIDER(noiseIntensity, float, 0f, 1f, 4)
_SLIDER(scanlinesIntensity, float, 0f, 2f, 4)
_SLIDER(scanlinesCount, int, 0f, 4096f, 0)
_SLIDER(scanlinesOffset, float, 0f, 300f, 2)
_CHECKBOX(verticalScanlines)
_SLIDER(distortion, float, -2f, 2f, 4)
_SLIDER(cubicDistortion, float, -2f, 2f, 4)
_SLIDER(scale, float, 0.01f, 2f, 4)dnl
)dnl
