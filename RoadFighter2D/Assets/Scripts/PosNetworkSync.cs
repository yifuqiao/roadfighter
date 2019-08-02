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
        else
        {
            Destroy(this.GetComponent<Collider2D>());
            Destroy(this.GetComponent<Rigidbody2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.CompareTag("NPC_Car"))
        {
            photonView.RPC("HitACar", RpcTarget.OthersBuffered);
            PlayerControl.Instance.OnCollidedWithCar();
        }
    }

    [PunRPC]
    public void HitACar()
    {
        PlayerControl.OpponentInstance.OnCollidedWithCar();
    }

    public void LocalExplosion()
    {
        photonView.RPC("RemoteExplosion",RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void RemoteExplosion()
    {
        PlayerControl.OpponentInstance.ExplodeNow();
    }


    public void SyncEndGameMessage(int myWinningResult,int opponentWinningResult)
    {
        photonView.RPC("SyncEndGameMessageRPC", RpcTarget.OthersBuffered, myWinningResult, opponentWinningResult);
    }

    [PunRPC]
    public void SyncEndGameMessageRPC(int myWindowMessage, int opponentWindowMessage)
    {
        PlayerControl.OpponentInstance.m_text.text = (myWindowMessage == 1) ? "Opponent Won!" : "Opponent Lost!";
        PlayerControl.Instance.m_text.text = (opponentWindowMessage == 1) ? "You Won!" : "You Lost!";
        PlayerControl.OpponentInstance.m_winLosePanel.SetActive(true);
        PlayerControl.Instance.m_winLosePanel.SetActive(true);

        PlayerControl.OpponentInstance.m_currentState = (myWindowMessage == 1) ? PlayerControl.CarState.Won : PlayerControl.OpponentInstance.m_currentState;
        PlayerControl.Instance.m_currentState = (opponentWindowMessage == 1) ? PlayerControl.CarState.Won : PlayerControl.Instance.m_currentState;
    }
}
