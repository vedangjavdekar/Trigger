using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOnStart: MonoBehaviour
{
    [SerializeField]
    private AudioClip m_Music;

    void Start()
    {
        GameManager.Instance.soundManager.PlayMusic(m_Music,true);
    }
}
