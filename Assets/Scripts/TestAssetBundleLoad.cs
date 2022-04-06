using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TestAssetBundleLoad : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        Debug.Log("Start 1");
        //开始一个协程
        // StartCoroutine(LoadFromMemoryAsync2());
        // StartCoroutine(LoadFromFileAsync2());
        // StartCoroutine(UnityWbRequest2());

        StartCoroutine(UnityWbRequest3());

        Debug.Log("Start 2");

    }

    private void Update()
    {
        Debug.Log("---> TestLife 1");
        StartCoroutine(TestLife());
        Debug.Log("---> TestLife 3");
    }

    private void LateUpdate()
    {
        Debug.Log("---> TestLife 6");
    }


    IEnumerator TestLife()
    {
        Debug.Log("---> TestLife 2");
        yield return null;
        Debug.Log("---> TestLife 4");
        yield return PrintAAA();
        Debug.Log("---> TestLife 5");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("---> TestLife b1");
    }

    private IEnumerator PrintAAA()
    {
        Debug.Log("---> TestLife 44");
        return null;
    }


    ///
    /// 第一帧输出： 1 2 3 6
    /// 第二帧输出： 1 2 3 4 44 6
    /// 第三帧输出： 1 2 3 4 44 5 6
    /// 第四帧输出： 1 2 3 4 44 5 6
    /// 第五帧输出： 1 2 3 4 44 5 6
    /// ... 
    /// 0.1s后输出： 1 2 3 4 44 5 b1 6
    /// 后续  输出： 1 2 3 4 44 5 b1 6
    /// ...
    /// 
    /// 总结：
    /// 
    /// yield return 之前的部分在调用的时候执行 2
    /// yield return 之后的部分在Update执行完成之后调用 4 5， 
    /// 每次执行到 yield return 时协程将会等待 yield return 后的内容执行完毕,如果
    /// yield return 后的内容已经被执行完成则会执行一次yield return后的内容并且直接执行后面的内容。
    /// 
    /// 如果上述TestLife 中的yield return都被执行过了 相当于 如下代码：

    // void TestLife()
    // {
    //     Debug.Log("---> TestLife 2");
    //     Debug.Log("---> TestLife 4");
    //     PrintAAA();
    //     Debug.Log("---> TestLife 5");
    //     new WaitForSeconds(0.1f);
    //     Debug.Log("---> TestLife b1");
    // } 
    ///
    /// 
    /// 线程几个小用法:
    /// 1、将一个复杂程序分帧执行
    /// 2、异步加载等功能 AB包资源加载， Resources 资源异步加载 ，场景异步加载， www模块的异步请求

    //第一种加载方式(LoadFromMemoryAsync)从内存加载
    IEnumerator LoadFromMemoryAsync2()
    {
        Debug.Log("---> LoadFromMemoryAsync2  start");
        string path = "AssetBundles/testab.ab";
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(path));
        yield return request;
        AssetBundle ab = request.assetBundle;
        Object[] obj = ab.LoadAllAssets<GameObject>();
        foreach (var o in obj)
        {
            Instantiate(o);
        }
        Debug.Log("---> LoadFromMemoryAsync2  end");
    }

    //第二种方式(LoadFromFile)从本地异步加载
    IEnumerator LoadFromFileAsync2()
    {
        Debug.Log("---> LoadFromFileAsync2  start");
        string path = "AssetBundles/testab.ab";
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        yield return request;
        AssetBundle ab = request.assetBundle;
        Object[] obj = ab.LoadAllAssets<GameObject>();
        foreach (var o in obj)
        {
            Instantiate(o);
        }
        Debug.Log("---> LoadFromFileAsync2  end");
    }

    //第三种加载方式   使用UnityWbRequest  服务器加载使用http本地加载使用file
    IEnumerator UnityWbRequest2()
    {
        Debug.Log("---> UnityWbRequest2  start");
        string uri = @"http://192.168.1.28:10101/HttpServer/U3D/AssetBundles/abtest/t1.ab";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);// UnityWebRequest.GetAssetBundle(uri);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
            //使用里面的资源
            //加载出来放入数组中
            Object[] obj = ab.LoadAllAssets<GameObject>();
            // 创建出来
            foreach (Object o in obj)
            {
                Instantiate(o);
            }
        }


        Debug.Log("---> UnityWbRequest2  end");
    }

    IEnumerator UnityWbRequest3()
    {
        Debug.Log("---> UnityWbRequest3  start");
        string uri = @"http://192.168.1.28:10101/HttpServer/U3D/AssetBundles/abtest/t1.ab";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);// UnityWebRequest.GetAssetBundle(uri);
        request.SendWebRequest();

        while (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            yield return null;
        }
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //使用里面的资源
        //加载出来放入数组中
        Object[] obj = ab.LoadAllAssets<GameObject>();
        // 创建出来
        foreach (Object o in obj)
        {
            Instantiate(o);
        }


        Debug.Log("---> UnityWbRequest3  end");
    }


}
