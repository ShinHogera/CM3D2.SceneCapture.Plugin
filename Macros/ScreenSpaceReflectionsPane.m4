include(`Common.m4')dnl
define(`_NAME', `ScreenSpaceReflections')dnl
_ENUM(SSR_SAMPLE_COUNTS, "Low" ````,'''' "Medium" ````,'''' "High")dnl
_PANE(ScreenSpaceReflections,
_COMBOBOX(sampleCount, SSR_SAMPLE_COUNTS)
_SLIDER(downsampling, int, 1f, 8f, 0)
_SLIDER(intensity, float, 0.0f, 2.0f, 4)
_SLIDER(rayDiffusion, float, 0.0f, 1.0f, 4)
_SLIDER(blurSize, float, 0.0f, 8.0f, 4)
_SLIDER(raymarchDistance, float, 0f, 10f, 4)
_SLIDER(falloffDistance, float, 0f, 10f, 4)
_SLIDER(rayHitDistance, float, 0f, 10f, 4)
_SLIDER(maxAccumulation, float, 0f, 100f, 2)
_SLIDER(stepBoots, float, 0f, 10f, 4)
_CHECKBOX(dangerousSamples)
_CHECKBOX(preRaymarchPass)
)dnl
