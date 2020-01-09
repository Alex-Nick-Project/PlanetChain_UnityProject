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

public class transictionScripts : MonoBehaviour
{
    private static transictionScripts _instance;

    public static transictionScripts Instance { get { return _instance; } }

    private string username = "";

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
    [SerializeField]
    InputField maxPlayers;
    [SerializeField]
    InputField timer;
    [SerializeField]
    InputField bet;
    [SerializeField]
    InputField prn;

    contractSetup contract = new contractSetup();

    public void B_createUser()
    {
        StartCoroutine("createUsers");
    }
    public void _CreateRoom()
    {
        int _maxPlayers = int.Parse(maxPlayers.text);
        int _timer = int.Parse(timer.text);
        float _bet = float.Parse(bet.text);
        int _prn = int.Parse(prn.text);
        if ((_maxPlayers != 0) && (_timer != 0) && (_bet != 0) && (_prn != null)) {
            Debug.Log("Ottimo");
            StartCoroutine(createRooms(_maxPlayers,_timer,_prn,_bet));
        }
        else
        {
            Debug.Log("Errore non tutti i campi sono specifici");
        }
        
    }
    public void _JoinRoom(int id, BigInteger weiPrice, int prn)
    {
        StartCoroutine(JoinRooms(id, weiPrice, prn));
    }


    public IEnumerator createUsers(string name)
    {
        bool finish = false;
        int pin = 4321;
        object[] array = new object[] { name, pin };
        var wait = 0;
        yield return new WaitForSeconds(wait);
        WalletData wd = WalletManager.Instance.GetSelectedWalletData();
        wait = 2;

        if (wd.address != null)
        {
            var transactionInput = contract.createUserInput(wd.address, wd.privateKey, new HexBigInteger(4712388), name, pin);
            var transactionSignedRequest = new TransactionSignedUnityRequest(WalletManager.Instance.networkUrl, wd.privateKey);
            yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
            if (transactionSignedRequest.Exception == null)
            {
                var transactionHash = transactionSignedRequest.Result;
                Debug.Log("Sto creando l'user wait:" + transactionHash);

                //create a poll to get the receipt when mined
                var transactionReceiptPolling = new TransactionReceiptPollingRequest(WalletManager.Instance.networkUrl);
                //checking every 2 seconds for the receipt
                yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);

                if (transactionReceiptPolling.Exception == null)
                {

                    Debug.Log("Good Mined");
                }
                else
                {
                    Debug.Log("Bad Mined");
                }
            }
            else
            {
                Debug.Log("error");
                Debug.Log(transactionSignedRequest.Exception.ToString());
            }
        }
    }

    public IEnumerator createRooms(int maxPlayers,int timer,int prn,float bet)
    {
        BigInteger _bet = UnitConversion.Convert.ToWei(bet);
        var wait = 0;
        yield return new WaitForSeconds(wait);
        wait = 2;
        WalletData wd = WalletManager.Instance.GetSelectedWalletData();
        if (wd.address != null)
        {
            var transactionInput = contract.createRoomInput(wd.address, wd.privateKey, maxPlayers, timer, prn, new HexBigInteger(3000000), new HexBigInteger(_bet));
            var transactionSignedRequest = new TransactionSignedUnityRequest(WalletManager.Instance.networkUrl, wd.privateKey);
            yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
            if (transactionSignedRequest.Exception == null)
            {
                var transactionHash = transactionSignedRequest.Result;

                Debug.Log("Sto creando la room:" + transactionHash);

                //create a poll to get the receipt when mined
                var transactionReceiptPolling = new TransactionReceiptPollingRequest(WalletManager.Instance.networkUrl);
                //checking every 2 seconds for the receipt
                yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);

                if (transactionReceiptPolling.Exception == null)
                {
                    Debug.Log("Good Mined");
                }
                else
                {
                    Debug.Log("Bad Mined");
                }
            }
            else
            {
                Debug.Log("error");
                Debug.Log(transactionSignedRequest.Exception.ToString());
            }
        }
    }

    public IEnumerator JoinRooms(int id, BigInteger weiPrice, int prn)
    {
        var wait = 0;
        yield return new WaitForSeconds(wait);
        wait = 2;
        WalletData wd = WalletManager.Instance.GetSelectedWalletData();
        if (wd.address != null)
        {
            var transactionInput = contract.joinRoomInput(wd.address, wd.privateKey, id, prn, new HexBigInteger(3000000), new HexBigInteger(weiPrice));

            var transactionSignedRequest = new TransactionSignedUnityRequest(WalletManager.Instance.networkUrl, wd.privateKey);
            yield return transactionSignedRequest.SignAndSendTransaction(transactionInput);
            Debug.Log(wd.address);
            if (transactionSignedRequest.Exception == null)
            {
                var transactionHash = transactionSignedRequest.Result;

                Debug.Log("Join Room:" + transactionHash);

                //create a poll to get the receipt when mined
                var transactionReceiptPolling = new TransactionReceiptPollingRequest(WalletManager.Instance.networkUrl);
                //checking every 2 seconds for the receipt
                yield return transactionReceiptPolling.PollForReceipt(transactionHash, 2);

                if (transactionReceiptPolling.Exception == null)
                {
                    WalletManager.Instance.RefreshTopPanelView();
                    Debug.Log("Good Mined");
                }
                else
                {
                    Debug.Log("Bad Mined");
                }
            }
            else
            {
                Debug.Log("error");
                Debug.Log(transactionSignedRequest.Exception.ToString());
            }
        }
    }
}
