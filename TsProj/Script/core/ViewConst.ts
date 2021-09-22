import { System, Log } from "csharp";
import MainUIBinder from "../view/MainUIView/MainUIBinder";
import { $typeof } from "puerts";
import { MainUIView } from "../view/MainUIView/MainUIView";

export class ViewConst {
    public static MainUIView_ID: string = "MainUIView";
    public static LevelUpDialogView_ID: string = "LevelUpDialogView";
    public static MagicChangeDialog_ID: string = "MagicChangeDialog";
    public static MagicEditDialog_ID: string = "MagicEditDialog";
    public static WeaponLevelUpDialog_ID: string = "WeaponLevelUpDialog";
    public static RewardView_ID: string = "RewardView";


    public static pkgNameDic: Map<string, string> = new Map<string, string>();
    public static comNameDic: Map<string, string> = new Map<string, string>();
    public static resURLDic: Map<string, string> = new Map<string, string>();
    public static pngStrDic: Map<string, number> = new Map<string, number>(); //包名 - 图集PNG的数量

    public static scaleDic: Map<string, boolean> = new Map();//组件名ID  是否根据横竖屏进行缩放

    public static init() {

        ViewConst.pngStrDic.set("Loading", 1);
        ViewConst.initMainUI();
    }

    private static initMainUI() {
        //==============包名注册（组件名，包名）=======================
        ViewConst.pkgNameDic.set(ViewConst.MainUIView_ID, "MainUI");

        ViewConst.pngStrDic.set("MainUI", 1);
        ViewConst.pngStrDic.set("SkillJoy", 1);
        ///此处是包名，对应包的图集atls0.png 1 2 3 如果该包，没有图集，请不要赋值

        //===============组件名注册==============
        //一般直接使用组件ID作为组件名的key
        ViewConst.comNameDic.set(ViewConst.MainUIView_ID, ViewConst.MainUIView_ID);
        //===============组件URL注册+================
        ViewConst.resURLDic.set(MainUIView.URL, ViewConst.MainUIView_ID);
        //===============绑定扩展类===================
        MainUIBinder.BindAll();
    }

}
