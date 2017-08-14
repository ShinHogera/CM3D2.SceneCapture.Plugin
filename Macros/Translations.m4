divert(-1)
define(`_DEF',dnl
<$1>
$2</$1>)

define(`_TRANSLATION', `        <$1>$1</$1>'
)
define(`_PROP', _TRANSLATION($1))
divert(0)dnl
