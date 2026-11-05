using System;
using UnityEngine;

public class RealityManager : MonoBehaviour
{
    public static RealityManager Instance { get; private set; }

    public RealityType CurrentReality { get; private set; } = RealityType.Normal;

    public event Action<RealityType> RealityChanged;

    private GameObject backgroundNormal;
    private GameObject backgroundOtherSide;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetReality(RealityType reality)
    {
        if (CurrentReality == reality)
        {
            return;
        }

        CurrentReality = reality;

        // Update background visibility - ALWAYS search fresh since rooms activate/deactivate
        UpdateBackgrounds();

        RoomManager.Instance?.RefreshInteractables();
        RealityChanged?.Invoke(CurrentReality);

        Debug.Log($"[RealityManager] Reality changed to: {CurrentReality}");
    }

    public void ToggleReality()
    {
        SetReality(CurrentReality == RealityType.Normal ? RealityType.OtherSide : RealityType.Normal);
    }

    /// <summary>
    /// Public static wrapper for UI buttons - can be called directly from Inspector
    /// </summary>
    public static void ToggleRealityStatic()
    {
        if (Instance != null)
        {
            Instance.ToggleReality();
            Debug.Log("[RealityManager] Reality toggled from UI");
        }
        else
        {
            Debug.LogError("[RealityManager] Instance not found for UI toggle");
        }
    }

    private void UpdateBackgrounds()
    {
        // Always search fresh - rooms activate/deactivate dynamically
        FindBackgrounds();

        if (backgroundNormal != null && backgroundOtherSide != null)
        {
            var normalActive = CurrentReality == RealityType.Normal;
            var otherActive = CurrentReality == RealityType.OtherSide;

            backgroundNormal.SetActive(normalActive);
            backgroundOtherSide.SetActive(otherActive);

            Debug.Log($"[RealityManager] Updated backgrounds - Normal: {normalActive}, OtherSide: {otherActive}");
        }
        else if (backgroundNormal != null)
        {
            Debug.LogWarning("[RealityManager] BackgroundNormal found, but BackgroundOtherSide NOT found - current room may not have OtherSide variant");
        }
        else
        {
            Debug.LogWarning("[RealityManager] No backgrounds found in current room");
        }
    }

    private void FindBackgrounds()
    {
        // Find backgrounds - includeInactive: true because BackgroundOtherSide is inactive by default
        var allGameObjects = FindObjectsOfType<GameObject>(includeInactive: true);
        
        backgroundNormal = null;
        backgroundOtherSide = null;

        foreach (var go in allGameObjects)
        {
            if (go.name == "BackgroundNormal" && backgroundNormal == null)
            {
                backgroundNormal = go;
                Debug.Log($"[RealityManager] Found BackgroundNormal at {GetFullPath(go.transform)}");
            }
            else if (go.name == "BackgroundOtherSide" && backgroundOtherSide == null)
            {
                backgroundOtherSide = go;
                Debug.Log($"[RealityManager] Found BackgroundOtherSide at {GetFullPath(go.transform)}");
            }
        }
    }

    private string GetFullPath(Transform t)
    {
        var path = t.name;
        var current = t.parent;
        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }
        return path;
    }
}
