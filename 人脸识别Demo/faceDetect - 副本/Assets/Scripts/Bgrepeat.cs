using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bgrepeat : MonoBehaviour
{
    Vector2 startpos;
    float repeatwith;
    // Start is called before the first frame update
    void Start()
    {
        repeatwith = 12f;
        startpos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startpos.x - repeatwith)
        {
            transform.position = startpos;
        }

    }
}
