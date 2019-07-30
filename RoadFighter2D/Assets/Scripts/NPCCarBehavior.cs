using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCCarBehavior : MonoBehaviour
{
    public float m_speed = 2f;
    private bool m_bCanMove = false;

    private void OnBecameVisible()
    {
        m_bCanMove = true;

    }
    private void OnBecameInvisible()
    {
        m_bCanMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(m_bCanMove)
        {
            transform.position += m_speed * Vector3.up * Time.deltaTime;
        }

        var yDif = -transform.position.y + PlayerControl.Instance.transform.position.y;
        if (yDif > TrackGenerator.Instance.TrackTileLength * 2f)
        {
            gameObject.SetActive(false);
            TrackGenerator.Instance.m_deadCarList.Add(gameObject);
        }
    }
}
