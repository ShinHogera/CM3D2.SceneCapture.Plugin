include(`Common.m4')dnl
define(`_NAME', `DoubleVision')dnl
_VEC2(displace)dnl
_PANE(DoubleVision,
_VECSLIDER(displace, x, 0f, 1f, 4)
_VECSLIDER(displace, y, 0f, 1f, 4)
_SLIDER(amount, float, 0f, 1f, 4)dnl
)dnl
