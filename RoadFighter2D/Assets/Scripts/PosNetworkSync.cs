using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PosNetworkSync : MonoBehaviourPun, IPunObservable
{
    private float m_xPos = 0f;
    private bool m_bIsCalibrating = false;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(m_xPos);
            if (m_bIsCalibrating)
            {
                stream.SendNext(transform.parent.position.y);
            }
        }
        else
        {
            m_xPos = (float)stream.ReceiveNext();
            transform.localPosition = new Vector3(m_xPos,transform.localPosition.y, transform.localPosition.z);
            if (m_bIsCalibrating)
            {
                var yPos = (float)stream.ReceiveNext();
                transform.parent.position = new Vector3(transform.parent.position.x, yPos, transform.parent.position.z);
            }

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
            m_bIsCalibrating = true;
        }
    }

    [PunRPC]
    public void HitACar()
    {
        PlayerControl.OpponentInstance.OnCollidedWithCar();
    }
}
