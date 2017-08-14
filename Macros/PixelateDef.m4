include(`Common.m4')dnl
define(`_NAME', `Pixelate')dnl
_DEF(Pixelate,
_PROP(scale, float, 80.0f)dnl
_PROP(automaticRatio, bool, true)dnl
_PROP(ratio, float, 1.0f)dnl
_PROP(mode, Pixelate.SizeMode, Pixelate.SizeMode.ResolutionIndependent)dnl
)dnl
