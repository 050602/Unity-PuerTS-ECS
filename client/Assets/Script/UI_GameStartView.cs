/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using System;
using UnityEngine;

namespace GameStart
{
    public partial class UI_GameStartView : GComponent
    {

        public static GameStart.UI_GameStartView gameStartView;
        public GProgressBar m_pb;
        public GTextField m_txt;
        public GButton m_btn;
        public const string URL = "ui://rsx1bfmj7fhc1";

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            m_pb = (GProgressBar)GetChild("pb");
            m_txt = (GTextField)GetChild("txt");
            m_btn = (GButton)GetChild("btn");

            this.m_pb.max = 100;
            this.m_pb.value = 0;


            this.m_btn.visible = false;
            this.m_txt.text = "正在检查更新...";

        }
    }
}