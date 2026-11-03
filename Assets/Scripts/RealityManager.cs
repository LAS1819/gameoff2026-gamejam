    using System;
    using UnityEngine;

    public class RealityManager : MonoBehaviour
    {
        public static RealityManager Instance { get; private set; }

        public RealityType CurrentReality { get; private set; } = RealityType.Normal;

        public event Action<RealityType> RealityChanged;

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
            GameManager.Instance?.SetFlag(GameFlagNames.RealityUnlocked, true);

            if (RoomManager.Instance != null)
            {
                RoomManager.Instance.RefreshInteractables();
            }

            RealityChanged?.Invoke(CurrentReality);
        }

        public void ToggleReality()
        {
            SetReality(CurrentReality == RealityType.Normal ? RealityType.OtherSide : RealityType.Normal);
        }
    }
