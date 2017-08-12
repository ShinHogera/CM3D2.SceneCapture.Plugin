include(`Common.m4')dnl
define(`_NAME', `ComicBook')dnl
_DEF(ComicBook,
_PROP(stripAngle, float, 0.6f)dnl
_PROP(stripDensity, float, 180f)dnl
_PROP(stripThickness, float, 0.5f)dnl
_PROP(stripLimits, Vector2, new Vector2(0.25f, 0.4f))dnl
_PROP(stripInnerColor, Color, new Color(0.3f, 0.3f, 0.3f))dnl
_PROP(stripOuterColor, Color, new Color(0.8f, 0.8f, 0.8f))dnl
_PROP(fillColor, Color, new Color(0.1f, 0.1f, 0.1f))dnl
_PROP(backgroundColor, Color, Color.white)dnl
_PROP(edgeDetection, bool, false)dnl
_PROP(edgeThreshold, float, 5f)dnl
_PROP(edgeColor, Color, Color.black)dnl
_PROP(amount, float, 1f)dnl
)dnl
