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
    private bool gameManagerEventsSubscribed;

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

        if (gameManagerEventsSubscribed && GameManager.Instance != null)
        {
            GameManager.Instance.FlagChanged -= HandleFlagChanged;
        }

        roomEventsSubscribed = false;
        realityEventsSubscribed = false;
        gameManagerEventsSubscribed = false;
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

        if (!gameManagerEventsSubscribed && GameManager.Instance != null)
        {
            GameManager.Instance.FlagChanged += HandleFlagChanged;
            gameManagerEventsSubscribed = true;
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

    private void HandleFlagChanged(string flagName)
    {
        if (flagName == GameFlagNames.MainMenuIntroStarted
            || flagName == GameFlagNames.BedroomLightOn
            || flagName == GameFlagNames.BedroomComputerPowered
            || flagName == GameFlagNames.FlashlightActive)
        {
            ApplyCurrentProfile();
            RefreshLanternUI();
        }
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

        var intensity = reality == RealityType.OtherSide
            ? profile.otherSideIntensity
            : profile.normalIntensity;
        var color = reality == RealityType.OtherSide
            ? profile.otherSideColor
            : profile.normalColor;

        var roomId = room.id;
        var gameManager = GameManager.Instance;

        if (roomId.Equals(GameManager.MainMenuRoomId, StringComparison.OrdinalIgnoreCase)
            && gameManager != null
            && gameManager.HasFlag(GameFlagNames.MainMenuIntroStarted))
        {
            intensity += 0.30f;
            color = Color.Lerp(
                color,
                new Color(1f, 0.78f, 0.35f),
                0.55f);
        }

        if (roomId.Equals(GameManager.BedroomRoomId, StringComparison.OrdinalIgnoreCase)
            && gameManager != null)
        {
            if (gameManager.HasFlag(GameFlagNames.BedroomLightOn))
            {
                intensity += 0.18f;
                color = Color.Lerp(
                    color,
                    new Color(1f, 0.82f, 0.58f),
                    0.45f);
            }

            if (gameManager.HasFlag(GameFlagNames.BedroomComputerPowered))
            {
                intensity += 0.10f;
                color = Color.Lerp(
                    color,
                    new Color(35f / 255f, 144f / 255f, 99f / 255f),
                    0.30f);
            }
        }

        if (lanternActive)
        {
            intensity += reality == RealityType.OtherSide
                ? profile.lanternOtherSideBonus
                : profile.lanternNormalBonus;
            color = Color.Lerp(
                color,
                reality == RealityType.OtherSide
                    ? new Color(0.72f, 0.78f, 0.92f)
                    : new Color(1f, 0.82f, 0.58f),
                0.35f);
        }

        globalLight.intensity = Mathf.Clamp(intensity, 0f, 1.5f);
        globalLight.color = color;

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
