include(`Common.m4')dnl
define(`_NAME', `Convolution3x3')dnl
_DEF(Convolution3x3,
_PROP(kernelTop, Vector3, Vector3.zero)dnl
_PROP(kernelMiddle, Vector3, Vector3.up)dnl
_PROP(kernelBottom, Vector3, Vector3.zero)dnl
_PROP(divisor, float, 1f)dnl
_PROP(amount, float, 1f)dnl
)dnl
