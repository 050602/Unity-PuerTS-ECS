import { FairyGUI, System, EventCenter, Log } from "csharp";
import { UIManager } from "./UIManager";


export class BaseView extends FairyGUI.GComponent {

    public uiId: string

    public bindKeys: System.Collections.Generic.List$1<string>;

    public onInit(){}

    public onOpen(args: any = null) {
        Log.log("onOpen:" + this.uiId);

    }

    public onClose() {

    }

    public onDestroy() {
        if (this.bindKeys != null) {
            this.bindKeys = null;
        }


    }

    public close() {
        UIManager.getInstance().closeView(this.uiId);
    }

}