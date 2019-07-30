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
        if(m_bCanMove == true)
        {
            gameObject.SetActive(false);
            TrackGenerator.Instance.m_deadCarList.Add(gameObject);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bCanMove)
        {
            transform.position += m_speed * Vector3.up * Time.deltaTime;
        }
    }
}
