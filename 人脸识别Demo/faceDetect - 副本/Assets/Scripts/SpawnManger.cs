using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManger : MonoBehaviour
{
    private PlayerControler player;

    public GameObject[] obs;
    public Vector2 spawmpos = new Vector2(-3, 2.48f);
    // Start is called before the first frame update
    public float start_delay, repaeat_delay;
    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), start_delay, repaeat_delay);
        player = FindObjectOfType<PlayerControler>(); //获得控制权
    }

    // Update is called once per frame
    void Update()
    {

    }

    //生成的函数
    public void SpawnObstacle()
    {
        int index = Random.Range(0, obs.Length);
        if (player.isGameOver == false)
            Instantiate(obs[index], spawmpos, Quaternion.identity);//不旋转


    }
}
