import { Sigleton } from "./Sigleton";
import { FairyGUI, EventCenter, Log, System, AddressablesMgr, Timer, UnityEngine } from "csharp";
import { BaseView } from "./BaseView";
import { $promise, $ref, $typeof, $unref } from "puerts";
import { ViewConst } from "./ViewConst";
import { ViewEvent } from "../event/ViewEvent";
import { TSEvent } from "csharp";
import { TSEventCenter } from "../event/TSEventCenter";
import { gzaLog } from "../utils/LogTS";
import { ObjectPool } from "./ObjectPool";
import { TimerTS } from "./TimerTS";
import { World } from "../fight/World";

export enum UILayer {
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

class UIInfo {

    public uiId: string;
    public layer: UILayer;
    public cache: boolean;

    public isClose: boolean;
    public isOpening: boolean;
    public isTsView: boolean;
    public args1: any;

    constructor(uiId: string, layer: UILayer, cache: boolean, args1: any = null, isTsView: boolean = false, isOpening: boolean = true, isClose: boolean = false) {
        this.uiId = uiId;
        this.layer = layer;
        this.cache = cache;
        this.isClose = isClose;
        this.isOpening = isOpening;
        this.isTsView = isTsView;
        this.args1 = args1;
    }

}

export class UIManager extends Sigleton {
    static getInstance(): UIManager {
        return super.getInstance(UIManager);
    }


    public initInstance() {

        for (let key in UILayer) {
            let keyToAny: any = key;
            if (isNaN(keyToAny)) {

                let typeNum: any = UILayer[key];
                let layerName: UILayer = typeNum;
                let gc = new FairyGUI.GComponent();
                gc.name = layerName.toString();
                gc.gameObjectName = layerName.toString();
                gc.width = FairyGUI.GRoot.inst.width;
                gc.height = FairyGUI.GRoot.inst.height;
                gc.AddRelation(FairyGUI.GRoot.inst, FairyGUI.RelationType.Width);
                gc.AddRelation(FairyGUI.GRoot.inst, FairyGUI.RelationType.Height);
                gc.MakeFullScreen();
                FairyGUI.GRoot.inst.AddChild(gc);

                this.layerDic.set(layerName, gc);
            }
        }

        //把摇杆移动到指定层级  移动到太前的位置，会导致碰不到UI，暂时先不实现吧
        // for (let i = 0; i < FairyGUI.GRoot.inst.GetChildren().Length; i++) {
        //     let joy = FairyGUI.GRoot.inst.GetChildren().get_Item(i);
        //     if(joy.gameObjectName == "Joy"){
        //         this.layerDic.get(UILayer.Tips).AddChild(joy);
        //         break;
        //     }
        // }


        this.layerDic.get(UILayer.Fight).fairyBatching = true;
        this.layerDic.get(UILayer.FightUI).fairyBatching = true;

        this.initLoading();

        // TSEventCenter.getInstance().bind(FightEvent.FLOAT_TEXT, this, this.floatText);
        // TSEventCenter.getInstance().bind(FightEvent.FLOAT_TEXT_NORAML, this, this.floatTextNormal);

    }

    private async initLoading() {
        await this.addPackage("Loading");
        this.loading = FairyGUI.UIPackage.CreateObject("Loading", "LoadingView").asCom;
        this.layerDic.get(UILayer.Loading).AddChild(this.loading);

        this.loading.x = (FairyGUI.GRoot.inst.width - this.loading.width) * 0.5;
        this.loading.y = (FairyGUI.GRoot.inst.height - this.loading.height) * 0.5;
        this.loading.visible = false;
    }

    public dic: Map<string, UIInfo> = new Map();//key是uiid
    public viewCacheDic: Map<string, BaseView> = new Map<string, BaseView>();//key是uiid

    public layerDic: Map<UILayer, FairyGUI.GComponent> = new Map<UILayer, FairyGUI.GComponent>();
    public packageCache: string[] = [];

    public stackList: string[] = [];

    public loadingList: string[] = [];

    public loading: FairyGUI.GComponent;

    // public viewCacheDic: Map<string, TSBaseView>;


