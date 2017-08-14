include(`Common.m4')dnl
define(`_NAME', `ContrastVignette')dnl
_DEF(ContrastVignette,
_PROP(center, Vector2, new Vector2(0.5f, 0.5f))dnl
_PROP(sharpness, float, 32f)dnl
_PROP(darkness, float, 28f)dnl
_PROP(contrast, float, 20.0f)dnl
_PROP(contrastCoeff, Vector3, new Vector3(0.5f, 0.5f, 0.5f))dnl
_PROP(edgeBlending, float, 0f)dnl
)dnl
