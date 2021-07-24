using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private FaceDetect detecter;
    public float jumpforce = 10;
    public float GModify = 1.5f;
    public bool isGround = false;
    public bool isGameOver = false;
    private Rigidbody2D rb;

    void Start()
    {
        detecter =  FindObjectOfType<FaceDetect>(); 
        rb = GetComponent<Rigidbody2D>();
        Physics.gravity *= GModify;
    }

    // Update is called once per frame
    void Update()
    {//Input.GetKeyDown(KeyCode.Space)
        if (detecter.needjump && isGround)
        {
            rb.AddForce(Vector2.up * jumpforce);
            isGround = false;
            detecter.needjump = false;


        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("GameOver");

            isGameOver = true;

        }
    }

}
