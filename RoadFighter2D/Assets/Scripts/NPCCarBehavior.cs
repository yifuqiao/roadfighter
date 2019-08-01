using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCCarBehavior : MonoBehaviour
{
    public float m_speed = 2f;
    private bool m_bCanMove = false;
    private float yDiff = 0f;


    // Update is called once per frame
    void Update()
    {
        if(m_bCanMove)
        {
            transform.position += m_speed * Vector3.up * Time.deltaTime;
        }

        yDiff = -transform.position.y + PlayerControl.Instance.transform.position.y;

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
            TrackGenerator.Instance.m_deadCarList.Add(gameObject);
            m_bCanMove = false;
            return;
        }

        if (m_bCanMove && yDiff > TrackGenerator.Instance.TrackTileLength*0.5f)
        {
            gameObject.SetActive(false);
            TrackGenerator.Instance.m_deadCarList.Add(gameObject);
            m_bCanMove = false;
            return;
        }
    }
}
