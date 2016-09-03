using System;
using System.Collections.Generic;

/****************************************************************
*   作者：Dun
*   CLR版本：4.0.30319.42000
*   创建时间：2016/9/4 0:23:27
*   描述说明：Asset缓存
*   修改历史：
*
*****************************************************************/
namespace LYAssetFrameWork
{
    internal sealed class LYAssetCache
    {
        private static Dictionary<string, LYAssetBundle> m_assetBundleCache;
        private static Dictionary<string, LYAssetBundle> BundleCache
        {
            get
            {
                if (m_assetBundleCache == null)
                {
                    m_assetBundleCache = new Dictionary<string, LYAssetBundle>();
                }

                return m_assetBundleCache;
            }
        }

        private static Dictionary<string, string[]> m_dependCache;
        private static Dictionary<string, string[]> DependCache
        {
            get
            {
                if (m_dependCache == null)
                {
                    m_dependCache = new Dictionary<string, string[]>();
                }
                return m_dependCache;
            }
        }

        internal static bool InCache(string assetbundlename)
        {
            return LYAssetCache.BundleCache.ContainsKey(assetbundlename);
        }

        internal static void UnloadAssetBundle(string assetBundleName)
        {
            UnloadAssetBundleInternal(assetBundleName);
            UnloadDependencies(assetBundleName);
        }

        internal static void UnloadDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            //获取所有的依赖包名称
            if (!LYAssetCache.DependCache.TryGetValue(assetBundleName, out dependencies))
                return;

            //卸载依赖包
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }
            //删除依赖缓存策略
            LYAssetCache.DependCache.Remove(assetBundleName);
        }

        internal static void UnloadAssetBundleInternal(string assetBundleName)
        {
            LYAssetBundle bundle;
            LYAssetCache.BundleCache.TryGetValue(assetBundleName, out bundle);

            if (bundle == null)
            {
                return;
            }
            bundle.Release();
        }

        internal static LYAssetBundle GetBundleCache(string key)
        {
            LYAssetBundle ab;

            LYAssetCache.BundleCache.TryGetValue(key, out ab);

            return ab;
        }
        internal static void SetBundleCache(string key, LYAssetBundle value)
        {
            LYAssetCache.BundleCache.Add(key, value);
        }

        internal static string[] GetDependCache(string key)
        {
            string[] depends;

            LYAssetCache.DependCache.TryGetValue(key, out depends);

            return depends;
        }

        internal static void SetDependCache(string key, string[] value)
        {
            LYAssetCache.DependCache.Add(key, value);
        }

        internal static void FreeBundle(string key)
        {
            LYAssetCache.BundleCache.Remove(key);
        }
    }
}
