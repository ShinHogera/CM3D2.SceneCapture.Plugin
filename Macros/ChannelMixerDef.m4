include(`Common.m4')dnl
define(`_NAME', `ChannelMixer')dnl
_DEF(ChannelMixer,
_PROP(red, Vector3, new Vector3(100f, 0f, 0f))dnl
_PROP(green, Vector3, new Vector3(0f, 100f, 0f))dnl
_PROP(blue, Vector3, new Vector3(0f, 0f, 100f))dnl
_PROP(constant, Vector3, new Vector3(0f, 0f, 0f))dnl
)dnl
