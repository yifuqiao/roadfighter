using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCue : MonoBehaviour
{
	[SerializeField] private AudioSource m_audioSource;
	private Transform m_targetTransform=null;
	private AudioClip m_clip=null;
	private bool m_isDead = false;
	private float m_accuTime = 0f;

	public System.Action<SoundCue> m_OnSoundCueDead;
    
	public void InitSoundCue(Transform target, AudioClip clip)
	{
		m_targetTransform = target;
		m_clip = clip;

		m_accuTime = 0f;
        m_isDead = false;
        gameObject.SetActive(true);

		if (m_clip != null)
		{
			m_audioSource.clip = null;
			m_audioSource.clip = m_clip;
			m_audioSource.PlayDelayed(0f);
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (m_targetTransform != null)
			transform.position = m_targetTransform.position;
		
		if(m_audioSource.clip!=null)
		{
			m_accuTime += Time.deltaTime;
			if(m_accuTime >= m_audioSource.clip.length)
			{
				m_isDead = true;
				m_accuTime = 0f;
				gameObject.SetActive(false);
				if (m_OnSoundCueDead != null)
					m_OnSoundCueDead(this);
			}
		}
    }
}
