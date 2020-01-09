using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using System.Linq;

public class RoomInfo : MonoBehaviour
{
    public string accountCreator;
    public int roomID;
    public int roomCap;
    public int roomCurrentPlayers;
    public BigInteger roomPrice;
    public bool roomValid, roomSentPRN;
    public int timeOfCreation, timeLeft;
    public GameObject planet, ui;
}

