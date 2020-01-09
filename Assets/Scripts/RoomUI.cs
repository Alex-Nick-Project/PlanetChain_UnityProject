using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Nethereum.Util;
using System;

public class RoomUI : MonoBehaviour
{
    public int stage;

    [SerializeField]
    TextMeshProUGUI buttonText, usersText, timeText, ethText;
    [SerializeField]
    TMP_InputField numberText;
    private int id;
    private BigInteger WeiPrice;
    private decimal betETH;
    private bool rdyToNextState;
    private bool timerStatus;
    private float timer;
    private bool insideRoom;

    [SerializeField]
    private GameObject planetGO;

    // Start is called before the first frame update
    public void CreateInfo(int maxPlayers, int currentPlayers, int timeCreation, int timeOut, BigInteger _WeiPrice, int _id, GameObject planet)
    {
        usersText.text = currentPlayers + " / " + maxPlayers;
        timer = (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - (timeCreation + timeOut));
        timeText.text = timer.ToString();
        timerStatus = true;
        betETH = UnitConversion.Convert.FromWei(_WeiPrice);
        ethText.text = betETH * currentPlayers + " / " + betETH * maxPlayers;
        id = _id;
        WeiPrice = _WeiPrice;
        planetGO = planet;
    }

    private void LateUpdate()
    {
        if(timerStatus)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timerStatus = false;
                stage = 2;
                rdyToNextState = false;
                UpdateButtonStatus();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Planet" && !insideRoom /* && check if enough balance*/)
        {
            stage = 1;
            rdyToNextState = false;
            UpdateButtonStatus();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Planet")
        {
            stage = 0;
            rdyToNextState = false;
            UpdateButtonStatus();
        }
    }

    void Start()
    {
        UpdateButtonStatus();
    }
    public void UpdateButtonStatus()
    {
        switch (stage)
        {
            case 0:
                {
                    if(!rdyToNextState)
                    {
                        buttonText.text = "Check";
                        rdyToNextState = true;
                    }
                    else
                    {
                        var targetInstance = FindObjectOfType<CameraTarget>();
                        targetInstance.SetTarget(planetGO.transform);

                        // logic to move camera towards planet

                        // to do stage++; if player IS focusing on planet && can afford it && !insideRoom, then call this function || should be working

                        // to do stage == 2; if room can be closed, then call this function || should be working
                    }

                    break;
                }
            case 1:
                {
                    if (!rdyToNextState)
                    {
                        buttonText.text = "Join";
                        rdyToNextState = true;
                    }
                    else
                    {
                        int luckyNumber;
                        int.TryParse(numberText.text, out luckyNumber);

                        if (numberText != null && luckyNumber >= 0 && luckyNumber <= 10)
                        {
                            getScripts.Instance.nameReturn(id, WeiPrice, luckyNumber);
                            buttonText.text = "Check";
                            var inputField = numberText.transform.GetChild(0).GetChild(1).transform;
                            inputField.SetParent(numberText.transform.parent);
                            numberText.gameObject.SetActive(false);
                            rdyToNextState = true;
                            stage = 0;
                            return;
                        }
                        else
                        {
                            buttonText.text = "Insert Number";
                            rdyToNextState = false;
                            Invoke("UpdateButtonStatus", 3);
                            return;
                        }
                    }

                    // to do stage--; if player is NOT on focus on planet

                    break;
                }
            case 2:
                {
                    if(!rdyToNextState)
                    {
                        // close call //

                        // get a winner to display and ending/exploding animation //
                        rdyToNextState = true;
                    }
                    else
                    {
                        // display closed room data;
                    }

                    

                    break;
                }
        }
    }

    void Update()
    {

    }
}
