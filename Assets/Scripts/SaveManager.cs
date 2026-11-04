using UnityEngine;

/// <summary>
/// SaveManager handles save/load game state.
/// This is a stub for MVP. Later expansion: serialization, file I/O, autosave.
/// For MVP, game state is held in GameManager flags and room state in RoomManager.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private string saveManagerName = "SaveManager";

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

    private void Start()
    {
        Debug.Log($"[{saveManagerName}] Initialized (stub for MVP)");
    }

    /// <summary>
    /// Save current game state to file. Not implemented for MVP.
    /// </summary>
    public void SaveGame(string filename)
    {
        Debug.Log($"[{saveManagerName}] SaveGame('{filename}') - stub method, not implemented for MVP");
    }

    /// <summary>
    /// Load game state from file. Not implemented for MVP.
    /// </summary>
    public void LoadGame(string filename)
    {
        Debug.Log($"[{saveManagerName}] LoadGame('{filename}') - stub method, not implemented for MVP");
    }
}
