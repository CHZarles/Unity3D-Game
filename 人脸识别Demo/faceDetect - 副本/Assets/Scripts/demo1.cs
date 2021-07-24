using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Baidu.Aip.Face;
using System.Text;
using System;
using UnityEngine.UI;


public class demo1 : MonoBehaviour
{
    public string API_KEY = "zEsSeI3Pa7kPbgfo2L0mCKMo";
    public string SECRET_KEY = "UxHHOhZa7hElslpn5vcRo0elUKi9WeFs";
    Face client;

    void Start()
    {
        client = new Face(API_KEY, SECRET_KEY);
       // StartCoroutine(IEGetStringBase64());
        FaceSearch();
    }

    //获取图片base64字符串，由于QPS限制，此处采用协程降低注册频率
    IEnumerator IEGetStringBase64()
    {
        //获取到每一张图片的路径
        string[] picsPathArr = Directory.GetFiles(Application.streamingAssetsPath + "/FaceUpload/");
        //循环获取每张图片的base64字符串

        for (int i = 0; i < picsPathArr.Length; i++)
        {

            //unity会自动生成.meta文件，过滤掉
            if (picsPathArr[i].Contains("meta")) continue;
            //读取
            FileInfo file = new FileInfo(picsPathArr[i]);
            var stream = file.OpenRead();
            byte[] buffer = new byte[file.Length];
            //读取图片字节流
            stream.Read(buffer, 0, Convert.ToInt32(file.Length));
            //base64字符串
            string imageBase64 = Convert.ToBase64String(buffer);
            //采用base64字符串方式上传
            string imageType = "BASE64";
            //用户组
            string groupId = "Group1";
            //用户id，一般同一个人的图片放在同一个id下
            string userId = "User1";


            //开始注册
            SignUpFace(imageBase64, imageType, groupId, userId);


            yield return new WaitForSeconds(0.6f);
        }
    }
    //人脸注册
    public void SignUpFace(string image, string imageType, string groupId, string userId)
    {

        var options = new Dictionary<string, object>{
        {"user_info", "User1"},
        {"quality_control", "NORMAL"},
        {"liveness_control", "LOW"}
    };
        // 带参数调用人脸注册
        var result = client.UserAdd(image, imageType, groupId, userId, options);
        Debug.Log(result.ToString());
    }

    public void FaceSearch()
    {
        FileInfo file = new FileInfo(Application.streamingAssetsPath + "/FaceDetect/j4.jpg");
        var stream = file.OpenRead();
        byte[] buffer = new byte[file.Length];
        //读取图片字节流
        stream.Read(buffer, 0, Convert.ToInt32(file.Length));
        var image = Convert.ToBase64String(buffer);

        var imageType = "BASE64";
        //之前注册的组
        var groupIdList = "Group1";
        var result = client.Search(image, imageType, groupIdList);
        Debug.Log(result.ToString());
    }
}