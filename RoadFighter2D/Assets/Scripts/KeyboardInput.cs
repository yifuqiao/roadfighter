using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (InputManager.Instance.m_onLeft != null)
                InputManager.Instance.m_onLeft(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            if (InputManager.Instance.m_onLeft != null)
                InputManager.Instance.m_onLeft(false);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (InputManager.Instance.m_onRight != null)
                InputManager.Instance.m_onRight(true);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            if (InputManager.Instance.m_onRight != null)
                InputManager.Instance.m_onRight(false);
        }

    }
}
