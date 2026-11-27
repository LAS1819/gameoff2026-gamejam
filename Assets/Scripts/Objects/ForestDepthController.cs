using UnityEngine;

public class ForestDepthController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sky;
    [SerializeField] private SpriteRenderer centralPath;
    [SerializeField] private SpriteRenderer borderTrees;
    [SerializeField] private SpriteRenderer outerForest;
    [SerializeField] private GameObject backgroundNormal;
    [SerializeField] private GameObject forestAdvance;
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject ceilingLight;
    [SerializeField] private GameObject revelationLetter;
    [SerializeField] private Sprite[] skySprites;
    [SerializeField] private Sprite[] centralPathSprites;
    [SerializeField] private Sprite[] borderTreesSprites;
    [SerializeField] private Sprite[] outerForestSprites;
    [SerializeField] private GameObject ambulanceLights;
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject blueLight;

    private float nextAmbulanceFlashTime;
    private bool redLightActive;
    private Vector3 ambulanceBasePosition;
    private bool ambulanceBasePositionResolved;

    public void Configure(
        SpriteRenderer skyRenderer,
        SpriteRenderer centralPathRenderer,
        SpriteRenderer borderTreesRenderer,
        SpriteRenderer outerForestRenderer,
        Sprite[] skySpriteFrames,
        Sprite[] centralPathSpriteFrames,
        Sprite[] borderTreesSpriteFrames,
        Sprite[] outerForestSpriteFrames)
    {
        sky = skyRenderer;
        centralPath = centralPathRenderer;
        borderTrees = borderTreesRenderer;
        outerForest = outerForestRenderer;
        skySprites = skySpriteFrames;
        centralPathSprites = centralPathSpriteFrames;
        borderTreesSprites = borderTreesSpriteFrames;
        outerForestSprites = outerForestSpriteFrames;
        ApplyDepth(GameManager.Instance != null ? GameManager.Instance.ForestDepthLevel : 0);
    }

    public void RefreshDepth()
    {
        ApplyDepth(GameManager.Instance != null ? GameManager.Instance.ForestDepthLevel : 0);
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ForestDepthChanged += HandleDepthChanged;
            ApplyDepth(GameManager.Instance.ForestDepthLevel);
        }
    }

    private void Start()
    {
        RefreshDepth();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ForestDepthChanged -= HandleDepthChanged;
        }
    }

    private void Update()
    {
        if (ambulanceLights == null || !ambulanceLights.activeInHierarchy)
        {
            return;
        }

        if (Time.time < nextAmbulanceFlashTime)
        {
            return;
        }

        redLightActive = !redLightActive;
        if (redLight != null)
        {
            redLight.SetActive(redLightActive);
        }
        if (blueLight != null)
        {
            blueLight.SetActive(!redLightActive);
        }

        nextAmbulanceFlashTime = Time.time + 0.28f;
    }

    private void HandleDepthChanged(int depthLevel)
    {
        ApplyDepth(depthLevel);
    }

    private void ApplyDepth(int depthLevel)
    {
        ResolveFinalObjects();
        ResolveDepthRenderers();
        ResolveAmbulanceLights();
        ApplyAmbulanceLights(depthLevel);
        Debug.Log($"[ForestDepth] Applying depth {depthLevel}.");

        var isFinalRoom = depthLevel >= 6;
        if (backgroundNormal != null)
        {
            backgroundNormal.SetActive(!isFinalRoom);
        }

        if (forestAdvance != null)
        {
            forestAdvance.SetActive(!isFinalRoom);
        }

        if (table != null)
        {
            table.SetActive(isFinalRoom);
        }

        if (ceilingLight != null)
        {
            ceilingLight.SetActive(isFinalRoom);
        }

        if (revelationLetter != null)
        {
            revelationLetter.SetActive(isFinalRoom);
        }

        if (isFinalRoom)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetFlag(GameFlagNames.ForestFinalTableVisible, true);
            }

            return;
        }

        if (sky != null)
        {
            sky.sprite = GetDepthSprite(skySprites, depthLevel);
            sky.color = Color.white;
        }

        if (centralPath != null)
        {
            centralPath.sprite = GetDepthSprite(centralPathSprites, depthLevel);
        }

        if (borderTrees != null)
        {
            borderTrees.sprite = GetDepthSprite(borderTreesSprites, depthLevel);
        }

        if (outerForest != null)
        {
            outerForest.sprite = GetDepthSprite(outerForestSprites, depthLevel);
        }
    }

    private void ResolveAmbulanceLights()
    {
        if (ambulanceLights == null)
        {
            ambulanceLights = FindGameObject(transform, "Props/AmbulanceLights");
            if (ambulanceLights == null)
            {
                ambulanceLights = FindNamedGameObject(transform, "AmbulanceLights");
            }
        }

        if (ambulanceLights != null && !ambulanceBasePositionResolved)
        {
            ambulanceBasePosition = ambulanceLights.transform.localPosition;
            ambulanceBasePositionResolved = true;
        }

        if (redLight == null && ambulanceLights != null)
        {
            redLight = FindNamedChild(ambulanceLights.transform, "RedLight");
        }

        if (blueLight == null && ambulanceLights != null)
        {
            blueLight = FindNamedChild(ambulanceLights.transform, "BlueLight");
        }
    }

    private void ApplyAmbulanceLights(int depthLevel)
    {
        if (ambulanceLights == null)
        {
            return;
        }

        var visible = depthLevel >= 1 && depthLevel <= 5;
        ambulanceLights.SetActive(visible);
        if (!visible)
        {
            if (redLight != null)
            {
                redLight.SetActive(false);
            }
            if (blueLight != null)
            {
                blueLight.SetActive(false);
            }
            redLightActive = false;
            return;
        }

        var scale = 0.20f + ((depthLevel - 1) * 0.20f);
        ambulanceLights.transform.localScale = Vector3.one * Mathf.Clamp(scale, 0.20f, 1f);
        var positionOffset = 3f - ((depthLevel - 1) * 0.75f);
        ambulanceLights.transform.localPosition = new Vector3(
            ambulanceBasePosition.x,
            ambulanceBasePosition.y + Mathf.Clamp(positionOffset, 0f, 3f),
            ambulanceBasePosition.z);
        redLightActive = false;
        nextAmbulanceFlashTime = 0f;

        if (redLight != null)
        {
            redLight.SetActive(true);
        }
        if (blueLight != null)
        {
            blueLight.SetActive(false);
        }
    }

    private void ResolveFinalObjects()
    {
        if (backgroundNormal != null
            && forestAdvance != null
            && table != null
            && ceilingLight != null
            && revelationLetter != null)
        {
            return;
        }

        var forestRoot = transform;
        if (backgroundNormal == null)
        {
            backgroundNormal = FindGameObject(forestRoot, "Visuals/BackgroundNormal");
        }

        if (forestAdvance == null)
        {
            forestAdvance = FindGameObject(forestRoot, "Interactables/ForestAdvance");
        }

        if (table == null)
        {
            table = FindGameObject(forestRoot, "Props/Table");
        }

        if (ceilingLight == null)
        {
            ceilingLight = FindGameObject(forestRoot, "Props/CeilingLight");
            if (ceilingLight == null)
            {
                ceilingLight = FindGameObject(forestRoot, "Props/CeilingLamp");
            }
        }

        if (revelationLetter == null)
        {
            revelationLetter = FindGameObject(forestRoot, "Interactables/ForestRevelationLetter");
        }
    }

    private static GameObject FindNamedGameObject(Transform root, string objectName)
    {
        if (root == null || string.IsNullOrEmpty(objectName))
        {
            return null;
        }

        foreach (var child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child != null
                && child.name.Equals(objectName, System.StringComparison.OrdinalIgnoreCase))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private static GameObject FindNamedChild(Transform root, string objectName)
    {
        if (root == null || string.IsNullOrEmpty(objectName))
        {
            return null;
        }

        foreach (var child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child != null
                && child.name.Equals(objectName, System.StringComparison.OrdinalIgnoreCase))
            {
                return child.gameObject;
            }
        }

        return null;
    }

    private void ResolveDepthRenderers()
    {
        if (sky == null)
        {
            sky = FindSpriteRenderer(transform, "Sky");
        }
        if (centralPath == null)
        {
            centralPath = FindSpriteRenderer(transform, "CentralPath");
        }
        if (borderTrees == null)
        {
            borderTrees = FindSpriteRenderer(transform, "BorderTrees");
        }
        if (outerForest == null)
        {
            outerForest = FindSpriteRenderer(transform, "OuterForest");
        }
    }

    private static SpriteRenderer FindSpriteRenderer(Transform root, string objectName)
    {
        if (root == null || string.IsNullOrEmpty(objectName))
        {
            return null;
        }

        foreach (var renderer in root.GetComponentsInChildren<SpriteRenderer>(true))
        {
            if (renderer != null
                && renderer.gameObject.name.Equals(objectName, System.StringComparison.OrdinalIgnoreCase))
            {
                return renderer;
            }
        }

        return null;
    }

    private static GameObject FindGameObject(Transform root, string path)
    {
        var child = root.Find(path);
        if (child == null)
        {
            return null;
        }

        return child.gameObject;
    }

    private static Sprite GetDepthSprite(Sprite[] sprites, int depthLevel)
    {
        if (sprites == null || sprites.Length == 0)
        {
            return null;
        }

        if (depthLevel <= 0)
        {
            return sprites[0];
        }

        var index = Mathf.Clamp(depthLevel - 1, 0, sprites.Length - 1);
        return sprites[index];
    }

    private static Sprite GetSprite(Sprite[] sprites, int depthLevel)
    {
        if (sprites == null || sprites.Length == 0)
        {
            return null;
        }

        var index = Mathf.Clamp(depthLevel, 0, sprites.Length - 1);
        return sprites[index];
    }
}
