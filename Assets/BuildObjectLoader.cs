using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class BuildObjectLoader : MonoBehaviour
{
    public string assetName = "BundledSpriteObject";
    public string bundleName = "testbundle";
    AssetBundle assetBundle;
    GameObject asset;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.streamingAssetsPath + "/" + bundleName;
        assetBundle = AssetBundle.LoadFromFile(path);

        if(assetBundle == null)
        {
            print("Filed to load AssetBundel!");
        }

        asset = assetBundle.LoadAsset<GameObject>(assetName);
        Instantiate(asset);

        UnityEngine.Object[] objs = assetBundle.LoadAllAssets();
        Type type = objs[0].GetType();
        
        assetBundle.Unload(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            assetBundle.Unload(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            assetBundle.Unload(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Instantiate(asset);
        }
    }
}
