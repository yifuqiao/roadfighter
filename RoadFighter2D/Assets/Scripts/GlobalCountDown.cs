﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.Demo.PunBasics;

public class GlobalCountDown : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private int m_countDownStart = 3;
    private float m_accuTime = 0f;
    public int[] m_randIndexList = new int[100];

    [PunRPC]
    public void SynRandomList(int[] randArray)
    {
        for (int i = 0; i < randArray.Length; ++i)
        {
            m_randIndexList[i] = randArray[i];
        }
        CopyGenerator();
    }

    void CopyGenerator()
    {
        for(int i = 0; i < m_randIndexList.Length;++i)
        {
            TrackGenerator.Instance.m_randIndexList[i] = m_randIndexList[i];
            TrackGenerator.OpponentInstance.m_randIndexList[i] = m_randIndexList[i];
        }
    }

    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < m_randIndexList.Length; ++i)
            {
                m_randIndexList[i] = (Random.Range(0, 5));
            }
            photonView.RPC("SynRandomList", RpcTarget.OthersBuffered, m_randIndexList);
            CopyGenerator();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.m_countDownStart);
        }
        else
        {
            // Network player, receive data
            this.m_countDownStart = (int)stream.ReceiveNext();
        }
    }
    bool played1 = false;
    bool played2 = false;
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            m_accuTime += Time.deltaTime;
            if(m_accuTime>=1f)
            {
                m_countDownStart -= 1;
                m_accuTime = 0;
                
                if(m_countDownStart<0)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
        m_text.text = m_countDownStart.ToString();
        if (m_countDownStart == 1 && played1==false)
        {
            played1 = true;
            MusicManager.Instance.MakeSFX(MusicManager.AudioType.CountDown0, PlayerControl.Instance.transform);
        }
       
        if (m_countDownStart == 0 )
        {
            if(played2 == false)
            {
                played2 = true;
                MusicManager.Instance.MakeSFX(MusicManager.AudioType.CountDown1, PlayerControl.Instance.transform);
            }
            m_text.text = "Go!";
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.GameOn = true;
    }
}




