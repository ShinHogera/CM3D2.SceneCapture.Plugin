divert(-1)dnl

define(`upcase', `translit(`$*', `a-z', `A-Z')')

define(`downcase', `translit(`$*', `A-Z', `a-z')')
define(`_capitalize', `regexp(`$1', `^\(\w\)\(\w*\)', `upcase(`\1')`'downcase(`\2')')')
define(`capitalize', `patsubst(`$1', `\w+', `_$0(`\&')')')
define(`_NAME', `Undefined')
define(`_DEFS', `')
define(`_SHOWS', `')
define(`_PROPS', `')
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
        public _NAME( int fontSize ) : base( fontSize, Translation.GetText("Panes", "$1") ) {}

        override public void SetupPane()
        { $2
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
_DEFS`        private Custom$3 $1$2 = null;
')dnl
define(`_SHOWS',dnl
_SHOWS`           GUIUtil.AddGUI$4(this, this.$1$2);
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
`_REDEF(`$1', `Checkbox', `Checkbox', `Checkbox', `bool', `this.$1Checkbox.Value')dnl
_CHILD(`$1Checkbox',dnl
`            this.$1Checkbox = new CustomToggleButton ( _EFFECT(`$1'), "toggle" );
            this.$1Checkbox.Text = _TRANSL(`$1');')')dnl

define(`_SLIDER',dnl
`_REDEF(`$1', `Slider', `Slider', `Checkbox', `$2', `ifelse($2, float,,($2))this.$1Slider.Value')dnl
_CHILD(`$1Slider',dnl
            `this.$1Slider = new CustomSlider ( _EFFECT(`$1'), $3, $4, 4 );
            this.$1Slider.Text = _TRANSL(`$1');')')dnl

define(`_COLORPICKER',dnl
`_REDEF(`$1', `ColorPicker', `Picker', `Checkbox', `Color', `this.$1ColorPicker.Value')dnl
_CHILD(`$1Picker',dnl
`            this.$1Picker = new CustomColorPicker ( _EFFECT(`$1') );
            this.$1Picker.Text = _TRANSL(`$1');')')dnl

# define(`_CHECKBOX',dnl
# `define(`_DEFS',dnl
# _DEFS`private CustomToggleButton $1Checkbox = null;')
# dnl
# define(`_SHOWS',dnl
# _SHOWS`GUIUtil.AddGUICheckbox(this, this.$1Checkbox);')
# dnl
# define(`_PROPS',dnl
# _PROPS`VAL(`$1', `bool', `this.$1Checkbox.Value')')
# dnl
# _CHILD(`$1Checkbox', `this.$1Checkbox = new CustomToggleButton ( $2, "toggle" );
# this.$1Checkbox.Text = Translation.GetText("_NAME", "$1");')')dnl
# dnl
# dnl
# define(`_SLIDER',dnl
# `define(`_DEFS',dnl
# _DEFS`private CustomSlider $1Slider = null;')
# dnl
# define(`_SHOWS',dnl
# _SHOWS`GUIUtil.AddGUISlider(this, this.$1Slider);')
# dnl
# define(`_PROPS',dnl
# _PROPS`VAL(`$1', `float', `this.$1Slider.Value')')
# dnl
# _CHILD(`$1Slider', `this.$1Slider = new CustomSlider( _NAMEDef.downcase(`_NAME')Effect.$1, $2, $3, 4 );
# this.$1Slider.Text = Translation.GetText("_NAME", "$1")')')dnl
# dnl
# dnl
# define(`_COLORPICKER',dnl
# `define(`_DEFS',dnl
# _DEFS`private CustomColorPicker $1Picker = null;')dnl
# dnl
# define(`_SHOWS',dnl
# _SHOWS`GUIUtil.AddGUICheckbox(this, this.$1Picker);')dnl
# dnl
# define(`_PROPS',dnl
# _PROPS`VAL(`$1', `Color', `this.$1Slider.Value')')dnl
# dnl
# _CHILD(`$1Picker', `this.$1Picker = new CustomColorPicker( $2 );
# this.$1Picker.Text = Translation.GetText("_NAME", "$1")')')dnl
# dnl
# dnl
divert(0)dnl
