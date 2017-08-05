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
    internal class SepiaPane : BasePane
    {
        public SepiaPane( int fontSize ) : base( fontSize, "Sepia" ) {}

        override public void SetupPane()
        {

        }

        override public void ShowPane()
        {

        }

        override public void Reset()
        {

        }
    }
}
