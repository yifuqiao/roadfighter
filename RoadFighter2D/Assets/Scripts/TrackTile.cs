using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TrackTile : MonoBehaviour
{
    private float m_disapearDistance = 0f;
    [SerializeField] private Transform[] m_spawnPoints;
    


    void Awake()
    {
        m_disapearDistance = GetComponent<SpriteRenderer>().size.y * 2f;
    }
    void Update()
    {
        if (PlayerControl.Instance == null)
            return;
        if(PlayerControl.Instance.transform.position.y - transform.position.y > m_disapearDistance)
        {
            gameObject.SetActive(false);
            TrackGenerator.Instance.m_deadTrackList.Add(gameObject);
        }
    }

    public void Spawn(int index)
    {
        var pos = m_spawnPoints[index].position;
        TrackGenerator.Instance.SpawnNPCCar(pos.y, pos.x);
    }
}
