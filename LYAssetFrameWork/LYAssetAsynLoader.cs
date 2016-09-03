using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/****************************************************************
*   作者：Dun
*   CLR版本：4.0.30319.42000
*   创建时间：2016/9/4 1:24:37
*   描述说明：异步加载器
*   修改历史：
*
*****************************************************************/
namespace LYAssetFrameWork
{
    internal sealed class LYAssetAsynLoader
    {
        private enum State
        {
            None,
            Loading,
            Loaded
        }

        private State m_state;

        public bool IsLoaded
        {
            get { return m_state == State.Loaded; }
        }

        private string m_assertName;

        public LYAssetAsynLoader(string assertName)
        {
            m_state = State.None;
            m_assertName = assertName;
        }

        public void LoadAsyn()
        {
            if (m_state != State.None)
            {
                return;
            }
            var bundle = LYAssetCache.GetBundleCache(m_assertName);
            if (bundle != null)
            {
                //保留一次
                bundle.Retain();
                m_state = State.Loaded;
                return;
            }
            m_state = State.Loading;
            LYAssetManager.Instance.StartCoroutine(doAsynLoad());
        }

        private IEnumerator doAsynLoad()
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(LYAssetManager.FolderPath, m_assertName));
            yield return bundleLoadRequest;
            AssetBundle bundle = bundleLoadRequest.assetBundle;
            if (bundle == null)
            {
                Debug.LogError("AssetBundleName file not exist:" + Path.Combine(LYAssetManager.FolderPath, m_assertName));
                yield break;
            }
            LYAssetCache.SetBundleCache(m_assertName, new LYAssetBundle(bundle, m_assertName));
            m_state = State.Loaded;
        }
    }
}
