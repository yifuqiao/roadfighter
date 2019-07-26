using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform m_car = null;
    [SerializeField] private Transform m_self = null;
    [SerializeField] private float m_leftRightMoveSpeed = 1.0f;
    [SerializeField] private float m_acceleration = 1.0f;
    [SerializeField] private float MAX_SPEED = 5f;
    private float m_currentSpeed = 0f;

    private bool m_bIsMovingLeft = false;
    private bool m_bIsMovingRight = false;

    public static PlayerControl Instance
    {
        private set; get;
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
        Instance = this;
    }

    void Start()
    {
        InputManager.Instance.m_onLeft += OnLeft;
        InputManager.Instance.m_onRight += OnRight;
    }

    private void OnLeft(bool isMoving)
    {
        m_bIsMovingLeft = isMoving;
    }

    private void OnRight(bool isMoving)
    {
        m_bIsMovingRight = isMoving;
    }

    // Update is called once per frame
    void Update()
    {
        var offset = m_leftRightMoveSpeed * Time.deltaTime;
        if (m_bIsMovingLeft)
        {
            m_car.localPosition -= new Vector3(offset, 0f, 0f);
        }
        if (m_bIsMovingRight)
        {
            m_car.localPosition += new Vector3(offset, 0f, 0f);
        }

        if (m_currentSpeed < MAX_SPEED)
            m_currentSpeed += m_acceleration * Time.deltaTime;
        m_self.position += Vector3.up * m_currentSpeed * Time.deltaTime;
    }
}
