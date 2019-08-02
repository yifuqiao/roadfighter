using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTileCollider : MonoBehaviour
{
    [SerializeField] private TrackTile m_parentTrackTile;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_parentTrackTile.OnTriggerEnter2D(collision);
    }
}
