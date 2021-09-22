using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using System.Threading.Tasks;
using System.IO;

using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesMgr
{
    public static FairyGUI.UIPackage.LoadResource LanLuLoadFunc = (string name, string extension, System.Type type, out FairyGUI.DestroyMethod destroyMethod) =>
{
    destroyMethod = FairyGUI.DestroyMethod.Unload;
    var obj = AddressablesMgr.Instance.getFGUIAsset("Assets/Assets/UI/" + name + extension);

    return obj;
};


    private static AddressablesMgr _instance;
    public static AddressablesMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AddressablesMgr();
            }
            return _instance;
        }
    }

    public AsyncOperationHandle<List<IResourceLocator>> nowLoadHandle;
    public AsyncOperationHandle nowLoadHandle2;
    public Dictionary<string, UnityEngine.Object> fguiDic = new Dictionary<string, UnityEngine.Object>();
    public async void gameStart()
    {
        var updates = Addressables.CheckForCatalogUpdates(false);

        await updates.Task;
        //需要更新，显示百分比
        var updateCatalog = Addressables.UpdateCatalogs(updates.Result, false);

        await updateCatalog.Task;

        if (updateCatalog.Status == AsyncOperationStatus.Succeeded)
        {
            this.updateCatlogResult = updateCatalog.Result;
        }

        long size = 0;
        if (this.updateCatlogResult != null)
        {
            foreach (var item in this.updateCatlogResult)
            {
                var sizeHandle = Addressables.GetDownloadSizeAsync(item.Keys);

                await sizeHandle.Task;

                size += sizeHandle.Result;
            }
        }

        if (size > 0)
        {
            long sizeMB = size / 1048576;

            EventCenter.Instance.eventFunc(AddressablesEvent.NEED_LOAD_ASSET, sizeMB);

            EventCenter.Instance.bind(AddressablesEvent.UPDATE_SURE, this, this.runUpdate);
        }
        else
        {
            EventCenter.Instance.eventFunc(AddressablesEvent.LOAD_ADDRESSABLES_OVER, null);
        }

    }


    List<IResourceLocator> updateCatlogResult;
    //执行更新
    public async void runUpdate(object thisobj, params object[] args1)
    {
        if (this.updateCatlogResult != null)
        {
            foreach (var item in this.updateCatlogResult)
            {
                this.nowLoadHandle2 = Addressables.DownloadDependenciesAsync(item.Keys, Addressables.MergeMode.Union);
                await this.nowLoadHandle2.Task;

            }


        }

        EventCenter.Instance.unbind(AddressablesEvent.UPDATE_SURE, this);

        EventCenter.Instance.eventFunc(AddressablesEvent.LOAD_ADDRESSABLES_OVER, null);
    }


    public async Task<GameObject> getObjByKey(string key)
    {
        //使用Addressables.InstantiateAsync方法本身会有一些格外的开销，如果您需要实例化同一个对象很多次，比如说一帧内实例化100个，使用此方法就不再合适，可以考虑使用Addressables.LoadAssetAsync进行资源的加载，同时自己保存返回的结果，然后再使用GameObject.Instantiate()进行实例化，同时当所有的GameObject不再使用后，再通过Addessables.Release方法将保存的结果进行释放。使用此种方法虽然可以提高部分性能，但是需要对其增加格外的管理。
        var obj = Addressables.InstantiateAsync(key, null, false, true);

        await obj.Task;

        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            return obj.Result;
        }

        return null;
    }

    public async Task<T> loadAsset<T>(string key) where T : UnityEngine.Object
    {
        var obj = Addressables.LoadAssetAsync<T>(key);

        await obj.Task;

        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            return obj.Result;
        }

        return null;
    }

    public async Task<TextAsset> loadTextAsset(string key)
    {
        var obj = await this.loadAsset<TextAsset>(key);
        return obj;
    }

    public async Task<Texture> loadTexture(string key)
    {
        var obj = await this.loadAsset<Texture>(key);
        return obj;
    }

    public async Task<GameObject> loadGameObject(string key)
    {
        var obj = await this.loadAsset<GameObject>(key);
        return obj;
    }

    public void releaseGameObject(GameObject obj)
    {
        Addressables.Release(obj);
    }


    public async Task<bool> loadCatalog(string pkg)
    {
        await Addressables.LoadContentCatalogAsync(pkg).Task;
        return true;
    }

    public async Task<T> loadFGUIAsset<T>(string key) where T : UnityEngine.Object
    {
        var obj = Addressables.LoadAssetAsync<T>(key);

        // Debug.Log("key:" + key);
        await obj.Task;

        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            fguiDic[key] = obj.Result as UnityEngine.Object;
            return obj.Result;
        }

        return default(T);
    }


    public async Task<TextAsset> loadFGUITextAsset(string key)
    {
        var obj = await this.loadFGUIAsset<TextAsset>(key);
        return obj;
    }

    public async Task<Texture> loadFGUITexture(string key)
    {
        var obj = await this.loadFGUIAsset<Texture>(key);
        return obj;
    }


    public UnityEngine.Object getFGUIAsset(string key)
    {
        UnityEngine.Object obj;
        var ishas = this.fguiDic.TryGetValue(key, out obj);
        if (ishas)
        {
            return obj;
        }
        return null;
    }

    // public void releaseAsset(string key)
    // {
    //     Addressables.Release(key);
    // }

    public void releaseAssetByType<T>(T obj)
    {
        Addressables.Release(obj);
    }

    public void releaseTexture(Texture obj)
    {
        Addressables.Release(obj);
    }

    public void releaseTextAsset(TextAsset obj)
    {
        Addressables.Release(obj);
    }

    public void releaseObj(GameObject obj)
    {
        Addressables.ReleaseInstance(obj);
    }


    // public void loadOver(AsyncOperationHandle handle)
    // {
    //     EventCenter.Instance.eventFunc(AddressablesEvent.LOAD_ADDRESSABLES_OVER, null);
    // }

}