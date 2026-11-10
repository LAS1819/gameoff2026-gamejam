using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip themeFatherClip;

    private MusicManager musicManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicManager = GetComponent<MusicManager>();
        if (musicManager == null)
        {
            musicManager = gameObject.AddComponent<MusicManager>();
        }

        if (themeFatherClip != null)
        {
            musicManager.Configure(themeFatherClip);
        }
    }

    private void OnEnable()
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
    }

    private void Start()
    {
        if (themeFatherClip != null)
        {
            musicManager?.Configure(themeFatherClip);
        }

        musicManager?.EnsureTheme();
        musicManager?.ApplyReality(RealityManager.Instance != null ? RealityManager.Instance.CurrentReality : RealityType.Normal);
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
        musicManager?.EnsureTheme();
    }

    public void StopTheme()
    {
        musicManager?.Stop();
    }

    private void HandleRoomChanged(RoomData _)
    {
        musicManager?.EnsureTheme();
    }

    private void HandleRealityChanged(RealityType reality)
    {
        musicManager?.ApplyReality(reality);
    }

    private void HandleFlagChanged(string _)
    {
        // We keep the audio system passive for now; only the current theme is required.
    }
}
