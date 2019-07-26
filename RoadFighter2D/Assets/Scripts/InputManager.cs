using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager s_instance;
    public static InputManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    public System.Action<bool> m_onLeft;
    public System.Action<bool> m_onRight;

    private void Awake()
    {
        s_instance = this;
    }

    
}
