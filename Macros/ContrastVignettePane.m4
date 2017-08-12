include(`Common.m4')dnl
define(`_NAME', `ContrastVignette')dnl
_VEC2(center)dnl
_VEC(contrastCoeff)dnl
_PANE(ContrastVignette,
_VECSLIDER(center, x, 0f, 1f, 4)
_VECSLIDER(center, y, 0f, 1f, 4)
_SLIDER(sharpness, float, -100f, 100f, 2)
_SLIDER(darkness, float, 0f, 100f, 2)
_SLIDER(contrast, float, 0f, 200f, 2)
_VECSLIDER(contrastCoeff, x, 0f, 1f, 4)
_VECSLIDER(contrastCoeff, y, 0f, 1f, 4)
_VECSLIDER(contrastCoeff, z, 0f, 1f, 4)
_SLIDER(edgeBlending, float, 0f, 200f, 2)dnl
)dnl
