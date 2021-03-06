using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFoller1 : MonoBehaviour
{
    // Start is called before the first frame update

    public void LookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 _targetPos = objectToFollow.position +
                             objectToFollow.forward * offset.z +
                             objectToFollow.right * offset.x +
                             objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.Tab))
        //offset = 
        LookAtTarget();
        MoveToTarget();
    }

    public Transform objectToFollow;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float followSpeed = 10;
    public float lookSpeed = 10;
}
