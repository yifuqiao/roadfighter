using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform m_car = null;
    [SerializeField] private Transform m_self = null;
    [SerializeField] private float m_leftRightMoveSpeed = 1.0f;
    [SerializeField] private float m_acceleration = 1.0f;
    [SerializeField] private float m_curAcceleration = 1f;
    [SerializeField] private float m_deceleration = -2f;
    [SerializeField] private float m_collisionDecelerationTimer = 1f;
    [SerializeField] private float m_collisionDecelerationAccuTime = 0f;
    [SerializeField] public CarState m_currentState = CarState.InitialLaunch;
    [SerializeField] private float MAX_SPEED = 5f;
    [SerializeField] private float m_currentSpeed = 0f;
    [SerializeField] private Rect m_localViewPortRect;
    [SerializeField] private Rect m_remoteViewPortRect;
    [SerializeField] private int m_maxLife = 3;
    [SerializeField] private GameObject[] m_lifeArray;

    [SerializeField] public Text m_text;
    [SerializeField] public GameObject m_winLosePanel;

    [SerializeField] public Text m_coinCount;
    [SerializeField] private Text m_coinCountLabel;

    public Button m_rematchButton;
    public Button m_returnButton;

    private bool m_bModifiedCoin = false;
    private bool m_bIsMovingLeft = false;
    private bool m_bIsMovingRight = false;
    private PosNetworkSync m_posNetworkSync;
    public Material m_transparentMaterial;
    public GameObject m_explosionAnimation;

    public static PlayerControl Instance
    {
        private set; get;
    }
    public static PlayerControl OpponentInstance
    {
        private set; get;
    }



    public float OnScreenXPos
    {
        get
        {
            Vector3 screenPos = GetComponent<Camera>().WorldToScreenPoint(m_car.position);
            return screenPos.x;
        }
    }

    public enum CarState
    {
        InitialLaunch,
        Forward,
        Collision,
        Explosion,
        Won
    }

    void Awake()
    {
        m_posNetworkSync = GetComponentInChildren<PosNetworkSync>();
        if (m_posNetworkSync.photonView.IsMine)
        {
            Instance = this;
            gameObject.name += "_local";
        }
        else
        {
            m_car.GetComponent<SpriteRenderer>().material = m_transparentMaterial;
            OpponentInstance = this;
            gameObject.name += "_opponent";

        }
    }

    void Start()
    {
        if (m_posNetworkSync.photonView.IsMine == false)
        {
            Destroy(GetComponent<AudioListener>());
            tag = "Untagged";
            GetComponent<Camera>().rect = m_remoteViewPortRect;
            m_coinCountLabel.gameObject.SetActive(false);
            m_coinCount.gameObject.SetActive(false);
            m_rematchButton.gameObject.SetActive(false);
            m_returnButton.gameObject.SetActive(false);
        }
        else
        {
            InputManager.Instance.m_onLeft += OnLeft;
            InputManager.Instance.m_onRight += OnRight;
            GetComponent<Camera>().rect = m_localViewPortRect;
            GetComponent<Camera>().depth = 0;
            m_coinCount.text = UserProfileManager.Instance.GetUserGameCoin().ToString();
            m_rematchButton.onClick.AddListener(new UnityAction(GameManager.Instance.LeaveRoom));
            m_returnButton.onClick.AddListener(new UnityAction(Application.Quit));
        }
    }

    private void OnLeft(bool isMoving)
    {
        if (isMoving == false)
            return;
        if (m_currentState == CarState.InitialLaunch)
            return;
        if (m_currentState == CarState.Collision)
            return;
        if (m_currentState == CarState.Explosion)
            return;
        if (m_currentState == CarState.Won)
            return;

        var offset = m_leftRightMoveSpeed * Time.deltaTime;
        m_car.localPosition -= new Vector3(offset, 0f, 0f);
    }

    private void OnRight(bool isMoving)
    {
        if (isMoving == false)
            return;
        if (m_currentState == CarState.InitialLaunch)
            return;
        if (m_currentState == CarState.Collision)
            return;
        if (m_currentState == CarState.Explosion)
            return;
        if (m_currentState == CarState.Won)
            return;

        var offset = m_leftRightMoveSpeed * Time.deltaTime;
        m_car.localPosition += new Vector3(offset, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_car == null || transform == null)
            return;
        switch(m_currentState)
        {
            case CarState.InitialLaunch:
                if (m_car.GetComponent<PhotonView>().IsMine == false)
                    transform.position = new Vector3(10000f, transform.position.y, transform.position.z);

                if (GameManager.Instance.GameOn)
                {
                    if(m_posNetworkSync.photonView.IsMine)
                        UserProfileManager.Instance.ModifyUserCoin(-1);
                    m_currentState = CarState.Forward;
                }
                break;
            case CarState.Forward:
                if (m_currentSpeed < MAX_SPEED)
                    m_currentSpeed += m_acceleration * Time.deltaTime;
                m_self.position += Vector3.up * m_currentSpeed * Time.deltaTime;
                break;
            case CarState.Collision:
                if (m_maxLife == 0 && Instance == this)
                {
                    OnCollidedWithWall();
                    return;
                }
                if (m_currentSpeed > 0f )
                    m_currentSpeed += m_deceleration * Time.deltaTime;
                m_self.position += Vector3.up * m_currentSpeed * Time.deltaTime;
                m_collisionDecelerationAccuTime += Time.deltaTime;

                m_car.GetComponent<SpriteRenderer>().enabled= (Time.frameCount % 2) == 1;

                if (m_collisionDecelerationAccuTime >= m_collisionDecelerationTimer)
                {
                    m_car.GetComponent<SpriteRenderer>().enabled = true;
                    m_currentState = CarState.Forward;
                    m_collisionDecelerationAccuTime = 0f;
                }
                
                break;
            case CarState.Explosion:
                break;
            case CarState.Won:
                if (m_currentSpeed < MAX_SPEED)
                    m_currentSpeed += m_acceleration * Time.deltaTime;
                m_self.position += Vector3.up * m_currentSpeed * Time.deltaTime;
                if(m_bModifiedCoin==false)
                {
                    m_bModifiedCoin = true;
                    if (m_posNetworkSync.photonView.IsMine)
                        UserProfileManager.Instance.ModifyUserCoin(2);
                }
                break;
        }
    }

    public void OnCollidedWithCar()
    {
        if(m_currentState == CarState.Forward)
            m_maxLife--;

        m_currentState = CarState.Collision;

        if (m_maxLife>=0)
            m_lifeArray[m_maxLife].SetActive(false);

        
    }

    public void OnCollidedWithWall()
    {
        ExplodeNow();
        m_posNetworkSync.LocalExplosion();
        m_posNetworkSync.SyncEndGameMessage(0, 1);
        m_text.text = "You Lost!";
        m_winLosePanel.SetActive(true);
        OpponentInstance.m_text.text = "Opponent Won!";
        OpponentInstance.m_winLosePanel.SetActive(true);
        OpponentInstance.m_currentState = CarState.Won;
    }

    public void ExplodeNow()
    {
        m_currentState = CarState.Explosion;
        m_explosionAnimation.transform.position = m_car.position;
        m_explosionAnimation.SetActive(true);
        m_car.gameObject.SetActive(false);
    }
}
