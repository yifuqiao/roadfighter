using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private float CarXPos
    {
        get
        {
            return PlayerControl.Instance.OnScreenXPos;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerControl.Instance == null)
            return;

		if (Input.touchCount > 0)
		{
            if (Input.touchCount == 1 && Input.GetTouch(0).position.x <= CarXPos)
            {
                if (InputManager.Instance.m_onLeft != null)
                    InputManager.Instance.m_onLeft(true);
            }
            if (Input.touchCount == 1 && Input.GetTouch(0).position.x > CarXPos)
            {
                if (InputManager.Instance.m_onRight != null)
                    InputManager.Instance.m_onRight(true);
            }
            if(Input.touchCount >1)
            {
                if (InputManager.Instance.m_onLeft != null)
                    InputManager.Instance.m_onLeft(true);
                if (InputManager.Instance.m_onRight != null)
                    InputManager.Instance.m_onRight(true);
            }
            
        }
        else
        {
            if (InputManager.Instance.m_onLeft != null)
                InputManager.Instance.m_onLeft(false);
            if (InputManager.Instance.m_onRight != null)
                InputManager.Instance.m_onRight(false);
        }
	}
}
