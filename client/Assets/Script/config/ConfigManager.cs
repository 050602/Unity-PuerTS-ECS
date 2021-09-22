using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
public class Configs
{
    public KvConfigCls KvConfig { get; set; }
    public PetInfoConfigCls PetInfoConfig { get; set; }
    public PetStrenConfigCls PetStrenConfig { get; set; }
    public GoodsConfigCls GoodsConfig { get; set; }
}
public class ConfigManager
{
    public static TextAsset source;
    public static Configs configs;
    public async static Task<bool> Load(string url)
    {
        source = await AddressablesMgr.Instance.loadAsset<TextAsset>(url);
        return true;
    }
    public static void Init()
    {
        configs = JsonConvert.DeserializeObject<Configs>(source.text);
        configs.KvConfig.Parse();
        configs.PetInfoConfig.Parse();
        configs.PetStrenConfig.Parse();
        configs.GoodsConfig.Parse();
    }
}