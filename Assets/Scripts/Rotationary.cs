using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotationary : MonoBehaviour
{
    public bool isRotationary = true;

    public float rotSpeed = 10;
    Quaternion x;
    private void Start()
    {
        if (isRotationary)
        {
            var desiredRotQ = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            transform.localRotation = desiredRotQ;
        }

    }
    private void LateUpdate()
    {
        if(!isRotationary)
        {
            transform.Rotate(-transform.up * Time.fixedDeltaTime * rotSpeed*50);
        }
        else
        {
            transform.parent.Rotate(-transform.parent.up * Time.fixedDeltaTime * rotSpeed);
        }

    }

}


