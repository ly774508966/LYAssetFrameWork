using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LYAssetFrameWork
{
    /// <summary>
    /// 采用本地IO读取AssetBundle方式
    /// AssetBundle资源更新由游戏版本更新逻辑处理
    /// </summary>
    public class LYAssetManager : SingletonMono<LYAssetManager>
    {
        public static string FolderPath { set; get; }
        public static string ManifestName { set; get; }
        public AssetBundleManifest Manifest { set; get; }

        void Awake()
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(FolderPath, ManifestName));
            if (bundle == null)
            {
                Debug.LogError("Manifest file not exist:" + Path.Combine(FolderPath, ManifestName));
            }
            Manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            bundle.Unload(false);
            if (Manifest == null)
            {
                Debug.LogError("Load manifest error: can not get manifest from assetBundle");
            }
            else
            {
                Debug.Log("Load manifest ok");
            }
        }

        #region 同步加载

        public LYAssetBundle LoadAssetBundle(string assetBundleName)
        {
            LYAssetBundle bundle = LYAssetCache.GetBundleCache(assetBundleName);
            if (bundle != null)
            {
                return bundle;
            }
            LoadDependencies(assetBundleName);
            return doLoadAssetBundle(assetBundleName);
        }

        public void LoadDependencies(string assetBundleName)
        {
            if (this.Manifest == null)
            {
                Debug.LogError("Manifest is null!");
                return;
            }

            string[] dependencies = GetDependencies(assetBundleName);
            if (dependencies.Length == 0) return;

            for (int i = 0; i < dependencies.Length; i++)
            {
                doLoadAssetBundle(dependencies[i]);
            }
        }

        private LYAssetBundle doLoadAssetBundle(string assetBundleName)
        {
            LYAssetBundle ly_bundle = LYAssetCache.GetBundleCache(assetBundleName);
            if (ly_bundle != null)
            {
                //保留一次
                ly_bundle.Retain();
                return ly_bundle;
            }
            AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(FolderPath, assetBundleName));
            if (bundle == null)
            {
                Debug.LogError("AssetBundleName file not exist:" + Path.Combine(FolderPath, assetBundleName));
                return null;
            }
            ly_bundle = new LYAssetBundle(bundle, assetBundleName);
            LYAssetCache.SetBundleCache(assetBundleName, ly_bundle);
            return ly_bundle;
        }

        #endregion

        #region 异步加载

        public void LoadAssetBundleAsyn(string assetBundleName, Action<LYAssetBundle> callback)
        {
            LYAssetBundle bundle = LYAssetCache.GetBundleCache(assetBundleName);
            if (bundle != null)
            {
                if (callback != null)
                {
                    callback(bundle);
                }
                return;
            }
            StartCoroutine(doLoadAssetBundleAsyn(assetBundleName, callback));
        }

        private IEnumerator doLoadAssetBundleAsyn(string assetBundleName, Action<LYAssetBundle> callback)
        {
            string[] dependencies = GetDependencies(assetBundleName);
            if (dependencies.Length > 0)
            {
                int index = 0;
                while (index < dependencies.Length)
                {
                    LYAssetAsynLoader dependLoader = new LYAssetAsynLoader(dependencies[index]);
                    dependLoader.LoadAsyn();
                    while (dependLoader.IsLoaded == false)
                    {
                        yield return null;
                    }
                    index++;
                }
            }
            yield return null;
            LYAssetAsynLoader loader = new LYAssetAsynLoader(assetBundleName);
            loader.LoadAsyn();
            while (loader.IsLoaded == false)
            {
                yield return null;
            }
            if (callback != null)
            {
                callback(LYAssetCache.GetBundleCache(assetBundleName));
            }
        }

        #endregion


        public string[] GetDependencies(string assetBundleName)
        {
            // 获取依赖包裹
            string[] dependencies = this.Manifest.GetAllDependencies(assetBundleName);

            if (dependencies.Length != 0)
            {
                // 记录并且加载所有的依赖包裹
                LYAssetCache.SetDependCache(assetBundleName, dependencies);
            }
            return dependencies;
        }
    }
}