    public getLayerByUILayer(layer: UILayer): FairyGUI.GComponent {
        var gcom = this.layerDic.get(layer)
        return gcom;
    }


    public showLoading(dt: number) {

        if (this.loadingList.length > 0) {
            this.loading.visible = true;
        }
        else {
            this.loading.visible = false;
            TimerTS.getInstance().clear(this, this.showLoading);
        }
    }


    public setLoading(uiId: string) {
        if (!this.loading) {
            return;
        }

        let index = this.loadingList.indexOf(uiId);
        if (index != -1) {
            Log.logWarning("指定界面已经进入Loading:" + uiId);
            return;
        }

        this.loadingList.push(uiId);
        TimerTS.getInstance().once(this, this.showLoading, 0.35);


    }


    public clearLoading(uiId: string) {

        let index = this.loadingList.indexOf(uiId);
        this.loadingList.splice(index, 1);

        if (this.loadingList.length == 0) {
            this.loading.visible = false;
            TimerTS.getInstance().clear(this, this.showLoading);
        }
    }

    /**
    *@param name 包名
    */
    public async addPackage(name: string) {
        if (this.packageCache.indexOf(name) != -1) {
            return true;
        }

        var path = "Assets/Assets/UI/" + name + "_fui.bytes";
        if (AddressablesMgr.Instance.getFGUIAsset(path) == null) {
            let task = AddressablesMgr.Instance.loadFGUITextAsset(path);
            await $promise(task);
            // int[] png;
            var len = ViewConst.pngStrDic.get(name);
            if (len) {
                let task2 = AddressablesMgr.Instance.loadFGUITexture("Assets/Assets/UI/" + name + "_atlas0.png");
                await $promise(task2);
                for (var i = 1; i < len; i++) {
                    let task3 = AddressablesMgr.Instance.loadFGUITexture("Assets/Assets/UI/" + name + "_atlas0_" + i + ".png");
                    await $promise(task3);
                }
            }

        }

        FairyGUI.UIPackage.AddPackage(name, AddressablesMgr.LanLuLoadFunc);
        this.packageCache.push(name);

        return true;

    }

