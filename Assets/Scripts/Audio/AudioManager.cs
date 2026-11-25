using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip themeFatherClip;

    private MusicManager musicManager;
    private SFXManager sfxManager;
    private bool roomEventsStarted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        musicManager = GetComponent<MusicManager>();
        if (musicManager == null)
        {
            musicManager = gameObject.AddComponent<MusicManager>();
        }

        sfxManager = GetComponent<SFXManager>();
        if (sfxManager == null)
        {
            sfxManager = gameObject.AddComponent<SFXManager>();
        }

        if (themeFatherClip != null)
        {
            musicManager.Configure(themeFatherClip);
        }
    }

    private void OnEnable()
    {
        TrySubscribeRuntimeEvents();
    }

    private void TrySubscribeRuntimeEvents()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged += HandleRoomChanged;
        }

        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged += HandleRealityChanged;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FlagChanged += HandleFlagChanged;
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ItemAdded += HandleItemAdded;
        }
    }

    private void OnDisable()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged -= HandleRoomChanged;
        }

        if (RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged -= HandleRealityChanged;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FlagChanged -= HandleFlagChanged;
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ItemAdded -= HandleItemAdded;
        }
    }

    private void Start()
    {
        if (themeFatherClip != null)
        {
            if (musicManager != null)
            {
                musicManager.Configure(themeFatherClip);
            }
        }

        if (musicManager != null)
        {
            musicManager.EnsureTheme();
            musicManager.ApplyReality(
                RealityManager.Instance != null
                    ? RealityManager.Instance.CurrentReality
                    : RealityType.Normal);
        }
        UpdateSteamLoop();
    }

    public void SetThemeClip(AudioClip clip)
    {
        themeFatherClip = clip;
        if (musicManager != null)
        {
            musicManager.Configure(clip);
        }
    }

    public void PlayTheme()
    {
        if (musicManager != null)
        {
            musicManager.EnsureTheme();
        }
    }

    public void StopTheme()
    {
        if (musicManager != null)
        {
            musicManager.Stop();
        }
    }

    public void PlaySFX(SFXCue cue)
    {
        if (sfxManager != null)
        {
            sfxManager.Play(cue);
        }
    }

    public void SetSFXRootVolume(float volume)
    {
        if (sfxManager != null)
        {
            sfxManager.SetRootVolume(volume);
        }
    }

    public void StartSteamLoop()
    {
        if (sfxManager != null)
        {
            sfxManager.StartSteamLoop();
        }
    }

    public void StopSteamLoop()
    {
        if (sfxManager != null)
        {
            sfxManager.StopSteamLoop();
        }
    }

    private void HandleRoomChanged(RoomData _)
    {
        if (musicManager != null)
        {
            musicManager.EnsureTheme();
        }

        if (roomEventsStarted)
        {
            PlaySFX(SFXCue.RoomTransition);
        }
        roomEventsStarted = true;
        UpdateSteamLoop();
    }

    private void HandleRealityChanged(RealityType reality)
    {
        if (musicManager != null)
        {
            musicManager.ApplyReality(reality);
        }

        PlaySFX(SFXCue.Shift);
        UpdateSteamLoop();
    }

    private void HandleFlagChanged(string flagName)
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return;
        }

        if (flagName.StartsWith("Memory_", System.StringComparison.OrdinalIgnoreCase)
            && gameManager.HasFlag(flagName))
        {
            PlaySFX(SFXCue.MemoryDiscovery);
        }

        if (gameManager.HasFlag(flagName))
        {
            if (flagName == GameFlagNames.DoorOpened
                || flagName == GameFlagNames.LibraryDoorUnlocked
                || flagName == GameFlagNames.BasementUnlocked
                || flagName == GameFlagNames.LibrarySecretDoorOpen
                || flagName == GameFlagNames.BasementDoorVisible)
            {
                PlaySFX(SFXCue.DoorAction);
            }

            if (flagName == GameFlagNames.PressureValveRepaired)
            {
                PlaySFX(SFXCue.PressureValveAction);
            }

            if (flagName == GameFlagNames.BedroomComputerPowered
                || flagName == GameFlagNames.BedroomComputerDiskInserted)
            {
                PlaySFX(SFXCue.ItemUse);
            }

            if (flagName == GameFlagNames.LibraryShelfActivated)
            {
                PlaySFX(SFXCue.ShelfSecretAction);
            }

            if (flagName == GameFlagNames.BedroomLightOn)
            {
                PlaySFX(SFXCue.LightSwitch);
            }

            if (flagName == GameFlagNames.BasementCodeUsed)
            {
                PlaySFX(SFXCue.ItemUse);
            }
        }

        if (flagName == GameFlagNames.SteamEnabled)
        {
            UpdateSteamLoop();
        }
    }

    private void HandleItemAdded(InventoryItemData _)
    {
        PlaySFX(SFXCue.ItemPickup);
    }

    private void UpdateSteamLoop()
    {
        var gameManager = GameManager.Instance;
        var realityManager = RealityManager.Instance;
        var roomManager = RoomManager.Instance;
        var shouldPlay = gameManager != null
            && realityManager != null
            && roomManager != null
            && roomManager.CurrentRoom != null
            && roomManager.CurrentRoom.id == GameManager.BasementRoomId
            && realityManager.CurrentReality == RealityType.OtherSide
            && gameManager.HasFlag(GameFlagNames.SteamEnabled);

        if (shouldPlay)
        {
            StartSteamLoop();
        }
        else
        {
            StopSteamLoop();
        }
    }
}
