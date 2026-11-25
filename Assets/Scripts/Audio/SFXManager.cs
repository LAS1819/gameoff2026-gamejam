using UnityEngine;
using UnityEngine.Audio;

public enum SFXCue
{
    UiClick,
    UiDenied,
    Shift,
    RoomTransition,
    ItemPickup,
    ItemUse,
    DoorAction,
    LightSwitch,
    MemoryDiscovery,
    StoryOpen,
    ShelfSecretAction,
    PressureValveAction,
    SteamLoop
}

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private string sfxGroupName = "SFX";
    [SerializeField] private AudioMixerGroup sfxOutputGroup;
    [SerializeField, Range(0f, 1f)] private float rootVolume = 1f;

    [SerializeField] private AudioClip uiClickClip;
    [SerializeField, Range(0f, 1f)] private float uiClickVolume = 1f;
    [SerializeField] private AudioClip uiDeniedClip;
    [SerializeField, Range(0f, 1f)] private float uiDeniedVolume = 1f;
    [SerializeField] private AudioClip shiftClip;
    [SerializeField, Range(0f, 1f)] private float shiftVolume = 1f;
    [SerializeField] private AudioClip roomTransitionClip;
    [SerializeField, Range(0f, 1f)] private float roomTransitionVolume = 1f;
    [SerializeField] private AudioClip itemPickupClip;
    [SerializeField, Range(0f, 1f)] private float itemPickupVolume = 1f;
    [SerializeField] private AudioClip itemUseClip;
    [SerializeField, Range(0f, 1f)] private float itemUseVolume = 1f;
    [SerializeField] private AudioClip doorActionClip;
    [SerializeField, Range(0f, 1f)] private float doorActionVolume = 1f;
    [SerializeField] private AudioClip lightSwitchClip;
    [SerializeField, Range(0f, 1f)] private float lightSwitchVolume = 1f;
    [SerializeField] private AudioClip memoryDiscoveryClip;
    [SerializeField, Range(0f, 1f)] private float memoryDiscoveryVolume = 1f;
    [SerializeField] private AudioClip storyOpenClip;
    [SerializeField, Range(0f, 1f)] private float storyOpenVolume = 1f;
    [SerializeField] private AudioClip shelfSecretActionClip;
    [SerializeField, Range(0f, 1f)] private float shelfSecretActionVolume = 1f;
    [SerializeField] private AudioClip pressureValveActionClip;
    [SerializeField, Range(0f, 1f)] private float pressureValveActionVolume = 1f;
    [SerializeField] private AudioClip steamLoopClip;
    [SerializeField, Range(0f, 1f)] private float steamLoopVolume = 0.55f;

    private AudioSource oneShotSource;
    private AudioSource steamSource;

    private void Awake()
    {
        EnsureSources();
    }

    public void SetRootVolume(float volume)
    {
        rootVolume = Mathf.Clamp01(volume);
        if (steamSource != null)
        {
            steamSource.volume = rootVolume * steamLoopVolume;
        }
    }

    public void Play(SFXCue cue)
    {
        EnsureSources();

        var clip = ResolveClip(cue, out var clipVolume);
        if (clip == null)
        {
            Debug.LogWarning($"[SFXManager] Missing clip for cue '{cue}'.");
            return;
        }

        oneShotSource.volume = rootVolume;
        oneShotSource.PlayOneShot(clip, Mathf.Clamp01(clipVolume));
    }

    public void StartSteamLoop()
    {
        EnsureSources();

        if (steamLoopClip == null)
        {
            Debug.LogWarning("[SFXManager] Missing steam loop clip.");
            return;
        }

        if (steamSource.clip != steamLoopClip)
        {
            steamSource.clip = steamLoopClip;
        }

        steamSource.loop = true;
        steamSource.volume = rootVolume * steamLoopVolume;
        if (!steamSource.isPlaying)
        {
            steamSource.Play();
        }
    }

    public void StopSteamLoop()
    {
        if (steamSource != null && steamSource.isPlaying)
        {
            steamSource.Stop();
        }
    }

    private AudioClip ResolveClip(SFXCue cue, out float clipVolume)
    {
        switch (cue)
        {
            case SFXCue.UiClick:
                clipVolume = uiClickVolume;
                return uiClickClip;
            case SFXCue.UiDenied:
                clipVolume = uiDeniedVolume;
                return uiDeniedClip;
            case SFXCue.Shift:
                clipVolume = shiftVolume;
                return shiftClip;
            case SFXCue.RoomTransition:
                clipVolume = roomTransitionVolume;
                return roomTransitionClip;
            case SFXCue.ItemPickup:
                clipVolume = itemPickupVolume;
                return itemPickupClip;
            case SFXCue.ItemUse:
                clipVolume = itemUseVolume;
                return itemUseClip;
            case SFXCue.DoorAction:
                clipVolume = doorActionVolume;
                return doorActionClip;
            case SFXCue.LightSwitch:
                clipVolume = lightSwitchVolume;
                return lightSwitchClip;
            case SFXCue.MemoryDiscovery:
                clipVolume = memoryDiscoveryVolume;
                return memoryDiscoveryClip;
            case SFXCue.StoryOpen:
                clipVolume = storyOpenVolume;
                return storyOpenClip;
            case SFXCue.ShelfSecretAction:
                clipVolume = shelfSecretActionVolume;
                return shelfSecretActionClip;
            case SFXCue.PressureValveAction:
                clipVolume = pressureValveActionVolume;
                return pressureValveActionClip;
            case SFXCue.SteamLoop:
                clipVolume = steamLoopVolume;
                return steamLoopClip;
            default:
                clipVolume = 1f;
                return null;
        }
    }

    private void EnsureSources()
    {
        if (oneShotSource == null)
        {
            oneShotSource = FindOrCreateSource("SFX OneShot");
        }

        if (steamSource == null)
        {
            steamSource = FindOrCreateSource("SFX Steam Loop");
        }

        ResolveOutputGroup();
        if (sfxOutputGroup != null)
        {
            oneShotSource.outputAudioMixerGroup = sfxOutputGroup;
            steamSource.outputAudioMixerGroup = sfxOutputGroup;
        }

        oneShotSource.spatialBlend = 0f;
        oneShotSource.playOnAwake = false;
        steamSource.spatialBlend = 0f;
        steamSource.playOnAwake = false;
    }

    private AudioSource FindOrCreateSource(string sourceName)
    {
        var sources = GetComponentsInChildren<AudioSource>(true);
        foreach (var source in sources)
        {
            if (source.gameObject.name == sourceName)
            {
                return source;
            }
        }

        var sourceObject = new GameObject(sourceName);
        sourceObject.transform.SetParent(transform);
        sourceObject.transform.localPosition = Vector3.zero;
        return sourceObject.AddComponent<AudioSource>();
    }

    private void ResolveOutputGroup()
    {
        if (sfxOutputGroup != null)
        {
            return;
        }

        if (mainAudioMixer == null)
        {
            mainAudioMixer = LoadMainMixer();
        }

        if (mainAudioMixer == null)
        {
            return;
        }

        var groups = mainAudioMixer.FindMatchingGroups(sfxGroupName);
        if (groups != null && groups.Length > 0)
        {
            sfxOutputGroup = groups[0];
        }
    }

    private static AudioMixer LoadMainMixer()
    {
        var mixer = Resources.Load<AudioMixer>("MainAudioMixer");
        if (mixer != null)
        {
            return mixer;
        }

#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/Audio/MainAudioMixer.mixer");
#else
        return null;
#endif
    }
}
