using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class TSMgr : Singleton<TSMgr>
{
    public const string TSPath = "Assets/Assets/ts/";
    public const string TSEnd = ".txt";
    private Dictionary<string, string> dic = new Dictionary<string, string>();
    public async Task<bool> init()
    {
        var cjsload = await AddressablesMgr.Instance.loadAsset<TextAsset>("cjsload");
        var init2 = await AddressablesMgr.Instance.loadAsset<TextAsset>("init");
        var csharp = await AddressablesMgr.Instance.loadAsset<TextAsset>("csharp");
        var log = await AddressablesMgr.Instance.loadAsset<TextAsset>("log");
        var modular = await AddressablesMgr.Instance.loadAsset<TextAsset>("modular");
        var polyfill = await AddressablesMgr.Instance.loadAsset<TextAsset>("polyfill");
        var promises = await AddressablesMgr.Instance.loadAsset<TextAsset>("promises");
        var timer = await AddressablesMgr.Instance.loadAsset<TextAsset>("timer");
        var events = await AddressablesMgr.Instance.loadAsset<TextAsset>("events");

        this.dic.Add("Assets/Assets/ts/puerts/cjsload.js.txt", cjsload.text);
        this.dic.Add("Assets/Assets/ts/puerts/init.js.txt", init2.text);
        this.dic.Add("Assets/Assets/ts/puerts/csharp.js.txt", csharp.text);
        this.dic.Add("Assets/Assets/ts/puerts/log.js.txt", log.text);
        this.dic.Add("Assets/Assets/ts/puerts/modular.js.txt", modular.text);
        this.dic.Add("Assets/Assets/ts/puerts/polyfill.js.txt", polyfill.text);
        this.dic.Add("Assets/Assets/ts/puerts/promises.js.txt", promises.text);
        this.dic.Add("Assets/Assets/ts/puerts/timer.js.txt", timer.text);
        this.dic.Add("Assets/Assets/ts/puerts/events.js.txt", events.text);


        // var lines = AddressablesMgr.Instance.loadAsset<TextAsset>(TSPath + "ScriptName.txt");
        // var str = await lines;
        // var line = str.text.Split("|"[0]);
        // var max = line.Length;
        // for (var i = 0; i < line.Length; i++)
        // {
        //     if (line[i] == "")
        //     {
        //         continue;
        //     }
        TextAsset data = await AddressablesMgr.Instance.loadAsset<TextAsset>("Assets/Assets/ts/main.js.txt");
        if (data)
        {
            // Debug.Log(data);
            this.dic.Add("Assets/Assets/ts/main.js.txt", data.text);
            // this.dic.Add(line[i], data.text);
            // var arg = new GameStartProgressEventArgs();
            // arg.prog = i / max * 100;
            // float prog = 1 * 100 / max;
            // Log.gzaLog("prog", i, max, prog);

            // EventCenter.Instance.eventFunc(UIViewEvent.GAME_START_PROGRESS, prog);
            GameStart.UI_GameStartView.gameStartView.m_pb.value += 16;
        }
        // }

        return true;

    }

    public string getScript(string name)
    {
        string str;
        // Debug.Log(name);
        this.dic.TryGetValue(name, out str);
        return str;
    }

    public bool checkScript(string name)
    {
        string str;
        // Debug.Log("==" + name);
        var ishas = this.dic.TryGetValue(name, out str);
        return ishas;
        // return true;
    }


}