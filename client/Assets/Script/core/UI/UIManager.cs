using System;
using FairyGUI;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public enum UILayer
{
    Background = 0,
    Main,//主界面
    Module,//功能UI 装备 神器
    Pop,//弹出窗口
    Fight,//战斗实体，显示飘字之类
    FightUI,//战斗内所有界面
    Tips,//提示
    Top,//顶层
    Loading,//加载层
    Cmd,//GM指令界面
    Toast,
}

public class UIInfo
{

    public string uiId;
    public UILayer layer;
    public bool cache;

    public bool isClose;
    public bool isOpening;
    public bool isTsView;
    public object args1;

    public UIInfo(string uiId, UILayer layer, bool cache, object args1 = null, bool isTsView = false, bool isOpening = true, bool isClose = false)
    {
        this.uiId = uiId;
        this.layer = layer;
        this.cache = cache;
        this.isClose = isClose;
        this.isOpening = isOpening;
        this.isTsView = isTsView;
        this.args1 = args1;
    }

}

public class UIManager : Singleton<UIManager>
{
    public Dictionary<string, UIInfo> dic = new Dictionary<string, UIInfo>();//key是uiid
    public Dictionary<string, object> viewCacheDic = new Dictionary<string, object>();//key是uiid

    public Dictionary<UILayer, GComponent> layerDic = new Dictionary<UILayer, GComponent>();
    public List<string> packageCacge = new List<string>();

    public List<string> loadingList;

    public List<string> stackList;

    public int curToastCount = 0;

    public GComponent getLayerByUILayer(UILayer layer)
    {
        var gcom = default(GComponent);
        this.layerDic.TryGetValue(layer, out gcom);
        return gcom;
    }


    public bool isTopUI(string uiId)
    {
        if (this.stackList.Count == 0)
        {
            return false;
        }

        return this.stackList[this.stackList.Count - 1] == uiId;
    }

}