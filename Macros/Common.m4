include(`Translations.m4')
dnl divert(-1)dnl
dnl 
dnl define(`upcase', `translit(`$*', `a-z', `A-Z')')
dnl define(`downcase', `translit(`$*', `A-Z', `a-z')')
dnl define(`_capitalize', `regexp(`$1', `^\(\w\)\(\w*\)', `upcase(`\1')`'`\2'')')
dnl define(`capitalize', `patsubst(`$1', `\w+', `_$0(`\&')')')
dnl define(`_decapitalize', `regexp(`$1', `^\(\w\)\(\w*\)', `downcase(`\1')`'`\2'')')
dnl define(`decapitalize', `patsubst(`$1', `\w+', `_$0(`\&')')')
dnl define(`_NAME', `Undefined')
dnl define(`_DEFS', `')
dnl define(`_SHOWS', `')
dnl define(`_PROPS', `')
dnl 
dnl define(`_DEFAULTS', `')
dnl define(`_INITS', `')
dnl define(`_UPDATES', `')
dnl define(`_RESETS', `')
dnl 
dnl define(`_DEF',`define(`_NAME', `$1')dnl
dnl using System;
dnl using System.Reflection;
dnl using System.Linq;
dnl using UnityEngine;
dnl using UnityInjector;
dnl using UnityInjector.Attributes;
dnl using System.Collections.Generic;
dnl using System.IO;
dnl using System.Xml;
dnl 
dnl namespace CM3D2.SceneCapture.Plugin
dnl {
dnl     internal class _NAME`'Def
dnl     {
dnl         public static _NAME _EFFECTVAR;
dnl $2
dnl 
dnl         public _NAME`'Def()
dnl         {
dnl             if( _EFFECTVAR == null)
dnl             {
dnl                 _EFFECTVAR = Util.GetComponentVar<_NAME, _NAME`'Def>(_EFFECTVAR);
dnl             }
dnl _DEFAULTS
dnl         }
dnl 
dnl         public void InitMemberByInstance(_NAME decapitalize(_NAME))
dnl         {
dnl _INITS        }
dnl 
dnl         public static void Update(_NAME`'Pane decapitalize(_NAME)Pane)
dnl         {
dnl             if( Instances.needEffectWindowReload == true )
dnl             {
dnl                 decapitalize(_NAME)Pane.IsEnabled = _EFFECTVAR.enabled;
dnl             }
dnl             else
dnl             {
dnl                 _EFFECTVAR.enabled = decapitalize(_NAME)Pane.IsEnabled;
dnl             }
dnl _UPDATES
dnl         }
dnl 
dnl         public static void Reset()
dnl         {
dnl             if( _EFFECTVAR == null )
dnl                 return;
dnl _RESETS
dnl         }
dnl     }
dnl }
dnl '
dnl )
dnl 
dnl define(`_EFFECTVAR', `decapitalize(_NAME)Effect')
dnl define(`_DEFREDEF',dnl
dnl `define(`_DEFAULTS',dnl
dnl _DEFAULTS
dnl             `$1 = $3;')dnl
dnl define(`_INITS',dnl
dnl _INITS`            $1 = decapitalize(_NAME).$1';
dnl )dnl
dnl define(`_UPDATES',dnl
dnl _UPDATES
dnl             decapitalize(_NAME)`Effect.$1' = decapitalize(_NAME)`Pane.'capitalize($1)`Value;')
dnl define(`_RESETS',dnl
dnl _RESETS
dnl             _EFFECTVAR`.$1 = $1;')')
dnl 
dnl define(`_PROP',dnl
dnl `_DEFREDEF(`$1', `$2', `$3')dnl
dnl         public static $2 $1 { get; set; }')
dnl dnl
dnl define(`_PANE', `define(`_NAME', `$1Pane')dnl
dnl using System;
dnl using System.Collections.Generic;
dnl using System.IO;
dnl using System.Reflection;
dnl using System.Text;
dnl using System.Text.RegularExpressions;
dnl using System.Xml;
dnl using System.Xml.Serialization;
dnl using System.Linq;
dnl using System.Runtime.Serialization.Formatters.Binary;
dnl using UnityEngine;
dnl using UnityInjector;
dnl using UnityInjector.Attributes;
dnl 
dnl namespace CM3D2.SceneCapture.Plugin
dnl {
dnl     internal class _NAME : BasePane
dnl     {
dnl         public _NAME`'( int fontSize ) : base( fontSize, Translation.GetText("Panes", "$1") ) {}
dnl 
dnl         override public void SetupPane()
dnl         {$2
dnl         }
dnl 
dnl         override public void ShowPane()
dnl         {
dnl _SHOWS        }
dnl 
dnl         override public void Reset()
dnl         {
dnl             $1Def.Reset();
dnl         }
dnl 
dnl         #region Properties
dnl _PROPS        #endregion
dnl 
dnl         #region Fields
dnl _DEFS        #endregion
dnl     }
dnl }
dnl ')dnl
dnl define(`_CHILD', `$2
dnl             this.ChildControls.Add( this.$1 );')dnl
dnl dnl
dnl dnl
dnl define(`_VAL', `        public $2 capitalize(`$1')Value
dnl         {
dnl             get
dnl             {
dnl                 return $3;
dnl             }
dnl         }
dnl 
dnl ')dnl
dnl dnl
dnl define(`_TRANSL', `Translation.GetText("_NAME", "$1")')
dnl define(`_EFFECT', `_NAME`'Def.decapitalize(_NAME)Effect.$1')
dnl dnl
dnl define(`_REDEF',dnl
dnl `define(`_DEFS',dnl
dnl _DEFS`        private Custom$2 $1$3 = null;
dnl ')dnl
dnl define(`_SHOWS',dnl
dnl _SHOWS`           GUIUtil.AddGUI$4(this, this.$1$3);
dnl ')
dnl define(`_PROPS',dnl
dnl _PROPS`_VAL(`$1', `$5', `$6')')')
dnl 
dnl define(`_COMBOBOX',dnl
dnl `_REDEF(`$1', `ComboBox', `ComboBox', `Checkbox', `$3', `($3) Enum.Parse( typeof( $3 ), this.$1ComboBox.SelectedItem)')dnl
dnl _CHILD(`$1ComboBox',dnl
dnl             `this.$1ComboBox = new CustomComboBox ( $2 );
dnl             this.$1ComboBox.Text = _TRANSL(`$1');
dnl             this.$1ComboBox.SelectedIndex = (int)_EFFECT(`$1');')')dnl
dnl 
dnl define(`_CHECKBOX',dnl
dnl `_REDEF(`$1', `ToggleButton', `Checkbox', `Checkbox', `bool', `this.$1Checkbox.Value')dnl
dnl _CHILD(`$1Checkbox',dnl
dnl `            this.$1Checkbox = new CustomToggleButton( _EFFECT(`$1'), "toggle" );
dnl             this.$1Checkbox.Text = _TRANSL(`$1');')')dnl
dnl 
dnl define(`_SLIDER',dnl
dnl `_REDEF($1, `Slider', `Slider', `Slider', `$2', `ifelse($2, float,,($2))this.$1Slider.Value')dnl
dnl _CHILD(`$1Slider',dnl
dnl             `this.$1Slider = new CustomSlider( _EFFECT(`$1'), $3, $4, $5 );
dnl             this.$1Slider.Text = _TRANSL(`$1');')')dnl
dnl 
dnl define(`_VECSLIDER',dnl
dnl `_REDEF(`$1'upcase($2), `Slider', `Slider', `Slider', `float', `this.$1'upcase($2)`Slider.Value')dnl
dnl _CHILD(`$1'upcase($2)`Slider',dnl
dnl             `this.$1'upcase($2)`Slider = new CustomSlider( _EFFECT(`$1.$2'), $3, $4, $5);
dnl             this.$1'upcase($2)`Slider.Text = _TRANSL(`$1'upcase($2));')')dnl
dnl 
dnl define(`_COLORPICKER',dnl
dnl `_REDEF(`$1', `ColorPicker', `Picker', `Checkbox', `Color', `this.$1Picker.Value')dnl
dnl _CHILD(`$1Picker',dnl
dnl `            this.$1Picker = new CustomColorPicker( _EFFECT(`$1') );
dnl             this.$1Picker.Text = _TRANSL(`$1');')')dnl
dnl 
dnl define(`_VEC',
dnl `define(`_PROPS',dnl
dnl _PROPS`_VAL(`$1', `Vector3', `new Vector3(this.$1XSlider.Value, this.$1YSlider.Value, this.$1ZSlider.Value)')')')
dnl 
dnl define(`_VEC2',
dnl `define(`_PROPS',dnl
dnl _PROPS`_VAL(`$1', `Vector2', `new Vector2(this.$1XSlider.Value, this.$1YSlider.Value)')')')
dnl 
dnl define(`_ENUM',
dnl `define(`_DEFS',dnl
dnl _DEFS`        private static readonly string[] $1 = new string[] { $2 };
dnl ')')
dnl 
dnl divert(0)dnl
