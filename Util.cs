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
    internal static class Util {
        private static AssetBundle assetShaders;
        private static AssetBundle extraShaders;

        private readonly static string SHADER_DIR = ConstantValues.ConfigDir + @"\Shaders";

        internal static AssetBundle LoadAssetBundle(string name) {
            string path = SHADER_DIR + @"\"+ name + ".unity3d";
            byte[] bytes = File.ReadAllBytes(path);
            return AssetBundle.LoadFromMemory(bytes);
        }

        internal static void LoadShaders() {
            assetShaders = LoadAssetBundle("unity_shaders");
            extraShaders = LoadAssetBundle("extra_shaders");
        }

        internal static int GetPix(int i)
        {
            float f = 1f + (Screen.width / 1280f - 1f) * 0.6f;
            return (int)(f * i);
        }

        internal static string DrawTextFieldF(Rect rect, string sField, GUIStyle style)
        {
            string sTmp;
            sTmp = GUI.TextField(rect, sField, style);
            if (sTmp != sField)
            {
                float fTmp;
                if (float.TryParse(sTmp, out fTmp))
                {
                    return sTmp;
                }
            }
            return sField;
        }


        internal static bool IsMouseOnRect(Rect rect)
        {
            if (rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
            {
                return true;
            }
            return false;
        }

        public static T GetComponentVar<T, T2>(T v) where T : UnityEngine.Component where T2 : class {
            if (v != null)
                return v;

            v = GameMain.Instance.MainCamera.gameObject.GetComponent<T>();
            if (v == null)
            {
                v = GameMain.Instance.MainCamera.gameObject.AddComponent<T>();

                string[] sShaders = Util.GetFieldNamesSpecifyType<T, Shader>();

                //Type typeShaders = typeof(Shaders);
                //foreach (string sShader in sShaders)
                //{
                //    FieldInfo field = typeShaders.GetField(sShader);
                //    if (field == null)
                //    {
                //        if (sShader == "blurShader")
                //        {
                //            field = typeShaders.GetField("separableBlurShader");
                //        }
                //        else
                //        {
                //            field = typeShaders.GetField(typeof(T).Name + "_" + sShader);
                //        }
                //        if (field == null)
                //            continue;
                //    }

                //    Shader shader = new Material((string)field.GetValue(null)).shader;
                //    Util.SetFieldValue<T>(v, sShader, shader);
                //}

                foreach (string sShader in sShaders)
                {
                    Shader shader = assetShaders.LoadAsset(sShader) as Shader;
                    if(shader == null)
                    {
                        if (sShader == "blurShader")
                        {
                            shader = assetShaders.LoadAsset("separableBlurShader") as Shader;
                        }
                        else
                        {
                            shader = assetShaders.LoadAsset(typeof(T).Name + "_" + sShader) as Shader;
                        }

                        if (shader == null) {
                            shader = extraShaders.LoadAsset(typeof(T).Name + "_" + sShader) as Shader;
                        }

                        if (shader == null) {
                            Debug.LogError("Shader " + typeof(T).Name + " has no shader file in extrashaders.");
                            continue;
                        }
                    }

                    Util.SetFieldValue<T>(v, sShader, shader);
                }


                Type typeDef = typeof(T2);
                MethodInfo mi = typeDef.GetMethod("InitExtra");
                if (mi != null)
                    mi.Invoke(null, new object[] { v });
            }
            else
            {
                Util.SetPropertyValue<T>(v, "enabled", true);
                Type typeDef = typeof(T2);
                MethodInfo mi = typeDef.GetMethod("InitMemberByInstance");
                if (mi != null)
                    mi.Invoke(null, new object[] { v });
            }

            return v;
        }

        public static object GetComponentVar(Type t1, Type t2, object v)
        {
            v = GameMain.Instance.MainCamera.gameObject.GetComponent(t1);
            if (v == null)
            {
                v = GameMain.Instance.MainCamera.gameObject.AddComponent(t1);

                string[] sShaders = Util.GetFieldNamesSpecifyType(t1, typeof(Shader));
                foreach (string sShader in sShaders)
                {
                    Shader shader = assetShaders.LoadAsset(sShader) as Shader;
                    if (shader == null)
                    {
                        if (sShader == "blurShader")
                        {
                            shader = assetShaders.LoadAsset("separableBlurShader") as Shader;
                        }
                        else
                        {
                            shader = assetShaders.LoadAsset(t1.Name + "_" + sShader) as Shader;
                        }
                        if (shader == null) {
                            shader = extraShaders.LoadAsset(t1.Name + "_" + sShader) as Shader;
                        }

                        if (shader == null) {
                            Debug.LogError("Shader " + t1.Name + " has no shader file in extrashaders.");
                            continue;
                        }
                    }

                    Util.SetFieldValue(t1, v, sShader, shader);
                }

                MethodInfo mi = t2.GetMethod("InitExtra");
                if (mi != null)
                    mi.Invoke(null, new object[] { v });
            }
            else
            {
                Util.SetPropertyValue(t1, v, "enabled", true);

                MethodInfo mi = t2.GetMethod("InitMemberByInstance");
                if (mi != null)
                    mi.Invoke(null, new object[] { v });
            }

            return v;
        }

        public static string ConvertVector3ToString(Vector3 v3)
        {
            return v3[0] + "," + v3[1] + "," + v3[2];
        }

        public static Vector3 ConvertStringToVector3(string s)
        {
            string[] sSplit = s.Split(',');
            if (sSplit.Length == 3)
            {
                float[] fTmps = new float[3];
                bool bFailure = false;
                for (int x = 0; x < 3; x++)
                {
                    if (!float.TryParse(sSplit[x], out fTmps[x]))
                        bFailure = true;
                }
                if (!bFailure)
                    return new Vector3(fTmps[0], fTmps[1], fTmps[2]);
            }
            return Vector3.zero;
        }

        public static string ConvertQuaternionToString(Quaternion q)
        {
            return q[0] + "," + q[1] + "," + q[2] + "," + q[3];
        }

        public static Quaternion ConvertStringToQuaternion(string s)
        {
            string[] sSplit = s.Split(',');
            if (sSplit.Length == 4)
            {
                float[] fTmps = new float[4];
                bool bFailure = false;
                for (int x = 0; x < 4; x++)
                {
                    if (!float.TryParse(sSplit[x], out fTmps[x]))
                        bFailure = true;
                }
                if (!bFailure)
                    return new Quaternion(fTmps[0], fTmps[1], fTmps[2], fTmps[3]);
            }
            return Quaternion.identity;
        }

        public static string ConvertColor32ToString(Color32 color)
        {
            return color.r + "," + color.g + "," + color.b + "," + color.a;
        }

        public static Color32 ConvertStringToColor32(string s)
        {
            string[] sSplit = s.Split(',');
            if (sSplit.Length == 4)
            {
                byte[] byteTmps = new byte[4];
                bool bFailure = false;
                for (int x = 0; x < 4; x++)
                {
                    if (!byte.TryParse(sSplit[x], out byteTmps[x]))
                        bFailure = true;
                }
                if (!bFailure)
                    return new Color32(byteTmps[0], byteTmps[1], byteTmps[2], byteTmps[3]);
            }
            return Color.white;
        }

        public static string ConvertAnimationCurveToString(AnimationCurve curve)
        {
            float f0 = curve.keys[0].outTangent;
            float f0v = curve.keys[0].value;
            float f1 = curve.keys[1].inTangent;
            float f1v = curve.keys[1].value;
            return f0 + "," + f0v + "," + f1 + "," + f1v;
        }

        public static AnimationCurve ConvertStringToAnimationCurve(string s)
        {
            string[] sSplit = s.Split(',');
            if (sSplit.Length == 4)
            {
                float[] fTmps = new float[4];
                bool bFailure = false;
                for (int x = 0; x < 4; x++)
                {
                    if (!float.TryParse(sSplit[x], out fTmps[x]))
                        bFailure = true;
                }
                if (!bFailure)
                {
                    Keyframe[] keys = new Keyframe[2];
                    keys[0] = new Keyframe(0f, 0f, 0f, 1f);
                    keys[0].outTangent = fTmps[0];
                    keys[0].value = fTmps[1];
                    keys[1] = new Keyframe(1f, 1f, 1f, 0f);
                    keys[1].inTangent = fTmps[2];
                    keys[1].value = fTmps[3];
                    return new AnimationCurve(keys);
                }

            }
            return new AnimationCurve();
        }
        //

        internal static FieldInfo[] GetFields<T>()
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            return typeof(T).GetFields(bf);
        }
        internal static FieldInfo[] GetFields(Type t)
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            return t.GetFields(bf);
        }

        internal static string[] GetFieldNames<T>()
        {
            FieldInfo[] fields = GetFields<T>();
            if (fields.Length == 0) return new string[0];

            string[] sRet = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                sRet[i] = fields[i].Name;
            }
            return sRet;
        }

        internal static string[] GetFieldNamesSpecifyType<T, T2>()
        {
            FieldInfo[] fields = GetFields<T>();
            if (fields.Length == 0) return new string[0];

            Type t = typeof(T2);
            List<string> sRetList = new List<string>();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == t)
                    sRetList.Add(fields[i].Name);
            }
            return sRetList.ToArray();
        }
        internal static string[] GetFieldNamesSpecifyType(Type t, Type t2)
        {
            FieldInfo[] fields = GetFields(t);
            if (fields.Length == 0) return new string[0];

            List<string> sRetList = new List<string>();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == t2)
                    sRetList.Add(fields[i].Name);
            }
            return sRetList.ToArray();
        }
        internal static FieldInfo[] GetFieldsSpecifyType(Type t, Type t2)
        {
            FieldInfo[] fields = GetFields(t);
            if (fields.Length == 0) return new FieldInfo[0];

            List<FieldInfo> sRetList = new List<FieldInfo>();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == t2)
                    sRetList.Add(fields[i]);
            }
            return sRetList.ToArray();
        }


        internal static FieldInfo GetFieldInfo<T>(string name)
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            return typeof(T).GetField(name, bf);
        }
        internal static FieldInfo GetFieldInfo(Type t, string name)
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            return t.GetField(name, bf);
        }

        internal static TResult GetFieldValue<T, TResult>(T inst, string name)
        {
            if (inst == null) return default(TResult);

            FieldInfo field = GetFieldInfo<T>(name);
            if (field == null) return default(TResult);

            return (TResult)field.GetValue(inst);
        }

        internal static void SetFieldValue<T>(object inst, string name, object val)
        {
            if (inst == null) return;

            FieldInfo field = GetFieldInfo<T>(name);
            if (field != null)
            {
                field.SetValue(inst, val);
            }
        }
        internal static void SetFieldValue(Type t, object inst, string name, object val)
        {
            if (inst == null) return;

            FieldInfo field = GetFieldInfo(t, name);
            if (field != null)
            {
                field.SetValue(inst, val);
            }
        }

        internal static PropertyInfo GetPropertyInfo<T>(string name)
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            return typeof(T).GetProperty(name, bf);
        }
        internal static PropertyInfo GetPropertyInfo(Type t, string name)
        {
            BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            return t.GetProperty(name, bf);
        }

        internal static TResult GetPropertyValue<T, TResult>(T inst, string name)
        {
            if (inst == null) return default(TResult);

            PropertyInfo property = GetPropertyInfo<T>(name);
            if (property == null) return default(TResult);

            return (TResult)property.GetValue(inst, null);
        }

        internal static void SetPropertyValue<T>(object inst, string name, object val)
        {
            if (inst == null) return;

            PropertyInfo property = GetPropertyInfo<T>(name);
            if (property != null)
            {
                property.SetValue(inst, val, null);
            }
        }
        internal static void SetPropertyValue(Type t, object inst, string name, object val)
        {
            if (inst == null) return;

            PropertyInfo property = GetPropertyInfo(t, name);
            if (property != null)
            {
                property.SetValue(inst, val, null);
            }
        }
    }
}
