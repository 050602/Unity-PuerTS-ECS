import { AddressablesMgr, GameStart, Log, Timer, TSMain, UnityEngine } from "csharp";
import { ViewConst } from "../core/ViewConst";
import { MainUIView } from "./MainUIView/MainUIView";
import { UILayer, UIManager } from "../core/UIManager";
import { TSEventCenter } from "../event/TSEventCenter";
import { TimerTS } from "../core/TimerTS";
import { ViewEvent } from "../event/ViewEvent";
import { ECSManager } from "../core/ecs/base/ECSManager";
import { World } from "../fight/World";



export let __cacheDic: Map<string, any> = new Map();
function initTS() {
    //初始化界面定义
    ViewConst.init();
    let event = TSEventCenter.getInstance();
    let tsUIMgr = UIManager.getInstance();
    let timer = TimerTS.getInstance();
    let ecs = ECSManager.getInstance();

    let loa = new loading();
    loa.loadingConfig();
}

class loading {

    public async loadingConfig() {

        let view = GameStart.UI_GameStartView.gameStartView;
        view.m_txt.text = "加载游戏配置中...";


        this.loadingConfigOver();
    }

    //加载界面
    public async loadingConfigOver() {
        let view = GameStart.UI_GameStartView.gameStartView;
        view.m_pb.value += 16;

        view.m_txt.text = "加载主界面中...";
        await UIManager.getInstance().addPackage("FloatFont");
        await UIManager.getInstance().addPackage("Public");

        //加载界面
        let tsUIMgr = UIManager.getInstance();
        tsUIMgr.preLoadView(ViewConst.MainUIView_ID, UILayer.Main);

        let event = TSEventCenter.getInstance();
        event.bind(ViewEvent.VIEW_PRELOAD_OVER, this, this.loadingScene);
        



    }

    public async loadingScene() {
        let view = GameStart.UI_GameStartView.gameStartView;
        view.m_txt.text = "加载游戏场景中...";
        view.m_pb.value += 16;
        let model = new UnityEngine.SceneManagement.LoadSceneParameters(UnityEngine.SceneManagement.LoadSceneMode.Additive);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1, model);
        view.m_pb.value += 16;

        TimerTS.getInstance().once(this, this.loadingSceneOver, 0.1)

    }


    public async loadingSceneOver() {

        let view = GameStart.UI_GameStartView.gameStartView;
        view.m_pb.value += 16;
        view.m_txt.text = "加载世界中...";
        
        World.getInstance().init();

        this.loadingViewOver();
    }

    public loadingViewOver() {
        TSEventCenter.getInstance().unbind(ViewEvent.VIEW_PRELOAD_OVER, this);

        //加载完毕后销毁加载界面
        let view = GameStart.UI_GameStartView.gameStartView;
        view.m_pb.value += 16;

        this.loadingOver();
    }

    public loadingOver() {
        let view = GameStart.UI_GameStartView.gameStartView;

        view.m_btn.visible = true;
        view.m_pb.visible = false;
        view.m_btn.onClick.Add(() => { this.enterMain() });
        view.m_txt.text = "点击进入游戏";

    }

    public enterMain() {
        let view = GameStart.UI_GameStartView.gameStartView;
        UIManager.getInstance().openView(ViewConst.MainUIView_ID, UILayer.Main);

        view.parent.RemoveChild(view);
        GameStart.UI_GameStartView.gameStartView = null;
    }
}


initTS();


