using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour{

    //AssetBundle的本地路径
    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    private string StreamingAssetPath
    {
        get
        {
            return
#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
    "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif
        }
    }

    public enum AssetLoadState
    {
        E_NONE,
        E_LOADING,
        E_LOADED,
    }

    public class AssetLoadTask
    {
        public string assetName = "";
        public AssetBundleRequest assetBundleRequest = null;
        public AssetLoadState state = AssetLoadState.E_NONE;
        public Action<AssetBundleRequest> loadedCb = null;
    }

    public class AssetBundleLoadTask
    {
        public string path = "";
        public AssetBundle bundle = null;
        public AssetLoadState state = AssetLoadState.E_NONE;

        public Dictionary<string, AssetLoadTask> assetReqDic = new Dictionary<string, AssetLoadTask>();

        public AssetLoadTask getOrCreateAssetLoadTask(string assetName)
        {
            if (assetReqDic.ContainsKey(assetName))
            {
                return assetReqDic[assetName];
            }
            else
            {
                AssetLoadTask assetLoadTask = new AssetLoadTask {
                    assetName = assetName,
                    state = AssetLoadState.E_NONE,
                };
                assetReqDic.Add(assetName, assetLoadTask);
                return assetLoadTask;
            }
        }
    }

    private Dictionary<string, AssetBundleLoadTask> _assetBundleDic = new Dictionary<string, AssetBundleLoadTask>();

    public void loadLocalAssetAsync(string assetBundleName, string assetName, Action<AssetBundleRequest> loadCB)
    {
        string assetBundlePath = StreamingAssetPath + "AssetBundles/" + assetBundleName;
        //string assetBundlePath = "http://www.vmetu.com/external/res/buttons";
        Debug.Log("Asset:" + assetBundlePath);

        AssetBundleLoadTask bundleTask = null;
        if (_assetBundleDic.ContainsKey(assetBundlePath))
        {
            bundleTask = _assetBundleDic[assetBundlePath];
        }
        else
        {
            bundleTask = new AssetBundleLoadTask {
                path = assetBundlePath,
                state = AssetLoadState.E_NONE
            };
            _assetBundleDic.Add(assetBundlePath, bundleTask);
        }

        AssetLoadTask assetLoadTask = bundleTask.getOrCreateAssetLoadTask(assetName);

        switch (bundleTask.state)
        {
            case AssetLoadState.E_NONE: //还没开始加载
                bundleTask.state = AssetLoadState.E_LOADING;
                assetLoadTask.loadedCb += loadCB;
                StartCoroutine(doLoadAssetBunlde(bundleTask));
                break;
            case AssetLoadState.E_LOADING: //下载中，把回调加入到响应的回调中
                {
                    assetLoadTask.loadedCb += loadCB;
                }
                break;
            case AssetLoadState.E_LOADED: //assetBundle已下载完成
                {
                    if (assetLoadTask.state == AssetLoadState.E_LOADED)
                    {
                        loadCB(assetLoadTask.assetBundleRequest);
                    }
                    else if (assetLoadTask.state == AssetLoadState.E_LOADED)
                    {
                        assetLoadTask.loadedCb += loadCB;
                    }
                    else
                    {
                        StartCoroutine(doLoadAsset(assetName, bundleTask));
                    }
                }
                break;
        }
    }

    private IEnumerator doLoadAssetBunlde(AssetBundleLoadTask bundleTask)
    {
        // 需要等待缓存准备好
        while (!Caching.ready) yield return null;

        // 有相同版本号的AssetBundle就从缓存中获取，否则下载进缓存。
        WWW www = new WWW(bundleTask.path);
        yield return www;

        if (www.error != null) throw new Exception("WWW download had an error:" + www.error);

        bundleTask.state = AssetLoadState.E_LOADED;
        bundleTask.bundle = www.assetBundle;

        //加载asset
        foreach(KeyValuePair<string, AssetLoadTask> kvp in bundleTask.assetReqDic)
        {
            kvp.Value.state = AssetLoadState.E_LOADING;
            StartCoroutine(doLoadAsset(kvp.Key, bundleTask));
        }
    }

    private IEnumerator doLoadAsset(string name, AssetBundleLoadTask assetBundleLoadTask)
    {
        AssetLoadTask assetLoadTask = assetBundleLoadTask.getOrCreateAssetLoadTask(name);

        // 异步加载
        assetLoadTask.assetBundleRequest = assetBundleLoadTask.bundle.LoadAssetAsync(name);

        // 等待加载完成
        yield return assetLoadTask.assetBundleRequest;

        assetLoadTask.state = AssetLoadState.E_LOADED;

        if (assetLoadTask.loadedCb != null)
        {
            assetLoadTask.loadedCb(assetLoadTask.assetBundleRequest);
        }
        assetLoadTask.loadedCb = null;
    }


    public void loadNetAssetAsync(string url, string assetName, Action<AssetBundleRequest> loadedCB)
    {

    }

    private void Start()
    {
    }
}
