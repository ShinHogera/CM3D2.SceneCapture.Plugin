include(`Common.m4')dnl
define(`_NAME', `BrightnessContrastGamma')dnl
_DEF(BrightnessContrastGamma,
_PROP(brightness, float, 0f)dnl
_PROP(contrast, float, 0f)dnl
_PROP(contrastCoeff, Vector3, new Vector3(0.5f, 0.5f, 0.5f))dnl
_PROP(gamma, float, 1f)dnl
)dnl
