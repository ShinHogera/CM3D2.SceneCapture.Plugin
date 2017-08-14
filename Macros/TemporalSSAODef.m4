include(`Common.m4')dnl
define(`_NAME', `TemporalSSAO')dnl
_DEF(TemporalSSAO,
_PROP(SampleCount, TemporalSSAO.SampleCount, TemporalSSAO.SampleCount.Medium)dnl
_PROP(downsampling, int, 3)dnl
_PROP(radius, float, 0.25f)dnl
_PROP(intensity, float, 1.5f)dnl
_PROP(blurSize, float, 0.5f)dnl
_PROP(dangerousSamples, bool, true)dnl
_PROP(maxAccumulation, float, 100.0f)dnl
)dnl
