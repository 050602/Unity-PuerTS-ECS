import { EventCenter, Log, System, TSEvent } from "csharp";
import { Sigleton } from "../core/Sigleton";
import { UIManager } from "../core/UIManager";

export class TSEventCenter extends Sigleton {
    static getInstance(): TSEventCenter {
        return super.getInstance(TSEventCenter);
    }

    initInstance() {
        let inst: EventCenter = EventCenter.Instance;
        inst.bind(TSEvent.CLOSE_VIEW, this, this.closeView);
        inst.bind(TSEvent.PERLOAD_VIEW, this, this.perloadView);
        inst.bind(TSEvent.OPEN_VIEW, this, this.openView);
    }

    private map: Map<string, any[]> = new Map();

    public bind(name: string, thisobj: any, func: Function) {
        let arr = this.map.get(name);
        if (arr) {
            let len = arr.length;
            for (let i = 0; i < len; i++) {
                if (arr[i][0] == func && arr[i][1] == thisobj) {
                    Log.logWarning("重复注册事件", name);
                    return;
                }
            }
        } else {
            this.map.set(name, []);
            arr = this.map.get(name);
        }
        arr.push([func, thisobj]);
    }


    public unbind(name: string, thisobj: any) {
        let arr = this.map.get(name);
        if (arr) {
            let len = arr.length;
            for (let i = len - 1; i >= 0; i--) {
                if (arr[i][1] == thisobj) {
                    arr.splice(i, 1);
                }
            }
        }
        if (arr && arr.length == 0) {
            this.map.delete(name);
        }
    }


    public event(name: string, ...data: any[]) {
        let arr = this.map.get(name);
        if (arr) {
            for (let i = arr.length - 1; i >= 0; i--) {
                let f: Function = arr[i][0];
                f.apply(arr[i][1], data);
            }
        }
    }

    public closeView(obj: System.Object, args: System.Array$1<System.Object>) {
        let uiid2: any = args.get_Item(0);
        UIManager.getInstance().closeView(uiid2);
    }

    public perloadView(obj: System.Object, args: System.Array$1<System.Object>) {
        let uiid: any = args.get_Item(0);
        let obj2: any = args.get_Item(1);
        UIManager.getInstance().preLoadView(uiid, obj2);
    }

    public openView(obj: System.Object, args: System.Array$1<System.Object>) {
        let uiid2: any = args.get_Item(0);
        let obj2: any = args.get_Item(1);
        let iscache: any = args.get_Item(2);
        let args1: any;
        if (args.Length > 3)
            args1 = args.get_Item(3);
        UIManager.getInstance().openView(uiid2, obj2, iscache, args1);
    }

}