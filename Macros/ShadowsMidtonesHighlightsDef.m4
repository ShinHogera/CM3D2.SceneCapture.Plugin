include(`Common.m4')dnl
define(`_NAME', `ShadowsMidtonesHighlights')dnl
_DEF(ShadowsMidtonesHighlights,
_PROP(mode, ShadowsMidtonesHighlights.ColorMode, ShadowsMidtonesHighlights.ColorMode.LiftGammaGain)dnl
_PROP(shadows, Color, new Color(1f, 1f, 1f, 0.5f))dnl
_PROP(midtones, Color, new Color(1f, 1f, 1f, 0.5f))dnl
_PROP(highlights, Color, new Color(1f, 1f, 1f, 0.5f))dnl
_PROP(amount, float, 1f)dnl
)dnl
