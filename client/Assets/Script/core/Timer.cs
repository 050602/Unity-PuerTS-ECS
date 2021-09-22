using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerObj
{
    public TimerObj(object sender, Action<float> handler, int uid, float accTime, float interval = 0, int repeat = 0, bool runNow = false, float delay = 0)
    {
        this.thisObj = sender;
        this.handler = handler;
        this.interval = interval;
        this.repeat = repeat;
        this.runNow = runNow;
        this.delay = delay;
        this.uid = uid;
        this.accTime = accTime;
    }
    public object thisObj;
    public Action<float> handler;

    public float interval;
    public int repeat;
    public int uid;
    public bool runNow;
    public float accTime;
    public float delay;
}

public class Timer
{

    private static Timer _instance;
    public static Timer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Timer();
                _instance.initInstance();
            }
            return _instance;
        }
    }


    bool fightTimerRun = false;
    private List<TimerObj> timerList;
    bool _pause = false;
    public float logicTime;
    public void initInstance()
    {
        this.uid = 1;
        timerList = new List<TimerObj>();
    }

    public void destroyInstance()
    {
        this.uid = 1;
        this.timerList.Clear();
    }

    private int uid;
    //获取唯一ID
    public int getUID()
    {
        return this.uid++;
    }

    public float getLogicTime()
    {
        return this.logicTime;
    }

    public TimerObj loop(object thisObj, Action<float> arg, float interval = 0, int repeat = 0, bool runNow = false, float delay = 0)
    {
        var uid = this.getUID();
        var obj = new TimerObj(thisObj, arg, uid, 0, interval, repeat, runNow, delay);
        timerList.Add(obj);
        return obj;
    }

    public TimerObj once(object thisObj, Action<float> arg, float delay = 0)
    {
        return this.loop(thisObj, arg, 0, 1, false, delay);
    }

    //此处传入的Handler , 可以是重新 new 出来的，只要确保 对象 以及 方法 匹配；
    public void clear(Action<float> func)
    {
        for (var i = timerList.Count - 1; i >= 0; i--)
        {
            if (timerList[i].handler == func)
            {
                timerList.RemoveAt(i);
                i--;
            }
        }
    }
    //此处传入的Obj 必须是 loop 的时候返回的timerobj
    public void clear(TimerObj obj)
    {
        this.timerList.Remove(obj);
    }

    public void callLater(string funcName, object thisobj1, params object[] objs)
    {
        this.once(thisobj1, (float dt) =>
        {
            var method = thisobj1.GetType().GetMethod(funcName);
            method.Invoke(thisobj1, objs);
        }, 0.01f);
    }


    public void runFight()
    {
        this.fightTimerRun = true;
    }

    public void stopFight()
    {
        this.fightTimerRun = false;
    }

    public void update()
    {
        if (this._pause) return;

        logicTime += GameConst.GameUpdateCD;

        for (var i = timerList.Count - 1; i >= 0; i--)
        {
            var dt = GameConst.GameUpdateCD;
            var obj = timerList[i];
            obj.accTime += dt;  //0+0.033 +0.033 = 0.066 +0.033 = 0.1
            if (obj.delay > 0)
            {
                var delay = obj.delay;
                //有延时 
                if (obj.accTime >= delay)
                {
                    obj.accTime = 0;
                    obj.delay = 0;
                }
            }
            else
            {
                var interval = obj.interval;
                if (obj.accTime >= interval)
                {    //0.033> 0.1? 0.066>0.1? 0.1>=0.1
                    obj.accTime -= interval;
                    if (interval != 0)
                    {
                        dt = interval;
                    }

                    // var args = ObjectPool.Instance.getObj<TimerArgs>();
                    if (obj.repeat > 0)
                    {        //3 2 1
                        obj.repeat--;            //2 1 0
                                                 // func.apply(target, [dt]);     //执行一次 2 3
                                                 // args.dt = dt;
                        obj.handler(dt);

                        if (obj.repeat == 0)
                        {   //第三次时，等于0

                            // Log.log2UI("清除timer", obj.handler, i, timerList.Count, obj.repeat, obj.accTime, obj.delay, obj.interval);
                            // arr.splice(i, 1);   //被销毁
                            if (i < timerList.Count)
                            {
                                timerList.RemoveAt(i);
                            }
                            // i--;
                        }
                    }
                    else
                    {
                        // args.dt = dt;
                        obj.handler(dt);
                        // func.apply(target, [dt]);
                    }
                    // ObjectPool.Instance.recycle<TimerArgs>(args);
                }
            }

        }
    }

}