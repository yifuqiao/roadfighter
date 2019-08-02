using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TrackTile : MonoBehaviour
{
    private float m_disapearDistance = 0f;
    [SerializeField] private Transform[] m_spawnPoints;
    [SerializeField] public bool m_bIsOpponentTrack = false;


    void Awake()
    {
        m_disapearDistance = GetComponent<SpriteRenderer>().size.y * 2f;
    }
    void Update()
    {
        if (PlayerControl.Instance == null || PlayerControl.OpponentInstance == null)
            return;
        if( (m_bIsOpponentTrack? PlayerControl.OpponentInstance.transform.position.y: PlayerControl.Instance.transform.position.y) - transform.position.y > m_disapearDistance)
        {
            gameObject.SetActive(false);
            if (m_bIsOpponentTrack)
                TrackGenerator.OpponentInstance.m_deadTrackList.Add(gameObject);
            else
                TrackGenerator.Instance.m_deadTrackList.Add(gameObject);
        }
    }

    public void Spawn(int index)
    {
        var pos = m_spawnPoints[index].position;
        if (m_bIsOpponentTrack)
            TrackGenerator.OpponentInstance.SpawnNPCCar(pos.y, pos.x);
        else
            TrackGenerator.Instance.SpawnNPCCar(pos.y, pos.x);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("NPC_Car"))
        {

        }
        else if(collision.CompareTag("Player"))
        {
            PlayerControl.Instance.OnCollidedWithWall();
        }
    }
}
