include(`Common.m4')dnl
define(`_NAME', `Blend')dnl
_PANE(Blend,
_SLIDER(amount, float, 0f, 1f, 4)
_COMBOBOX(mode, BLEND_MODES, Blend.BlendingMode)dnl
)dnl
