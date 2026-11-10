using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip themeFatherClip;
    [SerializeField] private AudioMixerGroup musicOutputGroup;
    [SerializeField] private bool loop = true;
    [SerializeField, Range(0f, 1f)] private float normalVolume = 0.80f;
    [SerializeField, Range(0.8f, 1.2f)] private float normalPitch = 1f;
    [SerializeField, Range(0.8f, 1.2f)] private float otherSidePitch = 1.08f;
    [SerializeField, Range(0f, 1f)] private float otherSideVolume = 0.90f;

    private AudioSource musicSource;

    private void Awake()
    {
        EnsureSource();
    }

    private void Start()
    {
        EnsureTheme();
    }

    public void Configure(AudioClip clip)
    {
        themeFatherClip = clip;
        EnsureSource();
        ApplyClip();
    }

    public void EnsureTheme()
    {
        EnsureSource();

        if (themeFatherClip == null)
        {
            Debug.LogWarning("[MusicManager] No theme clip assigned in the Inspector.");
            return;
        }

        if (musicSource.clip != themeFatherClip)
        {
            musicSource.clip = themeFatherClip;
        }

        musicSource.loop = loop;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void ApplyReality(RealityType reality)
    {
        EnsureSource();

        if (musicSource == null || themeFatherClip == null)
        {
            return;
        }

        var isOtherSide = reality == RealityType.OtherSide;
        musicSource.pitch = isOtherSide ? otherSidePitch : normalPitch;
        musicSource.volume = isOtherSide ? otherSideVolume : normalVolume;
    }

    public void Stop()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    private void EnsureSource()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        if (musicOutputGroup != null)
        {
            musicSource.outputAudioMixerGroup = musicOutputGroup;
        }
    }

    private void ApplyClip()
    {
        if (themeFatherClip == null)
        {
            return;
        }

        if (musicSource.clip != themeFatherClip)
        {
            musicSource.clip = themeFatherClip;
        }

        musicSource.loop = loop;
        musicSource.outputAudioMixerGroup = musicOutputGroup;
        musicSource.volume = normalVolume;
    }

}
