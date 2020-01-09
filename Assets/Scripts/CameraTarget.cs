using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]
    bool startLerp;

    [SerializeField]
    Vector3 finalPos, startPos;

    public Transform SetTarget(Transform target)
    {
        startPos = transform.position;
        
        timer = 0;
        startLerp = true;
        finalPos = target.position;
        return target;
    }

    [SerializeField][Range(1,10)]
    float degree;

    [SerializeField]
    AnimationCurve ac;

    float timer;

    // Update is called once per frame
    void Update()
    {
        if(startLerp)
        {
            if(timer<=ac.keys[ac.keys.Length-1].time)
            {
                transform.position = Vector3.Lerp(startPos, finalPos, ac.Evaluate(timer/degree));
            }
            else
            {
                timer = 0;
            }

        }
    }
}
