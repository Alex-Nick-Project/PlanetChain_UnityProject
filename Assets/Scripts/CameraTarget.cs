using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]
    CinemachineFreeLook planCam, roamCam;

    [SerializeField]
    bool moveToPlanet;

    [SerializeField]
    RoomUI rUI;

    public void SetTarget(Transform target)
    {
        print("setting target");
        CamFunction(roamCam, target);
        roamCam.transform.position = planCam.transform.position;
        roamCam.transform.rotation = planCam.transform.rotation;

        roamCam.gameObject.SetActive(true);
        planCam.gameObject.SetActive(false);
        moveToPlanet = true;

        var x = target.GetComponent<RoomInfo>().ui;
        rUI = x.GetComponent<RoomUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Planet" /*&& !insideRoom && check if enough balance*/)
        {
            CamFunction(planCam, other.transform);
            planCam.gameObject.SetActive(true);
            roamCam.gameObject.SetActive(false);

            moveToPlanet = false;

            if (rUI.canBeClosed)
            {
                rUI.stage = 2;
                rUI.rdyToNextState = false;
                rUI.UpdateButtonStatus();
            }
            else if (rUI.canAfford && !rUI.hasJoinned && !rUI.canBeClosed)
            {
                rUI.stage = 1;
                rUI.rdyToNextState = false;
                rUI.UpdateButtonStatus();
            }
            else
            {
                rUI.stage = 0;
                rUI.rdyToNextState = false;
                rUI.UpdateButtonStatus();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        //isFollowing = false; to be set on destination reached
        rUI.stage = 0;
            rUI.rdyToNextState = false;
            rUI.UpdateButtonStatus();
            
    }

    void CamFunction(CinemachineFreeLook cam, Transform target)
    {
        cam.Follow = target;
        cam.LookAt = target;
    }

    private void LateUpdate()
    {
        if (moveToPlanet)
        {
            transform.position = Camera.main.transform.position;
        }
    }
}
