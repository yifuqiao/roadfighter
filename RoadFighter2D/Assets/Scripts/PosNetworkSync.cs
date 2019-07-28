using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PosNetworkSync : MonoBehaviourPun, IPunObservable
{
    private float m_xPos = 0f;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(m_xPos);
        }
        else
        {
            m_xPos = (float)stream.ReceiveNext();
            transform.localPosition = new Vector3(m_xPos,transform.localPosition.y, transform.localPosition.z);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            m_xPos = transform.localPosition.x;
        }
    }
}
