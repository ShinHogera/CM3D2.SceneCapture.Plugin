include(`Common.m4')dnl
define(`_NAME', `Halftone')dnl
_DEF(Halftone,
_PROP(scale, float, 12f)dnl
_PROP(dotSize, float, 1.35f)dnl
_PROP(angle, float, 1.2f)dnl
_PROP(smoothness, float, 0.080f)dnl
_PROP(center, Vector2, new Vector2(0.5f, 0.5f))dnl
_PROP(desaturate, bool, false)dnl
)dnl
