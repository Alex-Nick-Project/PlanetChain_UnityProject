using Nethereum.Util;
using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    public int stage;
    public bool rdyToNextState;

    [SerializeField]
    TextMeshProUGUI buttonText, usersText, timeText, ethText;
    [SerializeField]
    TMP_InputField numberText;
    private int id;
    private BigInteger WeiPrice;
    private decimal betETH;
    private bool timerStatus;
    private float timer;
    private bool insideRoom;
    private bool isPressed;
    public bool hasJoinned;
    public bool canAfford;
    public bool canBeClosed;
    private float timeNow;
    private float timeSinceCreation;
    private float timeOut;
    CameraTarget targetInstance;

    [SerializeField]
    Slider timerSlider;

    [SerializeField]
    private GameObject planetGO;

    private void Start()
    {
        timerSlider = transform.GetChild(0).GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    public void CreateInfo(int maxPlayers, int currentPlayers, int timeCreation, int _timeOut, BigInteger _WeiPrice, int _id, GameObject planet)
    {
        print("creting room info for ui");
        usersText.text = currentPlayers + " / " + maxPlayers;
        timeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        timeSinceCreation = timeNow - timeCreation;
        timeOut = (float)_timeOut;
        betETH = UnitConversion.Convert.FromWei(_WeiPrice);
        ethText.text = betETH * currentPlayers + " / " + betETH * maxPlayers;
        id = _id;
        WeiPrice = _WeiPrice;
        planetGO = planet;
        timerStatus = true;
        //timerSlider.maxValue = timeOut;

        var text = WalletManager.Instance.EtherBalanceText.text;
        decimal bal;
        decimal.TryParse(text, out bal);
        if (bal >= betETH)
        {
            canAfford = true;
        }
    }

    private void LateUpdate()
    {
        if(timerStatus)
        {
            timer = timeOut - timeSinceCreation;

            if (timer >= 0)
            {
                timer -= Time.fixedDeltaTime;
                timeText.text = timer.ToString();
                timerSlider.value = timer/timeOut;
            }
            else
            { 
                timeText.text = "Ready to be closed";
                timerStatus = false;
                canBeClosed = true;
                rdyToNextState = false;
                timerSlider.value = 0;
                UpdateButtonStatus();
            }
        }
       


    }

    void OnEnable()
    {
        targetInstance = FindObjectOfType<CameraTarget>();
        UpdateButtonStatus();
    }
    public void UpdateButtonStatus()
    {
        if (!isPressed)
        {

            isPressed = true;
            switch (stage)
            {
                case 0:
                    {
                        if (!rdyToNextState)
                        {
                            buttonText.text = "Check";
                            rdyToNextState = true;
                        }
                        else
                        {
                            if (!canAfford)
                            {
                                ethText.text = "Not enough funds";
                            }
                            
                            targetInstance.SetTarget(planetGO.transform);

                            // logic to move camera towards planet // should be working

                            // to do stage++; if player IS focusing on planet && can afford it && !insideRoom, then call this function || should be working

                            // to do stage == 2; if room can be closed, then call this function || should be working
                        }

                        isPressed = false;
                        return;
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

                            if (numberText.text != "" && luckyNumber >= 0 && luckyNumber <= 10)
                            {
                                getScripts.Instance.nameReturn(id, WeiPrice, luckyNumber);
                                hasJoinned = true;
                                buttonText.text = "Check";
                                var text = numberText.text;
                                var inputField = numberText.transform.GetChild(0).Find("Text").transform;
                                inputField.parent = numberText.transform.parent;
                                inputField.GetComponent<TextMeshProUGUI>().text = text;
                                numberText.gameObject.SetActive(false);
                                rdyToNextState = true;
                                stage = 0;
                                isPressed = false;
                                return;
                            }
                            else
                            {
                                buttonText.text = "Insert Number";
                                rdyToNextState = false;
                                Invoke("UpdateButtonStatus", 3);
                                isPressed = false;
                                return;
                            }
                        }

                        // to do stage--; if player is NOT on focus on planet // should be working

                        return;
                    }
                case 2:
                    {
                        if (!rdyToNextState)
                        {
                            // get a winner to display and ending/exploding animation //
                            buttonText.text = "Close";
                            rdyToNextState = true;
                        }
                        else
                        {
                            // display closed room data;
                        }


                        isPressed = false;
                        return;
                    }
            }
        }

    }
}
