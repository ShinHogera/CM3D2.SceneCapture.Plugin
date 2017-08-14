include(`Common.m4')dnl
define(`_NAME', `Isoline')dnl
_DEF(Isoline,
_PROP(_lineColor, Color, new Color(8, 0.125f, 3))dnl
_PROP(_luminanceBlending, float, 1f)dnl
_PROP(_fallOffDepth, float, 40)dnl
_PROP(_backgroundColor, Color, Color.white)dnl
_PROP(_axis, Vector3, Vector3.one * 0.577f))dnl
_PROP(_interval, float, 0.25f)dnl
_PROP(_offset, Vector3, Vector3.zero)dnl
_PROP(_distortionFrequency, float, 1f)dnl
_PROP(_distortionAmount, float, 0)dnl
_PROP(_modulationMode, Isoline.ModulationMode, Isoline.ModulationMode.None)dnl
_PROP(_modulationAxis, Vector3, Vector3.forward)dnl
_PROP(_modulationFrequency, float, 0.2f)dnl
_PROP(_modulationSpeed, float, 1f)dnl
_PROP(_modulationExponent, float, 24f)dnl
_PROP(_direction, Vector3, Vector3.one * 0.577f)dnl
_PROP(_speed, float, 0.2f)dnl
)dnl
