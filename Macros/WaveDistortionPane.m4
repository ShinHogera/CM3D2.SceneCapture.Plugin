include(`Common.m4')dnl
define(`_NAME', `WaveDistortion')dnl
_PANE(WaveDistortion,
_SLIDER(amplitude, float, 0f, 1f, 4)
_SLIDER(waves, float, 0f, 100f, 2)
_SLIDER(colorGlitch, float, 0f, 5f, 4)
_SLIDER(phase, float, 0f, 180f, 2)dnl
)dnl
