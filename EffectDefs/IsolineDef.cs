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
    internal class IsolineDef
    {
        public static Isoline isolineEffect;

        public static float _luminanceBlending { get; set; }
        public static float _fallOffDepth { get; set; }
        public static Color _backgroundColor { get; set; }
        public static Color _lineColor { get; set; }
        public static Vector3 _axis { get; set; }
        public static float _interval { get; set; }
        public static Vector3 _offset { get; set; }
        public static float _distortionFrequency { get; set; }
        public static float _distortionAmount { get; set; }
        public static Isoline.ModulationMode _modulationMode { get; set; }
        public static Vector3 _modulationAxis { get; set; }
        public static float _modulationFrequency { get; set; }
        public static float _modulationSpeed { get; set; }
        public static float _modulationExponent { get; set; }
        public static Vector3 _direction { get; set; }
        public static float _speed { get; set; }

        public IsolineDef()
        {
            if( isolineEffect == null)
            {
                isolineEffect = Util.GetComponentVar<Isoline, IsolineDef>(isolineEffect);
            }

            _luminanceBlending = 1f;
            _fallOffDepth = 40;
            _backgroundColor = Color.white;
            _axis = Vector3.one * 0.577f;
            _lineColor = Color.white;
            _interval = 0.25f;
            _offset = Vector3.zero;
            _distortionFrequency = 1f;
            _distortionAmount = 0;
            _modulationMode = Isoline.ModulationMode.None;
            _modulationAxis = Vector3.forward;
            _modulationFrequency = 0.2f;
            _modulationSpeed = 1f;
            _modulationExponent = 24f;
            _direction = Vector3.one * 0.577f;
            _speed = 0.2f;
        }

        public void InitMemberByInstance(Isoline isoline)
        {
            _luminanceBlending = isoline._luminanceBlending;
            _fallOffDepth = isoline._fallOffDepth;
            _backgroundColor = isoline._backgroundColor;
            _axis = isoline._axis;
            _lineColor = isoline._lineColor;
            _interval = isoline._interval;
            _offset = isoline._offset;
            _distortionFrequency = isoline._distortionFrequency;
            _distortionAmount = isoline._distortionAmount;
            _modulationMode = isoline._modulationMode;
            _modulationAxis = isoline._modulationAxis;
            _modulationFrequency = isoline._modulationFrequency;
            _modulationSpeed = isoline._modulationSpeed;
            _modulationExponent = isoline._modulationExponent;
            _direction = isoline._direction;
            _speed = isoline._speed;
        }

        public static void Update(IsolinePane isolinePane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                isolinePane.IsEnabled = isolineEffect.enabled;
            }
            else
            {
                isolineEffect.enabled = isolinePane.IsEnabled;
            }

            isolineEffect._luminanceBlending = isolinePane._luminanceBlendingValue;
            isolineEffect._fallOffDepth = isolinePane._fallOffDepthValue;
            isolineEffect._backgroundColor = isolinePane._backgroundColorValue;
            isolineEffect._axis = isolinePane._axisValue;
            isolineEffect._lineColor = isolinePane._lineColorValue;
            isolineEffect._interval = isolinePane._intervalValue;

            if( isolinePane._speedValue == 0 )
                isolineEffect._offset = isolinePane._offsetValue;
            else
                isolinePane._offsetValue = isolineEffect._offset;

            isolineEffect._distortionFrequency = isolinePane._distortionFrequencyValue;
            isolineEffect._distortionAmount = isolinePane._distortionAmountValue;
            isolineEffect._modulationMode = isolinePane._modulationModeValue;
            isolineEffect._modulationAxis = isolinePane._modulationAxisValue;
            isolineEffect._modulationFrequency = isolinePane._modulationFrequencyValue;
            isolineEffect._modulationSpeed = isolinePane._modulationSpeedValue;
            isolineEffect._modulationExponent = isolinePane._modulationExponentValue;
            isolineEffect._direction = isolinePane._directionValue;
            isolineEffect._speed = isolinePane._speedValue;
        }

        public static void Reset()
        {
            if( isolineEffect == null )
                return;

            isolineEffect._luminanceBlending = _luminanceBlending;
            isolineEffect._fallOffDepth = _fallOffDepth;
            isolineEffect._backgroundColor = _backgroundColor;
            isolineEffect._axis = _axis;
            isolineEffect._lineColor = _lineColor;
            isolineEffect._interval = _interval;
            isolineEffect._offset = _offset;
            isolineEffect._distortionFrequency = _distortionFrequency;
            isolineEffect._distortionAmount = _distortionAmount;
            isolineEffect._modulationMode = _modulationMode;
            isolineEffect._modulationAxis = _modulationAxis;
            isolineEffect._modulationFrequency = _modulationFrequency;
            isolineEffect._modulationSpeed = _modulationSpeed;
            isolineEffect._modulationExponent = _modulationExponent;
            isolineEffect._direction = _direction;
            isolineEffect._speed = _speed;
        }
    }
}

