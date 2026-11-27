using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Applies room lighting profiles and owns the Basement-only flashlight state.
/// </summary>
public class LanternController : MonoBehaviour
{
    public static LanternController Instance { get; private set; }

    [SerializeField] private Light2D globalLight;

    private bool roomEventsSubscribed;
    private bool realityEventsSubscribed;

    public bool IsLanternActive
    {
        get
        {
            return GameManager.Instance != null
                && GameManager.Instance.HasFlag(GameFlagNames.FlashlightActive);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResolveGlobalLight();
    }

    private void OnEnable()
    {
        TrySubscribeEvents();
    }

    private void Start()
    {
        TrySubscribeEvents();
        ApplyCurrentProfile();
    }

    private void OnDisable()
    {
        CancelInvoke();
        if (roomEventsSubscribed && RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged -= HandleRoomChanged;
        }

        if (realityEventsSubscribed && RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged -= HandleRealityChanged;
        }

        roomEventsSubscribed = false;
        realityEventsSubscribed = false;
    }

    public bool TryToggleLantern()
    {
        if (UIManager.Instance != null && UIManager.Instance.IsStoryOverlayVisible())
        {
            return false;
        }

        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return false;
        }

        if (!gameManager.HasFlag(GameFlagNames.HasFlashlight))
        {
            ShowFeedback("I need to find the flashlight first.", 1.8f);
            PlayDeniedCue();
            return false;
        }

        var currentRoom = RoomManager.Instance != null
            ? RoomManager.Instance.CurrentRoom
            : null;
        if (currentRoom == null
            || !currentRoom.id.Equals(GameManager.BasementRoomId, StringComparison.OrdinalIgnoreCase))
        {
            ShowFeedback("The flashlight is only useful in the Basement.", 1.8f);
            PlayDeniedCue();
            return false;
        }

        var nextState = !gameManager.HasFlag(GameFlagNames.FlashlightActive);
        SetLanternActive(nextState);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.LightSwitch);
        }

        Debug.Log(nextState
            ? "[LanternController] Flashlight enabled in Basement."
            : "[LanternController] Flashlight disabled.");
        return true;
    }

    public void SetLanternActive(bool active)
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return;
        }

        gameManager.SetFlag(GameFlagNames.FlashlightActive, active);
        ApplyCurrentProfile();
        RefreshLanternUI();
    }

    private void TrySubscribeEvents()
    {
        if (!roomEventsSubscribed && RoomManager.Instance != null)
        {
            RoomManager.Instance.RoomChanged += HandleRoomChanged;
            roomEventsSubscribed = true;
        }

        if (!realityEventsSubscribed && RealityManager.Instance != null)
        {
            RealityManager.Instance.RealityChanged += HandleRealityChanged;
            realityEventsSubscribed = true;
        }
    }

    private void HandleRoomChanged(RoomData room)
    {
        var remainsInBasement = room != null
            && room.id.Equals(GameManager.BasementRoomId, StringComparison.OrdinalIgnoreCase);
        if (!remainsInBasement && IsLanternActive)
        {
            SetLanternActive(false);
        }

        ApplyCurrentProfile();
        RefreshLanternUI();
    }

    private void HandleRealityChanged(RealityType _)
    {
        ApplyCurrentProfile();
    }

    public void ApplyCurrentProfile()
    {
        ResolveGlobalLight();
        if (globalLight == null)
        {
            Debug.LogWarning("[LanternController] Global Light 2D reference is missing.");
            return;
        }

        var room = RoomManager.Instance != null
            ? RoomManager.Instance.CurrentRoom
            : null;
        var profile = room != null ? room.lightingProfile : null;
        if (profile == null)
        {
            globalLight.intensity = 1f;
            globalLight.color = Color.white;
            Debug.Log("[LanternController] No lighting profile found; using neutral fallback.");
            return;
        }

        var reality = RealityManager.Instance != null
            ? RealityManager.Instance.CurrentReality
            : RealityType.Normal;
        var isBasement = room.id.Equals(
            GameManager.BasementRoomId,
            StringComparison.OrdinalIgnoreCase);
        var lanternActive = isBasement
            && profile.lanternAllowed
            && IsLanternActive;

        if (reality == RealityType.OtherSide)
        {
            globalLight.intensity = lanternActive
                ? profile.lanternOtherSideIntensity
                : profile.otherSideIntensity;
            globalLight.color = profile.otherSideColor;
        }
        else
        {
            globalLight.intensity = lanternActive
                ? profile.lanternNormalIntensity
                : profile.normalIntensity;
            globalLight.color = profile.normalColor;
        }

        Debug.Log(
            $"[LanternController] Room={room.id} Reality={reality} Active={lanternActive} Intensity={globalLight.intensity}");
    }

    private void ResolveGlobalLight()
    {
        if (globalLight != null)
        {
            return;
        }

        var lights = FindObjectsByType<Light2D>(FindObjectsInactive.Include);
        for (var index = 0; index < lights.Length; index++)
        {
            var light = lights[index];
            if (light != null && light.lightType == Light2D.LightType.Global)
            {
                globalLight = light;
                return;
            }
        }
    }

    private static void RefreshLanternUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshLanternState();
        }
    }

    private static void ShowFeedback(string message, float duration)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowFeedback(message, duration);
        }
    }

    private static void PlayDeniedCue()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(SFXCue.UiDenied);
        }
    }
}
