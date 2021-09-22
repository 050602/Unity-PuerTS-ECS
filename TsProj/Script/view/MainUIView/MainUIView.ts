import { FairyGUI, Log, UnityEngine } from "csharp";
import { BaseView } from "../../core/BaseView";
import { EventCenter } from "csharp";

export class MainUIView extends BaseView {

    // public toFight: FairyGUI.GButton;
    // public backMain: FairyGUI.GButton;
    public btn_upLevel: FairyGUI.GLoader;
    public loader_2: FairyGUI.GLoader;
    public loader_1: FairyGUI.GLoader;
    public progressBar: FairyGUI.GProgressBar;
    public txt_percent: FairyGUI.GTextField;
    public txt_1: FairyGUI.GTextField;
    public txt_2: FairyGUI.GTextField;
    public txt_level: FairyGUI.GTextField;
    // public levelGroup: FairyGUI.GGroup;
    public levelUpClick: FairyGUI.GObject;
    public btn_clear: FairyGUI.GButton;
    public img_levelUp: FairyGUI.GComponent;
    public static URL: string = "ui://mbhw74n8hzlo0";


    constructor() {
        super();
    }

    public static createInstance(): MainUIView {
        return <MainUIView>(FairyGUI.UIPackage.CreateObject("MainUI", "MainUIView"));
    }

    public onInit() {
        this.levelUpClick = this.GetChild("levelUpClick");
        this.loader_1 = <FairyGUI.GLoader>(this.GetChild("loader_1"));
        this.loader_2 = <FairyGUI.GLoader>(this.GetChild("loader_2"));
        this.progressBar = <FairyGUI.GProgressBar>(this.GetChild("progressBar"));
        this.btn_upLevel = <FairyGUI.GLoader>(this.GetChild("btn_upLevel"));
        this.txt_1 = <FairyGUI.GTextField>(this.GetChild("txt_1"));
        this.txt_2 = <FairyGUI.GTextField>(this.GetChild("txt_2"));
        this.txt_level = <FairyGUI.GTextField>(this.GetChild("txt_level"));
        this.txt_percent = <FairyGUI.GTextField>(this.GetChild("txt_percent"));
        this.btn_clear = <FairyGUI.GButton>(this.GetChild("btn_clear"));
        this.img_levelUp = <FairyGUI.GComponent>(this.GetChild("img_levelUp"));
    }

    public onOpen(args1: any) {
        super.onOpen(args1);

        let ins: EventCenter = EventCenter.Instance;

    }




    public onClose() {
        super.onClose();

        this.levelUpClick.onClick.Clear();
        this.btn_clear.onClick.Clear();
        this.img_levelUp.onClick.Clear();

    }

    public onDestroy() {
        super.onDestroy();
    }
}