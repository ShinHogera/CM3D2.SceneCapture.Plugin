include(`Common.m4')dnl
define(`_NAME', `WhiteBalance')dnl
_PANE(WhiteBalance,
_COLORPICKER(white)
_COMBOBOX(mode, WHITEBALANCE_BALANCEMODES, WhiteBalance.BalanceMode)dnl
)dnl
