using System.Collections;
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
        if(m_countDownStart <= 0)
        {
            m_text.text = "Go!"; 
        }

        m_text.text = m_countDownStart.ToString();
    }

    void OnDestroy()
    {
        GameManager.Instance.GameOn = true;
    }
}




