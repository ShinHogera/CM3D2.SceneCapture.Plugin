dnl Compile with "m4 ExampleDef.m4 > ExampleDef.cs"
include(`Common.m4')dnl
define(`_NAME', `Example')dnl
_DEF(Example,
_PROP(dood, float, 0f)dnl
_PROP(lightType, Light.LightType, Light.LightType.Directional)dnl
_PROP(isWorking, bool, false)dnl
_PROP(backgroundColor, Color, new Color(1, 1, 1))dnl
)dnl
