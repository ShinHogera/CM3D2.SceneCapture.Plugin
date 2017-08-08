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
    internal class DragManager
    {
        public bool canDrag { get; set; }
        private bool inDrag { get; set; }
        public GameObject goDrag { get; set; }
        private Vector3 v3Screen { get; set; }
        private Vector3 v3Offset { get; set; }

        public DragManager()
        {
            goDrag = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            goDrag.transform.position = new Vector3(0f, 1.5f, -1f);
            goDrag.transform.localScale = Vector3.zero;
            goDrag.GetComponent<Renderer>().material.color = new Color32(255, 192, 192, 0);

            UnityEngine.Object.DontDestroyOnLoad(goDrag);
        }

        ~DragManager()
        {
            DeleteObject();
        }

        public void SetTransform(Transform trans)
        {
            this.goDrag.transform.position = trans.position;
        }

        private bool ChkObjectAndMouseOffsset()
        {
            v3Screen = Camera.main.WorldToScreenPoint(goDrag.transform.position);
            v3Offset = goDrag.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, v3Screen.z));

            float fObjectSize = goDrag.transform.lossyScale.x / 2;
            if (Mathf.Abs(v3Offset.x) > fObjectSize || Mathf.Abs(v3Offset.y) > fObjectSize)
                return false;

            return true;
        }

        public void StartDrag()
        {
            if (goDrag == null)
                return;

            canDrag = true;
            goDrag.GetComponent<Renderer>().material.color = new Color32(255, 192, 192, 192);

            if(goDrag.GetComponent<GizmoRenderTarget>() == null)
                goDrag.AddComponent<GizmoRenderTarget>();

            goDrag.GetComponent<GizmoRenderTarget>().Visible = true;
            goDrag.GetComponent<GizmoRenderTarget>().eRotate = false;
            goDrag.GetComponent<GizmoRenderTarget>().eAxis = true;
            goDrag.GetComponent<GizmoRenderTarget>().target_trans = this.goDrag.transform;
            goDrag.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        }

        public void StopDrag()
        {
            if (goDrag == null)
                return;

            canDrag = false;
            goDrag.transform.localScale = Vector3.zero;
            goDrag.GetComponent<Renderer>().material.color = new Color32(255, 192, 192, 0);

            if(goDrag.GetComponent<GizmoRenderTarget>() != null)
                UnityEngine.Object.Destroy(goDrag.GetComponent<GizmoRenderTarget>());
        }

        public void DeleteObject()
        {
            if (goDrag != null)
                UnityEngine.Object.Destroy(goDrag);
        }
    }
}
