using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteStateController : MonoBehaviour
{
    [SerializeField] private Sprite[] states;
    [SerializeField] private int defaultStateIndex;
    [SerializeField] private bool loopOnEnable;
    [SerializeField] private float loopInterval = 0.2f;

    private SpriteRenderer spriteRenderer;
    private int currentStateIndex;
    private bool isLooping;
    private Coroutine loopRoutine;

    public int CurrentStateIndex => currentStateIndex;
    public bool IsLooping => isLooping;
    public int StateCount => states == null ? 0 : states.Length;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyDefaultState();
    }

    private void OnEnable()
    {
        if (loopOnEnable && states != null && states.Length > 1)
        {
            PlayLoop();
        }
    }

    private void OnDisable()
    {
        StopLoop();
    }

    public bool SetState(int stateIndex)
    {
        if (states == null || states.Length == 0)
        {
            return false;
        }

        if (stateIndex < 0)
        {
            stateIndex = 0;
        }

        if (stateIndex >= states.Length)
        {
            stateIndex = states.Length - 1;
        }

        currentStateIndex = stateIndex;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = states[currentStateIndex];
        }

        StopLoop();
        return true;
    }

    public void SetVisible(bool visible)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.enabled = visible;
    }

    public void PlayLoop()
    {
        if (states == null || states.Length < 2)
        {
            return;
        }

        StopLoop();
        loopRoutine = StartCoroutine(LoopStates());
    }

    public void StopLoop()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }

        isLooping = false;
    }

    private void ApplyDefaultState()
    {
        if (states == null || states.Length == 0)
        {
            return;
        }

        var clampedIndex = Mathf.Clamp(defaultStateIndex, 0, states.Length - 1);
        currentStateIndex = clampedIndex;

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = states[currentStateIndex];
        }
    }

    private IEnumerator LoopStates()
    {
        isLooping = true;

        while (isLooping && states != null && states.Length > 1)
        {
            currentStateIndex = (currentStateIndex + 1) % states.Length;

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = states[currentStateIndex];
            }

            yield return new WaitForSeconds(loopInterval);
        }
    }
}
