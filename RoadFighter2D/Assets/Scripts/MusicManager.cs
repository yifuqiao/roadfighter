using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
	public enum AudioType
	{
		Engine0,
        Engine1,
		Engine2,
		CountDown0,
		CountDown1,
		Explosion,
        Collision,
        LastLife,
        Win,
        ANY
	}
    
	[System.Serializable]
	public class CueDefinition
	{
		public AudioType m_type;
		public AudioClip m_clip;
	}

	[SerializeField] private GameObject m_soundCuePrefab;
	[SerializeField] private CueDefinition[] m_curDefs;

	private List<SoundCue> m_deadSoundCueList = new List<SoundCue>();
	private List<SoundCue> m_aliveSoundCueList = new List<SoundCue>();
    
	public static MusicManager Instance 
	{
		private set; get;	
	}

	private void Awake()
	{
		Instance = this;
	}
    
	public void MakeSFX(AudioType type, Transform targetTransform)
	{
		CueDefinition selectedDef = null;
		if (type == AudioType.ANY)
		{
			if(m_curDefs.Length > 0)
			{
				selectedDef = m_curDefs[Random.Range(0, m_curDefs.Length)];
			}
		}
		else
		{
			for (int i = 0; i < m_curDefs.Length; ++i)
			{
				if (m_curDefs[i].m_type == type)
				{
					selectedDef = m_curDefs[i];
					break;
				}
			}
		}

		if (selectedDef != null)
		{
			if (m_deadSoundCueList.Count > 0)
			{
				m_deadSoundCueList[0].InitSoundCue(targetTransform, selectedDef.m_clip);
				m_aliveSoundCueList.Add(m_deadSoundCueList[0]);
				m_deadSoundCueList.RemoveAt(0);
			}
			else
			{
				var go = GameObject.Instantiate(m_soundCuePrefab);
				var soundCueComponent = go.GetComponent<SoundCue>();
				soundCueComponent.InitSoundCue(targetTransform, selectedDef.m_clip);
				m_aliveSoundCueList.Add(soundCueComponent);
				soundCueComponent.m_OnSoundCueDead += OnSoundCueDead;
			}
		}
	}

	private void OnSoundCueDead(SoundCue soundCue)
	{
		m_aliveSoundCueList.Remove(soundCue);
		m_deadSoundCueList.Add(soundCue);
	}
}
