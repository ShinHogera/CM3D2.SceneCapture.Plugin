include(`Common.m4')dnl
define(`_NAME', `Dithering')dnl
_PANE(Dithering,
_CHECKBOX(showOriginal)
_CHECKBOX(convertToGrayscale)
_SLIDER(redLuminance, float, 0f, 1f, 4)
_SLIDER(greenLuminance, float, 0f, 1f, 4)
_SLIDER(blueLuminance, float, 0f, 1f, 4)
_SLIDER(amount, float, 0f, 1f, 4)dnl
)dnl
