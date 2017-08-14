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
    internal class ChannelMixerDef
    {
        public static ChannelMixer channelMixerEffect;

        public static Vector3 red { get; set; }
        public static Vector3 green { get; set; }
        public static Vector3 blue { get; set; }
        public static Vector3 constant { get; set; }

        public ChannelMixerDef()
        {
            if( channelMixerEffect == null)
            {
                channelMixerEffect = Util.GetComponentVar<ChannelMixer, ChannelMixerDef>(channelMixerEffect);
            }

            red = new Vector3(100f, 0f, 0f);
            green = new Vector3(0f, 100f, 0f);
            blue = new Vector3(0f, 0f, 100f);
            constant = new Vector3(0f, 0f, 0f);
        }

        public void InitMemberByInstance(ChannelMixer channelMixer)
        {
            red = channelMixer.red;
            green = channelMixer.green;
            blue = channelMixer.blue;
            constant = channelMixer.constant;
        }

        public static void Update(ChannelMixerPane channelMixerPane)
        {
            if( Instances.needEffectWindowReload == true )
            {
                channelMixerPane.IsEnabled = channelMixerEffect.enabled;
            }
            else
            {
                channelMixerEffect.enabled = channelMixerPane.IsEnabled;
            }

            channelMixerEffect.red = channelMixerPane.RedValue;
            channelMixerEffect.green = channelMixerPane.GreenValue;
            channelMixerEffect.blue = channelMixerPane.BlueValue;
            channelMixerEffect.constant = channelMixerPane.ConstantValue;
        }

        public static void Reset()
        {
            if( channelMixerEffect == null )
                return;

            channelMixerEffect.red = red;
            channelMixerEffect.green = green;
            channelMixerEffect.blue = blue;
            channelMixerEffect.constant = constant;
        }
    }
}

