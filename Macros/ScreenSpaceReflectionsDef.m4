include(`Common.m4')dnl
define(`_NAME', `ScreenSpaceReflections')dnl
_DEF(ScreenSpaceReflections,
_PROP(SampleCount, ScreenSpaceReflections.SampleCount, ScreenSpaceReflections.SampleCount.Medium)dnl
_PROP(downsampling, int, 2)dnl
_PROP(intensity, float, 1.0f)dnl
_PROP(rayDiffusion, float, 0.01f)dnl
_PROP(blurSize, float, 1.0f)dnl
_PROP(raymarchDistance, float, 2.5f)dnl
_PROP(falloffDistance, float, 2.5f)dnl
_PROP(rayHitDistance, float, 0.15f)dnl
_PROP(maxAccumulation, float, 25.0f)dnl
_PROP(stepBoost, float, 0.0f)dnl
_PROP(dangerousSamples, bool, true)dnl
_PROP(preRaymarchPass, bool, true)dnl
)dnl
