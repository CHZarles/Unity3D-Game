using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float leftBound;
    private PlayerControler player;
    public float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControler>(); //获得控制权
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGameOver == false)
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
