include(`Common.m4')dnl
define(`_NAME', `Wiggle')dnl
_ENUM(`WIGGLE_ALGORITHMS', `"Simple" ``````,'''''' "Complex"')dnl
_PANE(Wiggle,
_COMBOBOX(mode, WIGGLE_ALGORITHMS, Wiggle.Algorithm)
_SLIDER(timer, float, 0f, 100f, 2)
_SLIDER(speed, float, 0f, 100f, 2)
_SLIDER(frequency, float, 0f, 360f, 2)
_SLIDER(amplitude, float, 0f, 360f, 2)
_CHECKBOX(automaticTimer)dnl
)dnl
