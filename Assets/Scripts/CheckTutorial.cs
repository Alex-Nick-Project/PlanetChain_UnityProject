using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CheckTutorial : MonoBehaviour
{
    public static CheckTutorial instance;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void StartTutorial()
    {

        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            //tutorialPanel.transform.Find("Tutorial").gameObject.SetActive(true);
            
        }
    }


}
