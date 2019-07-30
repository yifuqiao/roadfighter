using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TrackGenerator : MonoBehaviourPun 
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] m_trackPrefabs=null;
    [SerializeField] private float[] m_trackPrefabSpawnPercentage=null;
    [SerializeField] private int m_trackTileBufferSize = 6;
    [SerializeField] private int[] m_randIndexList = new int[100];
    

    [SerializeField] private GameObject m_NPCPrefab;

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
    
    void Awake()
    {
        Instance = this;

        m_tileLength = m_trackPrefabs[0].GetComponent<SpriteRenderer>().size.y;
        for ( m_newTileIndex = 0; m_newTileIndex < m_trackTileBufferSize; ++m_newTileIndex)
        {
            Instantiate(m_trackPrefabs[0], new Vector3(0, (float)m_newTileIndex * m_tileLength, 0f), Quaternion.identity);
        }
        if(PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i < m_randIndexList.Length; ++i)
            {
                m_randIndexList[i]=(Random.Range(0, 5));
            }
            photonView.RPC("SynRandomList", RpcTarget.OthersBuffered, m_randIndexList);
        }
    }

    [PunRPC]
    public void SynRandomList(int[] randArray)
    {
        for (int i = 0; i < randArray.Length; ++i)
        {
            m_randIndexList[i] = randArray[i];
        }
        Destroy(GetComponent<PhotonView>());
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
            Instantiate(m_NPCPrefab, new Vector3(xPos, yPos, 0f), Quaternion.identity);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(m_deadTrackList.Count > 0)
        {
            m_deadTrackList[0].transform.position = new Vector3(0, (float)m_newTileIndex * m_tileLength,0f);
            m_deadTrackList[0].SetActive(true);

            var trackTile = m_deadTrackList[0].GetComponent<TrackTile>();
            
            trackTile.Spawn(m_randIndexList[m_newTileIndex% m_randIndexList.Length]);

            m_deadTrackList.RemoveAt(0);
            m_newTileIndex++;
            
        }
    }

    
}
