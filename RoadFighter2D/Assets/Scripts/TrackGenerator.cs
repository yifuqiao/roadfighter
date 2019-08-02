using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class TrackGenerator : MonoBehaviourPun 
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] m_trackPrefabs=null;
    [SerializeField] private float[] m_trackPrefabSpawnPercentage=null;
    [SerializeField] private int m_trackTileBufferSize = 6;
    [SerializeField] private bool m_bIsOpponentTrackGenerator = false;
    

    [SerializeField] private GameObject m_NPCPrefab;
    public int[] m_randIndexList = new int[100];

    private float m_tileLength = 0f;
    private int m_newTileIndex = 0;

    public float TrackTileLength
    {
        get
        {
            return m_tileLength;
        }
    }

    public List<GameObject> m_deadTrackList = new List<GameObject>();
    public List<GameObject> m_deadCarList = new List<GameObject>();
    public static TrackGenerator Instance
    {
        private set; get;
    }
    public static TrackGenerator OpponentInstance
    {
        private set; get;
    }

    void Awake()
    {
        if (m_bIsOpponentTrackGenerator)
            OpponentInstance = this;
        else 
            Instance = this;

        m_tileLength = m_trackPrefabs[0].GetComponent<SpriteRenderer>().size.y;
        for ( m_newTileIndex = 0; m_newTileIndex < m_trackTileBufferSize; ++m_newTileIndex)
        {
            var go = (GameObject)Instantiate(m_trackPrefabs[0], new Vector3(transform.position.x, (float)m_newTileIndex * m_tileLength, transform.position.z), Quaternion.identity);
            if(m_bIsOpponentTrackGenerator)
            {
                go.GetComponent<TrackTile>().m_bIsOpponentTrack = true;
                go.name += "_opponent";
            }
            else
            {
                go.GetComponent<TrackTile>().m_bIsOpponentTrack = false;

                go.name += "_local";
            }
        }
    }

    public void SpawnNPCCar(float yPos, float xPos)
    {
        if (m_deadCarList.Count > 0)
        {
            m_deadCarList[0].transform.position = new Vector3(xPos, yPos, 0f);
            m_deadCarList[0].transform.rotation = Quaternion.identity;
            m_deadCarList[0].SetActive(true);
            m_deadCarList.RemoveAt(0);
        }
        else
        {
            var go = (GameObject)Instantiate(m_NPCPrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity);
            if (m_bIsOpponentTrackGenerator)
            {
                go.GetComponent<NPCCarBehavior>().m_bIsOpponentNPC = true;
                go.name += "_opponent";
            }
            else
            {
                go.GetComponent<NPCCarBehavior>().m_bIsOpponentNPC = false;
                go.name += "_local";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_deadTrackList.Count > 0)
        {
            m_deadTrackList[0].transform.position = new Vector3(transform.position.x, (float)m_newTileIndex * m_tileLength, transform.position.z);
            m_deadTrackList[0].SetActive(true);

            if (m_newTileIndex % 1 == 0)
            {
                var trackTile = m_deadTrackList[0].GetComponent<TrackTile>();
                trackTile.Spawn(m_randIndexList[m_newTileIndex % m_randIndexList.Length]);
            }

            m_deadTrackList.RemoveAt(0);
            m_newTileIndex++;
        }
    }
}
