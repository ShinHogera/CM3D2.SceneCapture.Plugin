include(`Common.m4')dnl
define(`_NAME', `TemporalSSAO')dnl
_ENUM(TSSAO_SAMPLE_COUNTS, "Low" ````,'''' "Medium" ````,'''' "High")dnl
_PANE(TemporalSSAO,
_COMBOBOX(sampleCount, TSSAO_SAMPLE_COUNTS)
_SLIDER(downsampling, int, 1f, 8f, 0)
_SLIDER(radius, float, 0.0f, 5.0f, 4)
_SLIDER(intensity, float, 0.0f, 8.0f, 4)
_SLIDER(blurSize, float, 0.0f, 8.0f, 4)
_CHECKBOX(dangerousSamples)
_SLIDER(maxAccumulation, float, 0f, 100f, 2)
)dnl
