include(`Common.m4')dnl
define(`_NAME', `ShadowsMidtonesHighlights')dnl
_PANE(ShadowsMidtonesHighlights,
_COMBOBOX(mode, SMH_COLORMODES, ShadowsMidtonesHighlights.ColorMode)
_COLORPICKER(shadows)
_COLORPICKER(midtones)
_COLORPICKER(highlights)
_SLIDER(amount, float, 0f, 1f, 4)dnl
)dnl
