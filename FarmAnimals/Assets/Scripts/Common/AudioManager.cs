using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [System.Serializable]
    public struct ClipInfo
    {
        public AudioClip audioClip;
        public float pitch;
        public float volume;
        public bool music;
        public bool playOnAwake;
        public bool global;
        public AudioMixerGroup output;
        public string clipID;
    }

    [SerializeField] [Range(1.0f, 20.0f)] int m_numOfChannels = 5;
    [SerializeField] ClipInfo[] m_clips = null;

    AudioSource[] m_SFXChannels;
    AudioSource[] m_musicChannels;

    void Awake()
    {
        m_SFXChannels = new AudioSource[m_numOfChannels];
        m_musicChannels = new AudioSource[m_numOfChannels];

        for (int i = 0; i < m_numOfChannels; ++i)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.spatialBlend = 1.0f;
            audioSource.dopplerLevel = 0.0f;
            m_SFXChannels[i] = audioSource;

            AudioSource music = gameObject.AddComponent<AudioSource>();
            music.loop = true;
            music.spatialBlend = 1.0f;
            music.dopplerLevel = 0.0f;
            m_musicChannels[i] = music;
        }

        for (int i = 0; i < m_clips.Length; ++i)
        {
            if (m_clips[i].playOnAwake)
            {
                PlayClip(m_clips[i].clipID, Vector3.zero);
            }
        }
    }

    public void StopAllClips(bool fade, float time = 1.0f)
    {
        if (fade)
        {
            StartCoroutine(FadeAudioVolume(time));
        }
        else
        {
            StopEverything();
        }
    }

    IEnumerator FadeAudioVolume(float time)
    {
        for (float a = 1.0f; a >= 0.0f; a -= Time.deltaTime / time)
        {
            for (int i = 0; i < m_numOfChannels; ++i)
            {
                AudioSource sfx = m_SFXChannels[i];
                AudioSource music = m_musicChannels[i];

                if (sfx.volume > a)
                {
                    sfx.volume = a;
                }
                if (music.volume > a)
                {
                    music.volume = a;
                }
            }

            yield return null;
        }

        StopEverything();
    }

    private void StopEverything()
    {
        for (int i = 0; i < m_numOfChannels; ++i)
        {
            AudioSource sfx = m_SFXChannels[i];
            AudioSource music = m_musicChannels[i];

            sfx.Stop();
            music.Stop();
        }
    }

    public void StopClip(string clipID)
    {
        ClipInfo clip = GetClip(clipID);

        for (int i = 0; i < m_numOfChannels; ++i)
        {
            AudioSource sfx = m_SFXChannels[i];
            AudioSource music = m_musicChannels[i];

            if (sfx.clip == clip.audioClip)
            {
                sfx.Stop();
            }
            if (music.clip == clip.audioClip)
            {
                music.Stop();
            }
        }
    }

    public void PlayClip(string clipID, Vector3 position, float duration = -1.0f)
    {
        // If the clip is set for global, position doesn't really matter
        // If duration is less than 0.0f, it will play the clip out fully

        ClipInfo clip = GetClip(clipID);

        duration = (duration <= 0.0f) ? clip.audioClip.length : duration;

        if (clip.music)
        {
            PlayMusic(clip, position, duration);
        }
        else
        {
            PlaySFX(clip, position, duration);
        }
    }

    private void PlaySFX(ClipInfo clip, Vector3 position, float duration)
    {
        foreach (AudioSource audioSource in m_SFXChannels)
        {
            if (clip.global && !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = clip.audioClip;
                audioSource.pitch = clip.pitch;
                audioSource.volume = clip.volume;
                audioSource.spatialBlend = (clip.global) ? 0.0f : 1.0f;
                audioSource.maxDistance = (clip.global) ? 1000 : 20;
                audioSource.outputAudioMixerGroup = clip.output;
                audioSource.Play();
                break;
            }
            else if (!clip.global)
            {
                GameObject temp = new GameObject("Temporary Sound");
                temp.transform.parent = transform;
                temp.transform.position = position;
                AudioSource audio = temp.AddComponent<AudioSource>();
                audio.dopplerLevel = 0.0f; audio.clip = clip.audioClip;

                audio.pitch = clip.pitch;
                audio.volume = clip.volume;
                audio.spatialBlend = 1.0f;
                audio.maxDistance = 20.0f;
                audio.outputAudioMixerGroup = clip.output;
                audio.Play();

                Destroy(temp, duration);
                break;
            }
        }
    }

    private void PlayMusic(ClipInfo clip, Vector3 position, float duration)
    {
        foreach (AudioSource audioSource in m_musicChannels)
        {
            if (clip.global && !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = clip.audioClip;
                audioSource.pitch = clip.pitch;
                audioSource.volume = clip.volume;
                audioSource.spatialBlend = 0.0f;
                audioSource.maxDistance = 1000.0f;
                audioSource.outputAudioMixerGroup = clip.output;
                audioSource.Play();
                break;
            }
            else if (!clip.global)
            {
                GameObject temp = new GameObject("Temporary Sound");
                temp.transform.parent = transform;
                temp.transform.position = position;
                AudioSource audio = temp.AddComponent<AudioSource>();
                audio.dopplerLevel = 0.0f;

                audio.clip = clip.audioClip;
                audio.pitch = clip.pitch;
                audio.volume = clip.volume;
                audio.spatialBlend = 1.0f;
                audio.maxDistance = 20.0f;
                audio.outputAudioMixerGroup = clip.output;
                audio.Play();

                Destroy(temp, duration);
                break;
            }
        }
    }

    private ClipInfo GetClip(string clipID)
    {
        ClipInfo clip = new ClipInfo();

        foreach (ClipInfo ci in m_clips)
        {
            if (ci.clipID.Equals(clipID))
            {
                clip = ci;
                break;
            }
        }

        return clip;
    }
}
