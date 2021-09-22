using Puerts;

public class TSMain
{
    private static TSMain _instance;
    public static TSMain Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TSMain();
                _instance.initInstance();
            }
            return _instance;
        }
    }

    public UnityEngine.GameObject camera;
    
    public UnityEngine.GameObject trueCamera;
    
    public delegate void JoyCallBack(float d);
    public delegate void JoyEndCallBack();
    public JoyCallBack joyCallBack;
    public JoyEndCallBack joyEndCallBack;
    JsEnv jsEnv;

    public bool isDebug = true;

    // Use this for initialization
    //JsEnv还有一个构造函数可以传loader，
    //通过实现不同的loader，可以做到从诸如
    //AssetBundle，压缩包，网络等源加载代码
    //这个无参构造会用默认的loader，默认loader
    //从Resources目录加载

    public void initInstance()
    {
        if (isDebug)
        {
            jsEnv = new JsEnv(new PuerTsLoader(), 8080);
        }
        else
        {
            jsEnv = new JsEnv(new PuerTsLoader());
        }

        jsEnv.UsingAction<object[]>();
        jsEnv.UsingAction<float>();
        jsEnv.UsingAction<int, FairyGUI.GObject>();
        jsEnv.Eval(@"
               require('main')
            ");

        // this.startRunTime();
    }

    public void startRunTime(float interval = 0)
    {
        Timer.Instance.loop(this, this.Update, interval);
    }


    public void stopRunTime()
    {
        Timer.Instance.clear(this.Update);
    }


    private void Update(float dt)
    {
        if (isDebug)
            jsEnv.Tick();
        EventCenter.Instance.eventFunc(TSEvent.TICK, dt);
    }

    public void destroyInstance()
    {
        jsEnv.Dispose();
        this.stopRunTime();
    }

    //从C#调用TS 需要注意自己引入包
    public void callTS(string jsStr)
    {
        jsEnv.Eval(@jsStr);
    }
}