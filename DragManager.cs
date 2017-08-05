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

        public bool Drag()
        {
            if (goDrag == null)
                return false;

            if (!canDrag)
                return false;

            if (Input.GetMouseButtonDown(0))
            {
                inDrag = ChkObjectAndMouseOffsset();
            }
            if (Input.GetMouseButtonUp(0))
            {
                inDrag = false;
            }
            if (inDrag)
            {
                Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, v3Screen.z);
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + v3Offset;

                goDrag.transform.position = currentPosition;
            }

            return inDrag;
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
            goDrag.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            goDrag.GetComponent<Renderer>().material.color = new Color32(255, 192, 192, 192);
        }

        public void StopDrag()
        {
            if (goDrag == null)
                return;

            canDrag = false;
            goDrag.transform.localScale = Vector3.zero;
            goDrag.GetComponent<Renderer>().material.color = new Color32(255, 192, 192, 0);
        }

        public void DeleteObject()
        {
            if (goDrag != null)
                UnityEngine.Object.Destroy(goDrag);
        }
    }
}
