using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleWebLoader : MonoBehaviour
{
    public string bundleUrl = "file://C:\\Users\\jiwoo\\Desktop\\bundletest\\testbundle";
    public string assetName = "BundledSpriteObject";
    IEnumerator Start()
    {
        using(WWW web = new WWW(bundleUrl))
        {
            yield return web;
            AssetBundle remoteAssetBundle = web.assetBundle;
            if (remoteAssetBundle == null)
            {
                Debug.LogError("Failed to download AssetBundle");
                yield break;
            }
            Instantiate(remoteAssetBundle.LoadAsset(assetName));
            remoteAssetBundle.Unload(false);//
        }
    }
}
