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
    internal class MaidManager
    {
        public List<Maid> listMaid { get; set; }
        public List<Transform> listTrs { get; set; }
        public List<string> listName { get; set; }
        public int iCurrent { get; set; }
        public string sCurrent { get; set; }

        public bool bUpdateRequest { get; set; }
        public bool bFade { get; set; }

        private CharacterMgr cm { get; set; }
        private CameraMain cameraMain { get; set; }

        public MaidManager()
        {
            listMaid = new List<Maid>();
            listTrs = new List<Transform>();
            listName = new List<string>();

            iCurrent = -1;
            sCurrent = "- - -";

            cm = GameMain.Instance.CharacterMgr;
            cameraMain = GameMain.Instance.MainCamera;

            bUpdateRequest = true;
        }

        public void Find()
        {
            bUpdateRequest = false;

            int iCount = listMaid.Count;
            listMaid.Clear();
            listTrs.Clear();
            listName.Clear();

            Maid maid;
            for (int i = 0; i < cm.GetMaidCount(); i++)
            {
                maid = cm.GetMaid(i);
                if (MaidManager.IsValid(maid))
                    listMaid.Add(maid);

            }
            List<Maid> _listStockMaid = cm.GetStockMaidList();

            for (int i = 0; i < _listStockMaid.Count; i++)
            {
                if (MaidManager.IsValid(_listStockMaid[i]) && !listMaid.Contains(_listStockMaid[i]))
                    listMaid.Add(_listStockMaid[i]);

            }

            if (listMaid.Count == 0)
            {
                iCurrent = -1;
                sCurrent = "- - -";
                this.OnMaidManagerFind();

                return;
            }

            if (iCurrent >= listMaid.Count)
            {
                iCurrent = 0;
            }
            else if(iCurrent < 0)
            {
                iCurrent = listMaid.Count - 1;
            }

            for(int i = 0; i < listMaid.Count; i++)
            {
                listName.Add(listMaid[i].Param.status.last_name + " " + listMaid[i].Param.status.first_name);
                listTrs.Add(listMaid[i].body0.trsHead);
            }
            sCurrent = listName[iCurrent];

            this.OnMaidManagerFind();
            return;
        }

        private void OnMaidManagerFind()
        {
            DepthOfFieldDef.OnMaidManagerFind(this);
            BokehDef.OnMaidManagerFind(this);
        }

        public Transform GetTransform()
        {
            return iCurrent >= 0 ? listTrs[iCurrent] : null;
        }

        public void Prev()
        {
            if (listMaid.Count == 0)
                return;

            if(--iCurrent < 0)
                iCurrent = listMaid.Count - 1;

            sCurrent = listName[iCurrent];
        }

        public void Next()
        {
            if (listMaid.Count == 0)
                return;

            if (++iCurrent >= listMaid.Count)
                iCurrent = 0;

            sCurrent = listName[iCurrent];
        }

        public bool Select(int iNum)
        {
            if (listMaid.Count == 0 || iNum < 0)
                return false;

            while(iNum >= listMaid.Count)
            {
                iNum--;
            }

            iCurrent = iNum;
            sCurrent = listName[iCurrent];

            return true;
        }

        public void Clear()
        {
            listMaid.Clear();
            listTrs.Clear();
            listName.Clear();
            iCurrent = -1;
            sCurrent = string.Empty;
        }

        public void Update()
        {
            if(cameraMain.IsFadeProc())
            {
                bFade = true;
            }
            if(bFade && cameraMain.IsFadeStateNon())
            {
                bFade = false;
                bUpdateRequest = true;
            }
            if (bUpdateRequest)
            {
                Find();
            }
        }

        public static bool IsValid(Maid m)
        {
            return m != null && m.body0.trsHead != null && m.Visible;
        }
    }
}