    public clearPackage(name: string) {
        // UIPackage.RemovePackage("Assets/Assets/UI/" + name);
        FairyGUI.UIPackage.RemovePackage(name);

        let path = "Assets/Assets/UI/" + name + "_fui.bytes";
        let obj: UnityEngine.TextAsset = AddressablesMgr.Instance.getFGUIAsset(path) as UnityEngine.TextAsset;
        if (obj != null) {
            AddressablesMgr.Instance.releaseTextAsset(obj);

            // int[] png;
            var len = ViewConst.pngStrDic.get(name);
            if (len) {
                let tx1 = AddressablesMgr.Instance.getFGUIAsset("Assets/Assets/UI/" + name + "_atlas0.png") as UnityEngine.Texture;
                AddressablesMgr.Instance.releaseTexture(tx1);
                for (var i = 1; i < len; i++) {
                    let tx2 = AddressablesMgr.Instance.getFGUIAsset("Assets/Assets/UI/" + name + "_atlas0_" + i + ".png") as UnityEngine.Texture;
                    AddressablesMgr.Instance.releaseTexture(tx2);
                }
            }

        }

        let index = this.packageCache.indexOf(name);
        this.packageCache.splice(index, 1);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="uiId"></param>
    /// <param name="throwWarn">如果界面不存在，是否抛出警告</param>
    public closeView(uiId: string, throwWarn = true) {
        let lastinfo = this.dic.get(uiId);
        if (lastinfo) {
            if (!lastinfo.isTsView) {
                gzaLog("尝试关闭非TS的VIew", uiId);
                return;
            }
            let view = this.viewCacheDic.get(uiId);
            if (view) {
                if (lastinfo.cache) {
                    view.visible = false;
                    lastinfo.isClose = true;
                    view.onClose();
                    // inst.stackList.Remove(uiId);
                    let index = this.stackList.indexOf(uiId);
                    if (index > -1) {
                        this.stackList.splice(index, 1);
                    }
                    return;
                } else {
                    view.onClose();
                    view.onDestroy();
                    view.Dispose();
                    let index = this.stackList.indexOf(uiId);
                    if (index > -1) {
                        this.stackList.splice(index, 1);
                    }
                    this.dic.delete(uiId);
                    this.viewCacheDic.delete(uiId);

                }

                EventCenter.Instance.eventFunc(ViewEvent.VIEW_CLOSE, uiId);
                TSEventCenter.getInstance().event(ViewEvent.VIEW_OPEN, uiId);
            } else {
                Log.logWarning("尝试关闭不存在的界面:" + uiId);
            }

        }
        else {
            if (throwWarn) {
                Log.logWarning("尝试关闭不存在的界面:" + uiId);
            }
        }
    }

    /**
   *预加载界面
   */
    public async preLoadView(uiId: string, layer: UILayer) {
        let pkgname = ViewConst.pkgNameDic.get(uiId);
        if (pkgname) {
            let task = await this.addPackage(pkgname);

            let comName = ViewConst.comNameDic.get(uiId);
            if (comName) {
                let info = new UIInfo(uiId, layer, true, null, true);
                this.dic.set(uiId, info);

                gzaLog("preLoadView", uiId, info.isTsView);


                FairyGUI.UIPackage.CreateObjectAsync(pkgname, comName, this.preloadViewOver);
            } else {
                Log.logWarning("尝试打开不存在的界面 找不到组件 界面ID:" + uiId);
            }
        } else {
            Log.logWarning("尝试打开不存在的界面 找不到组件 界面ID:" + uiId);
        }

    }

    public async openView(uiId: string, layer: UILayer, isCache: boolean = false, args1: any = null) {
        let lastinfo: UIInfo = this.dic.get(uiId);
        if (lastinfo) {
            if (!lastinfo.isTsView) {
                gzaLog("尝试打开非TS的VIew", uiId);
                return;
            }

            if (lastinfo.isOpening) {
                gzaLog("界面:" + uiId + "正在打开中，触发重复打开");
                return;
            }

            if (lastinfo.cache) {
                let view = this.viewCacheDic.get(uiId);
                if (view) {
                    view.visible = true;
                    lastinfo.isClose = false;
                    let layerCom = this.layerDic.get(lastinfo.layer);
                    if (layerCom) {
                        layerCom.AddChild(view);
                        this.stackList.push(uiId);
                        lastinfo.args1 = args1;
                        if (ViewConst.scaleDic.has(uiId)) {
                            if (FairyGUI.GRoot.inst.width < FairyGUI.GRoot.inst.height) {
                                //宽是小的，所以是正常的竖屏
                                view.scaleX = 1;
                                view.scaleY = 1;
                                view.SetSize(FairyGUI.GRoot.inst.width, FairyGUI.GRoot.inst.height);
                            } else {
                                //高是小的，因此是横屏，由于我们目前的界面都是竖的，因此需要按比例缩放
                                let percent = FairyGUI.GRoot.inst.height / FairyGUI.GRoot.inst.width;
                                view.scaleX = percent;
                                view.scaleY = percent;
                                view.SetSize(FairyGUI.GRoot.inst.width / percent, FairyGUI.GRoot.inst.height / percent);
                            }
                        }


                        view.onOpen(args1);

                        EventCenter.Instance.eventFunc(ViewEvent.VIEW_OPEN, uiId);
                        return;
                    }
                }
            }
        } else {
            let pkgname = ViewConst.pkgNameDic.get(uiId);
            if (pkgname) {
                this.setLoading(uiId);
                let task = await this.addPackage(pkgname);
                let comName = ViewConst.comNameDic.get(uiId);
                if (comName) {
                    if (!this.dic.has(uiId)) {
                        let info = new UIInfo(uiId, layer, isCache, args1, true);
                        this.dic.set(uiId, info);
                    }
                    FairyGUI.UIPackage.CreateObjectAsync(pkgname, comName, this.loadViewOver);
                } else {
                    Log.logWarning("尝试打开不存在的界面 找不到组件 界面ID:" + uiId);
                    this.clearLoading(uiId);
                }
            } else {
                Log.logWarning("尝试打开不存在的界面 找不到包 界面ID:" + uiId);
            }
        }
    }

    public loadViewOver(obj: FairyGUI.GObject) {
        let com = obj.asCom;
        let url = obj.resourceURL;
        let thisobj = UIManager.getInstance();
        let uiId = ViewConst.resURLDic.get(url);
        if (uiId) {
            let info = thisobj.dic.get(uiId)
            if (info) {
                if (!info.isTsView) {
                    gzaLog("加载完毕非TS的VIew", uiId);
                    return;
                }
                if (!info.isOpening) {
                    //该界面已被关闭
                    if (!info.cache) {
                        obj.Dispose();
                    }
                    return;
                }
                info.isOpening = false;
                info.isClose = false;

                let layer = thisobj.layerDic.get(info.layer);
                if (layer) {
                    layer.AddChild(com);
                    let baseview = com as BaseView;
                    if (baseview == null) {
                        Log.logError("没有绑定扩展类，XXXBinder!");
                        return;
                    }
                    baseview.uiId = uiId;
                    baseview.MakeFullScreen();

                    thisobj.viewCacheDic.set(uiId, baseview);

                    if (ViewConst.scaleDic.has(uiId)) {
                        if (FairyGUI.GRoot.inst.width < FairyGUI.GRoot.inst.height) {
                            //宽是小的，所以是正常的竖屏
                            baseview.scaleX = 1;
                            baseview.scaleY = 1;
                            // baseview.x = 0;
                            // baseview.y = 0;
                            baseview.SetSize(FairyGUI.GRoot.inst.width, FairyGUI.GRoot.inst.height);
                        } else {
                            //高是小的，因此是横屏，由于我们目前的界面都是竖的，因此需要按比例缩放
                            let percent = FairyGUI.GRoot.inst.height / FairyGUI.GRoot.inst.width;
                            baseview.scaleX = percent;
                            baseview.scaleY = percent;
                            baseview.SetSize(FairyGUI.GRoot.inst.width / percent, FairyGUI.GRoot.inst.height / percent);
                        }
                    }

                    baseview.onInit();
                    baseview.onOpen(info.args1);
                    // baseview.SetSize(FairyGUI.GRoot.inst.width, FairyGUI.GRoot.inst.height);
                    thisobj.stackList.push(uiId);
                    EventCenter.Instance.eventFunc(ViewEvent.VIEW_OPEN, uiId);
                    TSEventCenter.getInstance().event(ViewEvent.VIEW_OPEN, uiId);
                    thisobj.clearLoading(uiId);
                }
            }
        } else {
            Log.logWarning(url + "指定资源URL未注册");
        }

    }


    //预加载时，不允许界面传参
    public preloadViewOver(obj: FairyGUI.GObject) {

        let com = obj.asCom;
        let url = obj.resourceURL;
        let thisobj = UIManager.getInstance();
        let uiId = ViewConst.resURLDic.get(url);
        // gzaLog(thisobj,thisobj.dic,this);
        if (uiId) {
            let info = thisobj.dic.get(uiId);
            if (info) {
                if (!info.isTsView) {
                    gzaLog("预加载完毕非TS的VIew", uiId);
                    return;
                }
                info.isOpening = false;
                info.isClose = true;
                // let layer = this.layerDic.get(info.layer);

                let cacheView = thisobj.viewCacheDic.get(uiId);
                // gzaLog("perloadViewOver", inst.viewCacheDic);
                // let cacheView = inst.viewCacheDic.get(uiId);
                if (cacheView) {
                    gzaLog("预加载完毕时，界面已打开：" + uiId);
                    return;
                }

                com.visible = false;

                let baseview = com as BaseView;
                baseview.uiId = uiId;
                baseview.MakeFullScreen();
                baseview.onInit();
                gzaLog("预加载完毕 ID：", uiId, baseview.uiId);


                thisobj.viewCacheDic.set(uiId, baseview);

                EventCenter.Instance.eventFunc(ViewEvent.VIEW_PRELOAD_OVER, uiId);
                TSEventCenter.getInstance().event(ViewEvent.VIEW_PRELOAD_OVER, uiId);
            }
        } else {
            Log.logWarning(url + "指定资源URL未注册");
        }
    }


}