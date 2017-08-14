include(`Common.m4')dnl
define(`_NAME', `Dithering')dnl
_DEF(Dithering,
_PROP(showOriginal, bool, false)dnl
_PROP(convertToGrayscale, bool, false)dnl
_PROP(redLuminance, float, 0.299f)dnl
_PROP(greenLuminance, float, 0.587f)dnl
_PROP(blueLuminance, float, 0.114f)dnl
_PROP(amount, float, 1f)dnl
)dnl
