using UnityEngine;
using System.Collections;
using LYAssetFrameWork;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    public Image image;
    public Canvas canvas;

	// Use this for initialization
	void Start ()
	{
	    LYAssetManager.FolderPath = Application.streamingAssetsPath;
	    LYAssetManager.ManifestName = "StreamingAssets";
	   
        LYAssetManager.Instance.LoadAssetBundleAsyn("testprefab", (LYAssetBundle bundle) =>
        {
            var obj = bundle.assetBundle.LoadAsset<GameObject>("Panel");
            GameObject temp = Instantiate(obj);
            temp.transform.SetParent(canvas.transform, false);
            temp.transform.localPosition = Vector3.zero;
        });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
