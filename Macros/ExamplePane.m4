dnl Compile with "m4 ExamplePane.m4 > ExamplePane.cs"
include(`Common.m4')dnl
define(`_NAME', `Example')dnl
_PANE(Example,
_SLIDER(dood, float, 1f, 100f, 2)
_COMBOBOX(lightType, LIGHT_TYPES, Light.LightType)
_CHECKBOX(isWorking)
_COLORPICKER(backgroundColor)dnl
)dnl
