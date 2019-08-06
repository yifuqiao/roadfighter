using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.UI;

public class WaitForOpponentMessage : MonoBehaviour
{
    public Text m_waitingMessage;

    private void Update()
    {
        if(PhotonNetwork.CountOfPlayersInRooms >1)
        {
            Destroy(gameObject);
        }
    }
}
