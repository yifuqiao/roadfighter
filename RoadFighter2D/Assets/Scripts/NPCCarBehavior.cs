using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCCarBehavior : MonoBehaviour
{
    public float m_speed = 2f;
    private bool m_bCanMove = false;
    private float yDiff = 0f;
    public bool m_bIsOpponentNPC = false;
    private bool m_bIsCrashed = false;

    public void CollideWithWall()
    {
        m_bIsCrashed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerControl.Instance.m_currentState == PlayerControl.CarState.Won || PlayerControl.OpponentInstance.m_currentState == PlayerControl.CarState.Won)
        {
            gameObject.SetActive(false);

            if (m_bIsOpponentNPC)
                TrackGenerator.OpponentInstance.m_deadCarList.Add(gameObject);
            else
                TrackGenerator.Instance.m_deadCarList.Add(gameObject);

            m_bCanMove = false;
            return;
        }
        
        if(m_bCanMove)
        {
            transform.position += m_speed * Vector3.up * Time.deltaTime;
        }
        
        yDiff = -transform.position.y + (m_bIsOpponentNPC? PlayerControl.OpponentInstance.transform.position.y: PlayerControl.Instance.transform.position.y);

        //Debug.LogError("ydiff = " + yDiff + " | "+
        //    "transform.position.y = " + transform.position.y + " | "
        //    + "\nPlayerControl.Instance.transform.position.y = " + PlayerControl.Instance.transform.position.y + " | "
        //    + "\nTrackGenerator.Instance.TrackTileLength = " + TrackGenerator.Instance.TrackTileLength);

        if ( m_bCanMove == false && -yDiff < TrackGenerator.Instance.TrackTileLength * 0.5f)
        {
            m_bCanMove = true;
        }

        if(m_bCanMove && -yDiff > TrackGenerator.Instance.TrackTileLength * 0.51f)
        {
            gameObject.SetActive(false);

            if (m_bIsOpponentNPC)
                TrackGenerator.OpponentInstance.m_deadCarList.Add(gameObject);
            else
                TrackGenerator.Instance.m_deadCarList.Add(gameObject);

            m_bCanMove = false;
            return;
        }

        if (m_bCanMove && yDiff > TrackGenerator.Instance.TrackTileLength*0.5f)
        {
            gameObject.SetActive(false);

            if (m_bIsOpponentNPC)
                TrackGenerator.OpponentInstance.m_deadCarList.Add(gameObject);
            else
                TrackGenerator.Instance.m_deadCarList.Add(gameObject);

            m_bCanMove = false;
            return;
        }
    }
}
