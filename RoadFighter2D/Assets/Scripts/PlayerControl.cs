using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

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
    [SerializeField] private CarState m_currentState = CarState.InitialLaunch;
    [SerializeField] private float MAX_SPEED = 5f;
    [SerializeField] private float m_currentSpeed = 0f;
    private bool m_bIsMovingLeft = false;
    private bool m_bIsMovingRight = false;
    private PhotonView m_pView;
    public Material m_transparentMaterial;
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
        {
            Instance = this;
        }
        else
        {
            m_car.GetComponent<SpriteRenderer>().material = m_transparentMaterial;
            OpponentInstance = this;
        }
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
        if (m_currentState == CarState.InitialLaunch)
            return;
        if (m_currentState == CarState.Collision)
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

        var offset = m_leftRightMoveSpeed * Time.deltaTime;
        m_car.localPosition += new Vector3(offset, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_currentState)
        {
            case CarState.InitialLaunch:
                if (PhotonNetwork.IsMasterClient)
                    m_car.localPosition = new Vector3(-0.15f, 0f, 10f);
                else
                    m_car.localPosition = new Vector3(0.15f,0f,10f);
                if (GameManager.Instance.GameOn)
                    m_currentState = CarState.Forward;
                break;
            case CarState.Forward:
                if (m_currentSpeed < MAX_SPEED)
                    m_currentSpeed += m_acceleration * Time.deltaTime;
                m_self.position += Vector3.up * m_currentSpeed * Time.deltaTime;
                break;
            case CarState.Collision:
                if (m_currentSpeed > 0f )
                    m_currentSpeed += m_deceleration * Time.deltaTime;
                m_self.position += Vector3.up * m_currentSpeed * Time.deltaTime;
                m_collisionDecelerationAccuTime += Time.deltaTime;
                if (m_collisionDecelerationAccuTime >= m_collisionDecelerationTimer)
                {
                    m_currentState = CarState.Forward;
                    m_collisionDecelerationAccuTime = 0f;
                }
                break;
            case CarState.Explosion:
                break;
        }
    }

    public void OnCollidedWithCar()
    {
        m_currentState = CarState.Collision;
    }
}
