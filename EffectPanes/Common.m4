divert(-1)dnl

define(`upcase', `translit(`$*', `a-z', `A-Z')')

define(`downcase', `translit(`$*', `A-Z', `a-z')')
define(`_capitalize', `regexp(`$1', `^\(\w\)\(\w*\)', `upcase(`\1')`'`\2'')')
define(`capitalize', `patsubst(`$1', `\w+', `_$0(`\&')')')
define(`_NAME', `Undefined')
define(`_DEFS', `')
define(`_SHOWS', `')
define(`_PROPS', `')

define(`_DEFAULTS', `')
define(`_INITS', `')
define(`_UPDATES', `')
define(`_RESETS', `')

define(`_DEF',`define(`_NAME', `$1')dnl
using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CM3D2.SceneCapture.Plugin
{
    internal class _NAME`'Def
    {
        public static _NAME _EFFECTVAR;
$2
        public _NAME`'Def()
        {
            if( _EFFECTVAR == null)
            {
                _EFFECTVAR = Util.GetComponentVar<_NAME, _NAME`'Def>(_EFFECTVAR);
            }
_DEFAULTS
        }

        public void InitMemberByInstance(_NAME downcase(_NAME))
        {
_INITS        }

        public static void Update(_NAME`'Pane downcase(_NAME)Pane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                downcase(_NAME)Pane.IsEnabled = _EFFECTVAR.enabled;
            }
            else
            {
                _EFFECTVAR.enabled = downcase(_NAME)Pane.IsEnabled;
            }
        _UPDATES
        }

        public static void Reset()
        {
            if( _EFFECTVAR == null )
                return;
        _RESETS
        }
    }
}
'
)

define(`_EFFECTVAR', `downcase(_NAME)Effect')
define(`_DEFREDEF',dnl
  `define(`_DEFAULTS',dnl
_DEFAULTS
            `$1 = $3;')dnl
  define(`_INITS',dnl
_INITS`            $1 = downcase(_NAME).$1';
)dnl
  define(`_UPDATES',dnl
_UPDATES
            downcase(_NAME)`Effect.$1' = downcase(_NAME)`Pane.'capitalize($1)`Value;')
  define(`_RESETS',dnl
_RESETS
            _EFFECTVAR`.$1 = $1;')')

define(`_VECPROP', `_DEFREDEF(`$1', `$2')dnl
      public static $2 $1 { get; set; }')

define(`_PROP', `_DEFREDEF(`$1', `$2', `$3')dnl
      public static $2 $1 { get; set; }')dnl

define(`_PANE', `define(`_NAME', `$1Pane')dnl
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.SceneCapture.Plugin
{
    internal class _NAME : BasePane
    {
        public _NAME`'( int fontSize ) : base( fontSize, Translation.GetText("Panes", "$1") ) {}

        override public void SetupPane()
        {$2
        }

        override public void ShowPane()
        {
_SHOWS        }

        override public void Reset()
        {
            $1Def.Reset();
        }

        #region Properties
_PROPS        #endregion

        #region Fields
_DEFS        #endregion
    }
}
')dnl
define(`_CHILD', `$2
            this.ChildControls.Add( this.$1 );')dnl
dnl
dnl
define(`_VAL', `        public $2 capitalize(`$1')Value {
            get
            {
                return $3;
            }
        }

')dnl
dnl
define(`_TRANSL', `Translation.GetText("_NAME", "$1")')
define(`_EFFECT', `_NAME`'Def.downcase(_NAME)Effect.$1')
dnl
define(`_REDEF',dnl
`define(`_DEFS',dnl
_DEFS`        private Custom$2 $1$3 = null;
')dnl
define(`_SHOWS',dnl
_SHOWS`           GUIUtil.AddGUI$4(this, this.$1$3);
')
define(`_PROPS',dnl
_PROPS`_VAL(`$1', `$5', `$6')')')

define(`_COMBOBOX',dnl
`_REDEF(`$1', `ComboBox', `Box', `Checkbox', `$3', `($3) Enum.Parse( typeof( $3 ), this.$1ComboBox.SelectedItem)')dnl
_CHILD(`$1ComboBox',dnl
            `this.$1ComboBox = new CustomComboBox ( $2 );
            this.$1ComboBox.Text = _TRANSL(`$1');
            this.$1ComboBox.SelectedIndex = (int)_EFFECT(`$1');')')dnl

define(`_CHECKBOX',dnl
`_REDEF(`$1', `ToggleButton', `Checkbox', `Checkbox', `bool', `this.$1Checkbox.Value')dnl
_CHILD(`$1Checkbox',dnl
`            this.$1Checkbox = new CustomToggleButton ( _EFFECT(`$1'), "toggle" );
            this.$1Checkbox.Text = _TRANSL(`$1');')')dnl

define(`_SLIDER',dnl
`_REDEF($1, `Slider', `Slider', `Slider', `$2', `ifelse($2, float,,($2))this.$1Slider.Value')dnl
_CHILD(`$1Slider',dnl
            `this.$1Slider = new CustomSlider ( _EFFECT(`$1'), $3, $4, $5 );
            this.$1Slider.Text = _TRANSL(`$1');')')dnl

define(`_VECSLIDER',dnl
`_REDEF(`$1'upcase($2), `Slider', `Slider', `Slider', `float', `this.$1'upcase($2)`Slider.Value')dnl
_CHILD(`$1'upcase($2)`Slider',dnl
            `this.$1'upcase($2)`Slider = new CustomSlider ( _EFFECT(`$1.$2'), $3, $4, $5);
            this.$1'upcase($2)`Slider.Text = _TRANSL(`$1'upcase($2));')')dnl

define(`_COLORPICKER',dnl
`_REDEF(`$1', `ColorPicker', `Picker', `Checkbox', `Color', `this.$1Picker.Value')dnl
_CHILD(`$1Picker',dnl
`            this.$1Picker = new CustomColorPicker ( _EFFECT(`$1') );
            this.$1Picker.Text = _TRANSL(`$1');')')dnl

define(`_VEC',
`define(`_PROPS',dnl
_PROPS`_VAL(`$1', `Vector3', `new Vector3(this.$1XSlider.Value, this.$1YSlider.Value, this.$1ZSlider.Value)')')')

divert(0)dnl
