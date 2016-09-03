using UnityEngine;

/****************************************************************
*   作者：Dun
*   CLR版本：4.0.30319.42000
*   创建时间：2016/9/4 0:14:13
*   描述说明：
*   修改历史：
*
*****************************************************************/
namespace LYAssetFrameWork
{
    public sealed class LYAssetBundle
    {
        internal AssetBundle m_assetBundle;
        internal string m_assetBundleName;

        /// <summary>
        /// 引用计数
        /// </summary>
        private int m_referencedCount;
        internal int ReferencedCount
        {
            get { return m_referencedCount; }
        }

        internal LYAssetBundle(AssetBundle assetBundle, string name)
        {
            this.m_assetBundle = assetBundle;
            this.m_assetBundleName = name;
            this.m_referencedCount = 1;
        }

        internal void Retain()
        {
            this.m_referencedCount++;
        }

        internal void Release()
        {
            this.m_referencedCount--;
            if (this.m_referencedCount == 0)
            {
                //卸载资源
                this.m_assetBundle.Unload(true);
                LYAssetCache.FreeBundle(m_assetBundleName);
            }
        }

    }
}
