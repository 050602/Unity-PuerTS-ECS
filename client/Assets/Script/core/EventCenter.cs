using System;
using System.Collections.Generic;
using UnityEngine;
public class EventCenter
{

    private static EventCenter _instance;
    public static EventCenter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventCenter();
                // _instance.initInstance();
            }
            return _instance;
        }
    }
    // public delegate void CS2TSCallBack(object[] args);
    public delegate void EventCallBack(object thisobj, params object[] args);

    public event EventCallBack callback;

    // public event CS2TSCallBack cs2ts;

    public class EventObj : ObjectPoolIT
    {
        public EventObj()
        {

        }

        public void init(object sender, EventCallBack handler)
        {
            this.thisObj = sender;
            this.handler = handler;
        }
        public object thisObj;
        public EventCallBack handler;

        public void recycleToObjectPool()
        {
            this.thisObj = null;
            this.handler = null;
        }
    }




    private Dictionary<string, List<EventObj>> eventDic = new Dictionary<string, List<EventObj>>();
    public void bind(string eventName, object thisObj, EventCallBack callBack)
    {
        List<EventObj> callbacks = null;
        EventObj newEventObj = ObjectPool.Instance.getObj<EventObj>();
        newEventObj.init(thisObj, callBack);


        if (!eventDic.TryGetValue(eventName, out callbacks))
        {
            callbacks = new List<EventObj>();
            eventDic[eventName] = callbacks;
        }
        else
        {
            for (var i = 0; i < callbacks.Count; i++)
            {
                var curr = callbacks[i];
                if (curr.thisObj == thisObj && curr.handler == callBack)
                {
                    Log.logWarning("EventCenter 重复注册事件, " + eventName);
                    return;
                }
            }
        }
        callbacks.Add(newEventObj);
    }

    public void unbind(string eventName, object thisObj)
    {
        List<EventObj> callbacks = null;
        if (eventDic.TryGetValue(eventName, out callbacks))
        {
            // eventDic.Remove(eventName);

            for (var i = callbacks.Count - 1; i >= 0; i--)
            {
                if (callbacks[i].thisObj == thisObj)
                {
                    ObjectPool.Instance.recycle<EventObj>(callbacks[i]);

                    callbacks.RemoveAt(i);
                    if (callbacks.Count == 0)
                    {
                        eventDic.Remove(eventName);
                    }
                    return;
                }
            }
        }
    }

    public void eventFunc(string eventName, params object[] args)
    {
        List<EventObj> callbacks = null;
        var ishas = eventDic.TryGetValue(eventName, out callbacks);
        if (ishas)
        {
            var newlist = new List<EventObj>(callbacks);//new 一个List 是为了避免修改原数组导致的报错
            for (var i = 0; i < newlist.Count; i++)
            {
                EventObj obj = newlist[i];
                obj.handler(obj.thisObj, args);
            }
        }

    }


}

