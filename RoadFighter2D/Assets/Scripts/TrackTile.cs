using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTile : MonoBehaviour
{
    private float m_disapearDistance = 0f;   
    void Awake()
    {
        m_disapearDistance = GetComponent<SpriteRenderer>().size.y * 2f;
    }
    void Update()
    {
        if(PlayerControl.Instance.transform.position.y - transform.position.y > m_disapearDistance)
        {
            gameObject.SetActive(false);
            TrackGenerator.Instance.m_deadTrackList.Add(gameObject);
        }
    }
}
