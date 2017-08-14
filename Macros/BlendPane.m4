include(`Common.m4')dnl
define(`_NAME', `Blend')dnl
_ENUM(BLEND_MODES, `"Darken" ``,'' "Multiply" ``,'' "ColorBurn" ``,'' "LinearBurn" ``,'' "DarkerColor" ``,'' "Lighten" ``,'' "Screen" ``,'' "ColorDodge" ``,'' "LinearDodge" ``,'' "LighterColor" ``,'' "Overlay" ``,'' "SoftLight" ``,'' "HardLight" ``,'' "VividLight" ``,'' "LinearLight" ``,'' "PinLight" ``,'' "HardMix" ``,'' "Difference" ``,'' "Exclusion" ``,'' "Subtract" ``,'' "Divide" ')dnl
_PANE(Blend,
_SLIDER(amount, float, 0f, 1f, 4)
_COMBOBOX(mode, BLEND_MODES, Blend.BlendingMode)dnl
)dnl
