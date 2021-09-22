using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        this.init();
    }

    async void init()
    {
        FairyGUI.GRoot.inst.SetContentScaleFactor(1080, 1920, FairyGUI.UIContentScaler.ScreenMatchMode.MatchWidth);
        Log.gzaLog("当前分辨率", FairyGUI.GRoot.inst.width, FairyGUI.GRoot.inst.height);

        FairyGUI.UIObjectFactory.SetLoaderExtension(typeof(LanLuLoader));
        FairyGUI.UIPackage.AddPackage("GameStart");

        GameStart.GameStartBinder.BindAll();
        var com = FairyGUI.UIPackage.CreateObject("GameStart", "GameStartView").asCom;
        FairyGUI.GRoot.inst.AddChild(com);

        com.width = FairyGUI.GRoot.inst.width;
        com.height = FairyGUI.GRoot.inst.height;
        com.AddRelation(FairyGUI.GRoot.inst, FairyGUI.RelationType.Width);
        com.AddRelation(FairyGUI.GRoot.inst, FairyGUI.RelationType.Height);

        com.MakeFullScreen();

        GameStart.UI_GameStartView.gameStartView = com as GameStart.UI_GameStartView;

        GameStart.UI_GameStartView.gameStartView.m_pb.visible = true;
        GameStart.UI_GameStartView.gameStartView.m_txt.text = "加载游戏逻辑中...";


        await TSMgr.Instance.init();
        GameStart.UI_GameStartView.gameStartView.m_txt.text = "游戏逻辑加载完毕";
        var tsMain = TSMain.Instance;
        tsMain.camera = this.gameObject.transform.parent.gameObject;
        tsMain.trueCamera = this.gameObject;

        this.initPublicAudio();
    }

    //特殊处理音效播放，之后看看要不要挪到TS
    public async void initPublicAudio()
    {
        await AddressablesMgr.Instance.loadFGUIAsset<AudioClip>("Assets/Assets/UI/Public_nqxb17.wav");
        await AddressablesMgr.Instance.loadFGUIAsset<AudioClip>("Assets/Assets/UI/Public_nqxb18.wav");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Timer.Instance.update();
    }
}
