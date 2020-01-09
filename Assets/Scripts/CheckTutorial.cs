using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CheckTutorial : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {

        if (!PlayerPrefs.HasKey("Tutorial") || PlayerPrefs.GetInt("Tutorial") == 1)
        {
            //tutorialPanel.transform.Find("Tutorial").gameObject.SetActive(true);
            anim.SetTrigger("Tutorial");
        }
    }


}
