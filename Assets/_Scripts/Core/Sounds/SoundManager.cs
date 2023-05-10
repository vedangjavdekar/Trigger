using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_MusicSource;

    [SerializeField]
    private AudioSource m_SoundSource;

    [SerializeField,Range(0.0f,1.0f)]
    private float m_Volume;

    private void Awake()
    {
        SetMasterVolume(m_Volume);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if(!clip)
        {
            return;
        }

        m_SoundSource.PlayOneShot(clip, volume);
    }
    public void PlayMusic(AudioClip clip, bool looping = true)
    {
        m_MusicSource.loop = looping;

        if (m_MusicSource.clip != clip)
        {
            m_MusicSource.clip = clip;
            m_MusicSource.Play();
        }
    }

    public void ToggleSounds()
    {
        m_SoundSource.mute = !m_SoundSource.mute;
    }
    public void ToggleMusic()
    {
        m_MusicSource.mute = !m_MusicSource.mute;
    }
}
