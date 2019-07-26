using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] m_trackPrefabs=null;
    [SerializeField] private float[] m_trackPrefabSpawnPercentage=null;
    [SerializeField] private int m_trackTileBufferSize = 6;

    private float m_tileLength = 0f;
    private int m_newTileIndex = 0;

    public List<GameObject> m_deadTrackList = new List<GameObject>();
    public static TrackGenerator Instance
    {
        private set; get;
    }
    
    void Awake()
    {
        Instance = this;

        m_tileLength = m_trackPrefabs[0].GetComponent<SpriteRenderer>().size.y;
        for ( m_newTileIndex = 0; m_newTileIndex < m_trackTileBufferSize; ++m_newTileIndex)
        {
            GameObject.Instantiate(m_trackPrefabs[0], new Vector3(0, (float)m_newTileIndex * m_tileLength, 0f), Quaternion.identity);
        }
        
    }

   

    // Update is called once per frame
    void Update()
    {
        if(m_deadTrackList.Count>0)
        {
            m_deadTrackList[0].transform.position = new Vector3(0, (float)m_newTileIndex * m_tileLength,0f);
            m_deadTrackList[0].SetActive(true);
            m_deadTrackList.RemoveAt(0);
            m_newTileIndex++;
        }
    }
}
