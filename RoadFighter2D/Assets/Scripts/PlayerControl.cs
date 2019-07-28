using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class PlayerControl : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform m_car = null;
    [SerializeField] private Transform m_self = null;
    [SerializeField] private float m_leftRightMoveSpeed = 1.0f;
    [SerializeField] private float m_acceleration = 1.0f;
    [SerializeField] private float MAX_SPEED = 5f;
    private float m_currentSpeed = 0f;

    private bool m_bIsMovingLeft = false;
    private bool m_bIsMovingRight = false;
    private PhotonView m_pView;
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
        Explosion
    }

    void Awake()
    {
        m_pView = GetComponentInChildren<PhotonView>();
        if (m_pView.IsMine)
            Instance = this;
        else
            OpponentInstance=this;
    }

    void Start()
    {
        if (m_pView.IsMine == false)
        {
            Destroy(GetComponent<Camera>());
            Destroy(GetComponent<AudioListener>());
            tag = "Untagged";
        }
        else
        {
            InputManager.Instance.m_onLeft += OnLeft;
            InputManager.Instance.m_onRight += OnRight;
        }
    }

    private void OnLeft(bool isMoving)
    {
        if (isMoving == false)
            return;

        var offset = m_leftRightMoveSpeed * Time.deltaTime;
        m_car.localPosition -= new Vector3(offset, 0f, 0f);
    }

    private void OnRight(bool isMoving)
    {
        if (isMoving == false)
            return;

        var offset = m_leftRightMoveSpeed * Time.deltaTime;
        m_car.localPosition += new Vector3(offset, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameOn == false)
            return;

        if (m_currentSpeed < MAX_SPEED)
            m_currentSpeed += m_acceleration * Time.deltaTime;
        m_self.position += Vector3.up * m_currentSpeed * Time.deltaTime;
    }
}
