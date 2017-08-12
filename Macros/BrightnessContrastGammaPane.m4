include(`Common.m4')dnl
define(`_NAME', `BrightnessContrastGamma')dnl
_VEC(contrastCoeff)dnl
_PANE(BrightnessContrastGamma,
_SLIDER(brightness, float, -100f, 100f, 2)
_SLIDER(contrast, float, -100f, 100f, 2)
_VECSLIDER(contrastCoeff, x, 0f, 10f, 4)
_VECSLIDER(contrastCoeff, y, 0f, 10f, 4)
_VECSLIDER(contrastCoeff, z, 0f, 10f, 4)
_SLIDER(gamma, float, 0.1f, 9.9f, 4)dnl
)dnl
