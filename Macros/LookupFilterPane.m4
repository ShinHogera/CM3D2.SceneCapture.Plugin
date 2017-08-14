include(`Common.m4')dnl
define(`_NAME', `LookupFilter')dnl
_PANE(LookupFilter,
_SLIDER(amount, float, 0f, 1f, 4)
_CHECKBOX(forceCompatibility)
)dnl
