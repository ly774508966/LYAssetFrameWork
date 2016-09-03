# LYAssetFrameWork
 Unity5.3以上版本的AssetBundle加载、缓存

采用本地IO读取AssetBundle方式
AssetBundle资源更新由游戏版本更新逻辑处理

1.LoadFromFile同步加载AssetBundle
2.LoadFromFileAsync异步加载AssetBundle
3.引用计数

使用方法：
1.设置AssetBundle文件所在目录->FolderPath

2.设置AssetBundle的Manifest文件名->Manifest

3.使用LoadAssetBundle(string assetBundleName)同步加载

4.使用LoadAssetBundleAsyn(string assetBundleName, Action<LYAssetBundle> callback)异步加载
