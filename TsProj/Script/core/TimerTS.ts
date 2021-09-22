import { Sigleton } from "./Sigleton"
import { Log, EventCenter, TSEvent, System, TSMain } from "csharp";
import { gzaLog } from "../utils/LogTS";
import { GameUpdateCD } from "./GameConst";

export interface TimerObj {
    callback: Function; // 回调
    thisObj: any;        // 目标 上下文
    interval: number;   // 间隔
    repeat: number;     // 重复次数
    accTime: number;    // 累计时长
    delay: number;      // 延迟
    uid: number;//唯一ID
}


export class TimerTS extends Sigleton {

    static getInstance(): TimerTS {
        return super.getInstance(TimerTS);
    }

    public initInstance() {
        EventCenter.Instance.bind(TSEvent.TICK, this, this.update);
    }

    private static _isRunning: boolean = false;
    private static _arr: TimerObj[] = [];
    private static _pause: boolean = false;//是否暂停

    private uid: number = 1;

    //获取唯一ID
    public getUID(): number {
        return this.uid++;
    }


    public update(systobj: System.Object, args: System.Array$1<System.Object>) {
        // let dt: any = args.get_Item(0);
        if (TimerTS._pause) return;

        //隔0.1秒执行一次 执行3次
        let arr = TimerTS._arr;
        for (let i = 0; i < arr.length; i++) {
            let dt: number = GameUpdateCD;
            // let dt: any = args.get_Item(0);
            let obj: TimerObj = arr[i];

            obj.accTime += dt;  //0+0.033 +0.033 = 0.066 +0.033 = 0.1

            if (obj.delay) {
                let delay = obj.delay;
                //有延时 
                if (obj.accTime >= delay) {
                    obj.accTime = 0;
                    obj.delay = 0;
                }
            } else {
                let func: Function = obj.callback;
                let thisObj: any = obj.thisObj;
                let interval: number = obj.interval;

                if (obj.accTime >= interval) {    //0.033> 0.1? 0.066>0.1? 0.1>=0.1
                    obj.accTime -= interval;
                    if (interval != 0) {
                        dt = interval;
                    }
                    if (obj.repeat > 0) {        //3 2 1
                        obj.repeat--;            //2 1 0
                        func.apply(thisObj, [dt]);     //执行一次 2 3
                        if (obj.repeat == 0) {   //第三次时，等于0
                            arr.splice(i, 1);   //被销毁
                            i--;
                        }
                    } else {
                        func.apply(thisObj, [dt]);
                    }
                }

            }
        }
    }





    /**
     *  监听后不会马上触发，直到经过一次间隔, 除非runNow为true
        如果回调函数已调度，那么将不会重复调度它，只会更新时间间隔参数。=== 即不能重复注册同一个对象 同一个方法
        @param thisObj 方法对应的对象 
        @param callback 每次触发的回调方法 回调参数 dt：number 单位秒
        @param interval 间隔多少秒. 0 则每帧调用 最低间隔为0.016秒 1秒/60帧  如果间隔1秒，延迟2秒，那么方法将会在第3秒时执行
        @param repeat 重复次数, 0为永久重复执行 非0时，会自动解绑
        @param runNow 是否立即执行一次？ 立刻执行不影响重复次数和delay，即 不会减少重复次数
        @param delay 延迟多少秒执行逻辑，如果 间隔1秒，延迟2秒，那么方法将会在第3秒时执行
    */
    public loop(thisObj: any, callback: Function, interval: number = 0, repeat: number = 0, runNow: boolean = false, delay?: number): TimerObj {
        for (let i = 0; i < TimerTS._arr.length; i++) {
            if (TimerTS._arr[i].thisObj == thisObj && TimerTS._arr[i].callback == callback) {

                Log.logWarning("重复注册定时器");
                return;
            }
        }

        if (!TimerTS._isRunning) {
            TimerTS._isRunning = true;
            TSMain.Instance.startRunTime();
        }

        let obj: TimerObj = {
            callback: callback,
            thisObj: thisObj,
            interval: interval,
            repeat: repeat,
            accTime: 0,
            delay: delay,
            uid: this.getUID()
        };

        if (runNow) {
            callback.apply(thisObj, [0]);
        }

        TimerTS._arr.push(obj);

        return obj;
    }

    /**
  * 清理监听
  * @param callback 
  * @param thisObj 
  */
    public clear(thisObj: any, callback: Function): void {
        let arr = TimerTS._arr;
        for (let n = 0; n < arr.length; n++) {
            let obj: TimerObj = arr[n];
            if (obj.callback == callback && obj.thisObj == thisObj) {
                arr.splice(n, 1);
                // n--;
                return;
            }
        }
        TimerTS.checkStop();
    }

    /**
     * 执行一次的计时器
     * @param callback 每次触发的回调方法 回调参数 dt：number 单位秒
     * @param thisObj 方法对应的对象
     * @param delay 延迟多久触发 单位秒
     */
    public once(thisObj: any, callback: Function, delay: number): TimerObj {
        return this.loop(thisObj, callback, delay, 1);
    }

    //当前是否暂停
    public get pause(): boolean {
        return TimerTS._pause;
    }

    //设置暂停
    public set pause(_pause) {
        TimerTS._pause = _pause;
    }


    public static checkStop() {
        if (!TimerTS._arr.length) {
            TSMain.Instance.stopRunTime();
            TimerTS._isRunning = false;
        }

    }

}