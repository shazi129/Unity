using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButtons : MonoBehaviour {

    public AssetManager assetManager;

    // Use this for initializations
    void Start () {

        //         assetManager.loadLocalAssetAsync("test_assetbundle/buttons", "ButtonGreen", onLoadAssetSuc);
        //         assetManager.loadLocalAssetAsync("test_assetbundle/buttons", "ButtonYellow", onLoadAssetSuc);
        //         assetManager.loadLocalAssetAsync("test_assetbundle/buttons", "ButtonGreen", onLoadAssetSuc);
        //         assetManager.loadLocalAssetAsync("test_assetbundle/buttons", "ButtonYellow", onLoadAssetSuc);

        //Debug.Log(UnityEngine.Caching.path);
    }

    private void onLoadAssetSuc(AssetBundleRequest obj)
    {
        GameObject newObj = GameObject.Instantiate(obj.asset as GameObject);
        if (newObj != null)
        {
            newObj.transform.SetParent(gameObject.transform);
        }
    }
}
