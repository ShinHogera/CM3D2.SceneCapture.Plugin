include(`Common.m4')dnl
define(`_NAME', `WhiteBalance')dnl
_DEF(WhiteBalance,
_PROP(white, color, new Color(0.5f, 0.5f, 0.5f))dnl
_PROP(mode, WhiteBalance.BalanceMode, WhiteBalance.BalanceMode.Complex)dnl
)dnl
