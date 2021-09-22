class LanLuLoader : FairyGUI.GLoader
{
    protected async override void LoadExternal()
    {
        /*
        开始外部载入，地址在url属性
        载入完成后调用OnExternalLoadSuccess
        载入失败调用OnExternalLoadFailed

        注意：如果是外部载入，在载入结束后，调用OnExternalLoadSuccess或OnExternalLoadFailed前，
        比较严谨的做法是先检查url属性是否已经和这个载入的内容不相符。
        如果不相符，表示loader已经被修改了。
        这种情况下应该放弃调用OnExternalLoadSuccess或OnExternalLoadFailed。
        */
        // var url = this.url
        var texture = await AddressablesMgr.Instance.loadAsset<UnityEngine.Texture>(this.url);
        if (texture != null)
        {
            var ntext = new FairyGUI.NTexture(texture);
            this.onExternalLoadSuccess(ntext);
        }
        else
        {
            this.onExternalLoadFailed();
        }

    }

    protected override void FreeExternal(FairyGUI.NTexture texture)
    {
        //释放的资源管理之后再看
        //释放外部载入的资源
        // AddressablesMgr.Instance.releaseAssetByType
        // var text = AddressablesMgr.Instance.getFGUIAsset(this.url);
        var text = texture.alphaTexture;
        AddressablesMgr.Instance.releaseTexture(text);
    }
}