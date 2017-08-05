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
    internal class ChannelSwapDef
    {
        public static ChannelSwapper channelSwapEffect;

        public static ChannelSwapper.Channel redSource { get; set; }
        public static ChannelSwapper.Channel greenSource { get; set; }
        public static ChannelSwapper.Channel blueSource { get; set; }

        static ChannelSwapDef()
        {
            if(channelSwapEffect == null)
            {
                channelSwapEffect = Util.GetComponentVar<ChannelSwapper, ChannelSwapDef>(channelSwapEffect);
            }
            redSource = ChannelSwapper.Channel.Red;
            greenSource = ChannelSwapper.Channel.Green;
            blueSource = ChannelSwapper.Channel.Blue;
        }

        public static void InitMemberByInstance(ChannelSwapper cs)
        {
            redSource = cs.redSource;
            greenSource = cs.greenSource;
            blueSource = cs.blueSource;
        }

        public static void Update(ChannelSwapPane channelSwapPane)
        {
            if (Instances.needEffectWindowReload == true)
                channelSwapPane.IsEnabled = channelSwapEffect.enabled;
            else
                channelSwapEffect.enabled = channelSwapPane.IsEnabled;

            channelSwapEffect.redSource = channelSwapPane.RedSourceValue;
            channelSwapEffect.greenSource = channelSwapPane.GreenSourceValue;
            channelSwapEffect.blueSource = channelSwapPane.BlueSourceValue;
        }

        public static void Reset()
        {
            if (channelSwapEffect == null)
                return;

            channelSwapEffect.redSource = redSource;
            channelSwapEffect.greenSource = greenSource;
            channelSwapEffect.blueSource = blueSource;
        }

        public static void OnLoad()
        {

        }
    }
}
