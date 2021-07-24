using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayControl : MonoBehaviour
{
    //音效
    private AudioSource playerAudio;
    public AudioClip moneySound;

    //得分事件
    public delegate void PlayerScore(int temp);//定义委托
    public event PlayerScore GetScore;//定义得分事件，用于发出得分的消息

    //相机
    public GameObject camera0;
    public GameObject camera1;
    //横轴
    public float h;
    public float v;

    //角度
    private float angle ;

    public WheelCollider fl, fr, bl, br;

    public Transform flt, frt, blt, brt;

    //速度
    public float angleSpeed = 30f;

    public float speed = 500f;

    private void Start()
    {
        //开启相机1
        playerAudio = GetComponent<AudioSource>();
        camera0.SetActive(false);
        camera1.SetActive(true);
    }

    public void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Q))
        {
            camera0.SetActive(false);
            camera1.SetActive(true);
        }
        if (Input.GetKey(KeyCode.W))
        {
            camera1.SetActive(false);
            camera0.SetActive(true);
        }

       

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //左右按键影响angle,即转向
        angle = angleSpeed * h;
        fl.steerAngle = angle;
        fr.steerAngle = angle;

        //速度 
        fl.motorTorque = speed * v;
        fr.motorTorque = speed * v;


        UpdateWheel(flt, fl);
        UpdateWheel(frt, fr);
        UpdateWheel(blt, bl);
        UpdateWheel(brt, br);
    }

    void UpdateWheel(Transform trans , WheelCollider wheel)
    {
        Vector3 pos = trans.position;
        Quaternion rot = trans.rotation;
       
        //更新轮胎
        wheel.GetWorldPose(out pos,out rot);

        trans.position = pos;
        trans.rotation = rot;

        //刹车
        if(Input.GetKey(KeyCode.Space))
        {
            fl.brakeTorque = 1000f;
            fr.brakeTorque = 1000f;
            bl.brakeTorque = 1000f;
            br.brakeTorque = 1000f;
        }
        else
        {
            fl.brakeTorque = 0;
            fr.brakeTorque = 0;
            bl.brakeTorque = 0;
            br.brakeTorque = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

       if (other.gameObject.CompareTag("Money"))
        {
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

            if (GetScore != null)//检查事件是否为空，即有没有接收器订阅它
            {
                GetScore(1);//发送得分事件消息，为接收器提供参数1，实现+1分的效果
            }
        }
  
    }
}
