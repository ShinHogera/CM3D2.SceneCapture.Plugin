include(`Common.m4')dnl
define(`_NAME', `ChannelMixer')dnl
_VEC(red)dnl
_VEC(green)dnl
_VEC(blue)dnl
_VEC(constant)dnl
_PANE(ChannelMixer,
_VECSLIDER(red, x, 0f, 100f, 4)
_VECSLIDER(red, y, 0f, 100f, 4)
_VECSLIDER(red, z, 0f, 100f, 4)
_VECSLIDER(green, x, 0f, 100f, 4)
_VECSLIDER(green, y, 0f, 100f, 4)
_VECSLIDER(green, z, 0f, 100f, 4)
_VECSLIDER(blue, x, 0f, 100f, 4)
_VECSLIDER(blue, y, 0f, 100f, 4)
_VECSLIDER(blue, z, 0f, 100f, 4)
_VECSLIDER(constant, x, 0f, 100f, 4)
_VECSLIDER(constant, y, 0f, 100f, 4)
_VECSLIDER(constant, z, 0f, 100f, 4)dnl
)dnl
