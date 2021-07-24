using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Baidu.Aip.Face;
using System.Text;
using System;
using UnityEngine.UI;

public class FaceDetect : MonoBehaviour
{
    public string API_KEY = "zEsSeI3Pa7kPbgfo2L0mCKMo";
    public string SECRET_KEY = "UxHHOhZa7hElslpn5vcRo0elUKi9WeFs";

    Face client;


    //play controler的脚本
    private PlayerControler player;
   public bool needjump = false;


    void Awake()
    {
        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                           System.Security.Cryptography.X509Certificates.X509Chain chain,
                           System.Net.Security.SslPolicyErrors sslPolicyErrors)
               {
                   return true; // **** Always accept
               };
    }


    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerControler>(); //获得控制权

        Application.runInBackground = true;
        client = new Face(API_KEY, SECRET_KEY);
        StartCoroutine(CallCamera());
        // StartCoroutine(IEGetStringBase64());

    }

    private string deviceName;
    private WebCamTexture webTex;
    IEnumerator CallCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name;
            //设置摄像机摄像的区域    
            webTex = new WebCamTexture(deviceName, 1024, 768, 20);
            webTex.Play();//开始摄像    
            transform.GetComponent<RawImage>().texture = webTex;
        }
    }


    // Update is called once per frame
    void Update()
    {
        CaptureScreen();
    }

    public float timer = 0;
    //截屏
    void CaptureScreen()
    {
        timer += Time.deltaTime;
        //每隔两秒检测一次
        if (timer >0.5 && player.isGround) //在地上且间隔大于1s
        {
            //删除上一次检测的图片
            // File.Delete(Application.streamingAssetsPath + "/capture.jpg");
            CapturePhoto();
            timer = 0;
        }
    }



    public int width;
    public int height;
    //截图摄像头
    public Camera cameras;
    public string fileName;
    public void CapturePhoto()
    {
        Texture2D screenShot;

        RenderTexture rt = new RenderTexture(width, height, 1);
        cameras.targetTexture = rt;
        cameras.Render();
        RenderTexture.active = rt;



        screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        //运行此行代码前，先手动在Asset路径下新建一个StreamingAsset文件夹
        fileName = Application.streamingAssetsPath + "/capture.jpg";
        // byte[] bytes = screenShot.EncodeToJPG();

        ScaleTextureCutOut(screenShot, 0, 0, 1024, 768);
        Debug.Log(string.Format("截屏了一张照片: {0}", fileName));

    }

    byte[] ScaleTextureCutOut(Texture2D originalTexture, int pos_x, int pos_y, float originalWidth, float originalHeight)
    {
        Color[] pixels = new Color[(int)(originalWidth * originalHeight)];
        //要返回的新图
        Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalWidth), Mathf.CeilToInt(originalHeight));
        //批量获取点像素
        pixels = originalTexture.GetPixels(pos_x, pos_y, (int)originalWidth, (int)originalHeight);
        newTexture.SetPixels(pixels);
        newTexture.anisoLevel = 2;
        newTexture.Apply();
        //这一步把裁剪的新图片存下来
        byte[] jpgData = newTexture.EncodeToJPG();

        try
        {
            System.IO.File.WriteAllBytes(fileName, jpgData);
            CameraFaceSearch(fileName);
        }
        catch (System.Exception)
        {
            throw;
        }
        return jpgData;
    }


    public Text text;

    //实时摄像头人脸画面对比
    public void CameraFaceSearch(string fileName)
    {
        try
        {
            FileInfo file = new FileInfo(fileName);
            var stream = file.OpenRead();
            byte[] buffer = new byte[file.Length];
            //读取图片字节流
            stream.Read(buffer, 0, Convert.ToInt32(file.Length));
            var image = Convert.ToBase64String(buffer);

            var imageType = "BASE64";


            var result = client.Detect(image, imageType);
            text.text = result.ToString();

            int idx = text.text.IndexOf("rotation\":") + 10;
            String tmp = "";
            while(text.text[idx] != '}')
            {
                tmp += text.text[idx];
                idx++;
            }
            float ans = float.Parse(tmp);
            if (ans < -30)
                needjump = true;
            else
                needjump = false;

            Debug.Log(result);
        }
         catch (System.Exception)
        {
            throw;
        }
    }


}
