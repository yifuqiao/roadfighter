﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TrackTile : MonoBehaviour
{
    private float m_disapearDistance = 0f;
    [SerializeField] private Transform[] m_leftSpawnRegionAnchors;
    [SerializeField] private Transform[] m_rightSpawnRegionAnchors;


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

    public void Spawn()
    {
        var distance = PlayerControl.Instance.transform.position.y - PlayerControl.OpponentInstance.transform.position.y;
        if (distance > 1f)
        {
            var index = Random.Range(0, m_leftSpawnRegionAnchors.Length);
            var anchorLeft = m_leftSpawnRegionAnchors[index];
            var anchorRight = m_rightSpawnRegionAnchors[index];

            var spawnX = Random.Range(anchorLeft.position.x, anchorRight.position.x);

            TrackGenerator.Instance.SpawnNPCCar(m_leftSpawnRegionAnchors[index].position.y, spawnX);
        }
        else if(Mathf.Abs(distance) <= 1f && PhotonNetwork.IsMasterClient)
        {
            var index = Random.Range(0, m_leftSpawnRegionAnchors.Length);
            var anchorLeft = m_leftSpawnRegionAnchors[index];
            var anchorRight = m_rightSpawnRegionAnchors[index];

            var spawnX = Random.Range(anchorLeft.position.x, anchorRight.position.x);

            TrackGenerator.Instance.SpawnNPCCar(m_leftSpawnRegionAnchors[index].position.y, spawnX);
        }
    }
}