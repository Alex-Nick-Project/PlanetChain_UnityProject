using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]
    bool startLerp;

    [SerializeField]
    Vector3 finalPos, startPos;

    [SerializeField]
    CinemachineFreeLook vCam;

    public void SetTarget(Transform target)
    {
        startPos = transform.position;
        
        timer = 0;
        startLerp = true;
        finalPos = target.position;
        vCam.LookAt = target;
    }

    [SerializeField][Range(0.001f,1)]
    float degree;

    [SerializeField]
    AnimationCurve ac;

    float timer;

    // Update is called once per frame
    void Update()
    {
        if(startLerp)
        {
            timer += Time.deltaTime;
            
            var limit = ac.keys[ac.keys.Length - 1].time / degree;
            if(timer<=limit)
            {
                print(timer);
                transform.localPosition = Vector3.Lerp(startPos, finalPos, ac.Evaluate(timer*degree));
                
            }
            else
            {
                timer = 0;
                startLerp = false;
                vCam.Follow = transform;
            }

        }
    }
}
