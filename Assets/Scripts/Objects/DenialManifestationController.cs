using System.Collections;
using UnityEngine;

/// <summary>
/// Plays the short unreal disappearance effect for the Basement manifestation.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class DenialManifestationController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.35f;
    [SerializeField] private float shrinkScale = 0.72f;
    [SerializeField] private float distortionScale = 1.18f;

    private SpriteRenderer spriteRenderer;
    private SpriteStateController spriteStateController;
    private Coroutine rejectionRoutine;
    private Vector3 originalScale;
    private Color originalColor;
    private bool hasPlayed;

    public bool HasPlayed => hasPlayed;
    public bool IsPlaying => rejectionRoutine != null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteStateController = GetComponent<SpriteStateController>();
        originalScale = transform.localScale;
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            originalColor.a = Mathf.Clamp01(originalColor.a);
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }
    }

    private void OnDisable()
    {
        if (rejectionRoutine != null)
        {
            StopCoroutine(rejectionRoutine);
            rejectionRoutine = null;
        }
    }

    /// <summary>
    /// Plays the manifestation's unreal shrink, distortion and fade, then hides it.
    /// </summary>
    public void PlayRejection()
    {
        if (hasPlayed)
        {
            return;
        }

        hasPlayed = true;
        if (rejectionRoutine != null)
        {
            StopCoroutine(rejectionRoutine);
        }

        rejectionRoutine = StartCoroutine(PlayRejectionRoutine());
    }

    public void ResetForReplay()
    {
        hasPlayed = false;
        if (rejectionRoutine != null)
        {
            StopCoroutine(rejectionRoutine);
            rejectionRoutine = null;
        }

        transform.localScale = originalScale;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }
        if (spriteStateController != null)
        {
            spriteStateController.SetState(0);
        }
    }

    private IEnumerator PlayRejectionRoutine()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteStateController == null)
        {
            spriteStateController = GetComponent<SpriteStateController>();
        }

        if (spriteStateController != null)
        {
            spriteStateController.StopLoop();
        }

        transform.localScale = originalScale;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }

        var elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            var normalizedTime = Mathf.Clamp01(elapsed / fadeDuration);
            var pulse = Mathf.Sin(normalizedTime * Mathf.PI * 5f) * (1f - normalizedTime);

            var targetScale = Mathf.Lerp(1f, shrinkScale, normalizedTime);
            targetScale += pulse * (distortionScale - 1f) * 0.18f;
            transform.localScale = originalScale * targetScale;

            if (spriteRenderer != null)
            {
                var color = originalColor;
                color.a = Mathf.Lerp(originalColor.a, 0f, normalizedTime);
                spriteRenderer.color = color;
            }

            yield return null;
        }

        transform.localScale = originalScale * shrinkScale;
        if (spriteRenderer != null)
        {
            var color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }

        rejectionRoutine = null;
        gameObject.SetActive(false);
    }
}
