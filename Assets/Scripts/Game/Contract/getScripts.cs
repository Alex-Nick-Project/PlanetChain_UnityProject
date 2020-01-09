using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using Nethereum.Util;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;


public class getScripts : MonoBehaviour
{
    [SerializeField]
    List<RoomInfo> totalRooms;
    [SerializeField]
    List<GameObject> currentPlanetRooms;
    [SerializeField]
    List<GameObject> currentUIRooms;

    [SerializeField] // deve esserci draggata la search box
    TMP_InputField searchInput;

    [SerializeField]
    [Range(1, 10)]

    float sensibility;
    float sens;

    [SerializeField]
    ItemSpawner itemSpawner;

    [SerializeField]
    GameObject roomUIPrefab;

    [SerializeField]
    Transform scrollViewContent;

    contractSetup contract = new contractSetup();


    private static getScripts _instance;

    public static getScripts Instance { get { return _instance; } }

    private string playerName = "";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        if(WalletManager.Instance.GetSelectedWalletData() != null)
        {
            B_openRooms();
        }
    }
    public void B_getMyRoom()
    {
        StartCoroutine("getMyRoom");
    }
    public void B_openRooms()
    {
        StartCoroutine("openRooms");
    }
    public void B_getRoomsData()
    {
        StartCoroutine("getRoomsData");
    }
    public void createRooms()
    {
        StartCoroutine(GetName(0, 0, 0, "CreateRoom"));
    }

    public IEnumerator getMyRoom()
    {
        var wait = 0;
        yield return new WaitForSeconds(wait);
        wait = 2;
        WalletData wd = WalletManager.Instance.GetSelectedWalletData();
        var getOpenRequest = new EthCallUnityRequest(WalletManager.Instance.networkUrl);
        if (wd.address != null)
        {
            var getOpenInput = contract.createGetMyRoomCallInput();
            yield return getOpenRequest.SendRequest(getOpenInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
            if (getOpenRequest.Exception == null)
            {
                var topScoreUser = contract.decodeGetOpen(getOpenRequest.Result);
                Debug.Log("No error");
                foreach (int i in topScoreUser)
                {

                }
                wait = 3;
            }
            else
            {
                Debug.Log("error");
                Debug.Log(getOpenRequest.Exception.ToString());
            }
        }
    }

    public IEnumerator openRooms()
    {
        var wait = 0;
        yield return new WaitForSeconds(wait);
        wait = 2;
        WalletData wd = WalletManager.Instance.GetSelectedWalletData();
        var getOpenRequest = new EthCallUnityRequest(WalletManager.Instance.networkUrl);
        if (wd.address != null)
        {
            var getOpenInput = contract.createGetOpenCallInput();
            yield return getOpenRequest.SendRequest(getOpenInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
            if (getOpenRequest.Exception == null)
            {
                var topScoreUser = contract.decodeGetOpen(getOpenRequest.Result);
                Debug.Log("No error");
                bool x = true;
                foreach (int index in topScoreUser)
                {
                    var getRoom = contract.createGetRoomCallInput(index);
                    yield return getOpenRequest.SendRequest(getRoom, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
                    if (getOpenRequest.Exception == null)
                    {
                        var roomsData = contract.decodeGetOpenRoom(getOpenRequest.Result);

                        AddLobbys(roomsData.maxPlayers, roomsData.Players, roomsData.timeCreated, roomsData.timeLeft, index, roomsData.Price, x);
                        x = false;
                        wait = 3;
                    }
                    else
                    {
                        Debug.Log("error");
                        Debug.Log(getOpenRequest.Exception.ToString());
                    }
                }

                wait = 3;
            }
            else
            {
                Debug.Log("error");
                Debug.Log(getOpenRequest.Exception.ToString());
            }
        }
    }

    private void AddLobbys(int maxPlayers, int currentPlayers, int timeCreation, int timeOut, int _id, BigInteger WeiPrice, bool clear = false)
    {
        
        var planetInstance = itemSpawner.updateRooms(maxPlayers-1); // qui' e' il problema, vorrei pescare il gameobject instanziato tramite la funzione, ma non lo prende e non capisco perche'

        RoomInfo ri = planetInstance.AddComponent<RoomInfo>();
        GameObject UIinstance = Instantiate(roomUIPrefab, scrollViewContent);

        ri.planet = planetInstance;
        ri.roomCap = maxPlayers;
        ri.roomCurrentPlayers = currentPlayers;
        ri.timeOfCreation = timeCreation;
        ri.timeLeft = timeOut;
        ri.roomID = _id;
        ri.roomPrice = WeiPrice;
        ri.ui = UIinstance;

        UIinstance.GetComponent<RoomUI>().CreateInfo(ri.roomCap, ri.roomCurrentPlayers, ri.timeOfCreation, ri.timeLeft, ri.roomPrice, ri.roomID, ri.planet);

        currentUIRooms.Add(ri.ui);
        currentPlanetRooms.Add(ri.planet);
        totalRooms.Add(ri);
        print(ri.planet = planetInstance);
    }
    public IEnumerator getRoomsData()
    {
        var wait = 0;
        yield return new WaitForSeconds(wait);
        wait = 2;
        WalletData wd = WalletManager.Instance.GetSelectedWalletData();
        var getOpenRequest = new EthCallUnityRequest(WalletManager.Instance.networkUrl);
        if (wd.address != null)
        {

        }
    }
    public IEnumerator GetName(int id, BigInteger bet, int prn, string selection = "JoinRoom")
    {
        var wait = 0;
        wait = 2;
        WalletData wd = WalletManager.Instance.GetSelectedWalletData();
        var getOpenRequest = new EthCallUnityRequest(WalletManager.Instance.networkUrl);
        if (wd.address != null)
        {
            var getNameInput = contract.createGetNameCallInput(wd.address);
            yield return getOpenRequest.SendRequest(getNameInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
            if (getOpenRequest.Exception == null)
            {
                var topScoreUser = contract.decodeGetName(getOpenRequest.Result);
                Debug.Log("No error");
                if (topScoreUser == "")
                {
                    Debug.Log(topScoreUser);
                    yield return transictionScripts.Instance.createUsers(wd.name);
                    switch (selection)
                    {
                        case "JoinRoom":
                            transictionScripts.Instance._JoinRoom(id, bet,prn);
                            break;
                        case "CreateRoom":
                            transictionScripts.Instance._CreateRoom();
                            break;
                    }

                }
                else
                {
                    switch (selection)
                    {
                        case "JoinRoom":
                            transictionScripts.Instance._JoinRoom(id, bet,prn);
                            break;
                        case "CreateRoom":
                            transictionScripts.Instance._CreateRoom();
                            break;
                    }
                }

            }
            else
            {
                Debug.Log("error");
                Debug.Log(getOpenRequest.Exception.ToString());
            }
        }
    }
    public void nameReturn(int id, BigInteger bet, int prn)
    {
        StartCoroutine(GetName(id, bet, prn));

    }
    // internal variables

    public void RemoveRoom(RoomInfo ri)
    {
        totalRooms.Remove(ri);
        currentPlanetRooms.Remove(ri.planet);
        currentUIRooms.Remove(ri.ui);
    }

    public void UpdateRoom() // to call on input field on change
    {
        foreach (RoomInfo ri in totalRooms)
        {
            if (searchInput.text != null)
            {
                sens = System.Convert.ToInt64(searchInput.text);
                sens /= sensibility;

                // if the searched price is in range
                if (Mathf.Approximately(sens, (int)ri.roomPrice + sens) || Mathf.Approximately(sens, (int)ri.roomPrice - sens))
                {
                    // doesn't remove the instance from UI scroll view
                    return;
                }
                else
                {
                    currentUIRooms.Remove(ri.ui);
                    // should have a function in the RoomUI, 
                    // that sets the GO ready to be used from others (instead of destroying and instantiating over and over again)
                }
            }
        }
    }
}
