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
        public AssetBundle assetBundle;
        public string assetBundleName;

        /// <summary>
        /// 引用计数
        /// </summary>
        private int m_referencedCount;
        public int ReferencedCount
        {
            get { return m_referencedCount; }
        }

        internal LYAssetBundle(AssetBundle assetBundle, string name)
        {
            this.assetBundle = assetBundle;
            this.assetBundleName = name;
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
                this.assetBundle.Unload(true);
                LYAssetCache.FreeBundle(assetBundleName);
            }
        }

    }
}
