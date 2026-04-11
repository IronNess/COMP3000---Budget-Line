using UnityEngine;

/// <summary>
/// Plays a click sound using a screen ray from <see cref="Camera.main"/>.
/// Use this instead of <see cref="OnMouseDown"/> when:
/// - The <see cref="Collider"/> is on a child (OnMouseDown only runs on the object that has the collider).
/// - The collider is a trigger (OnMouseDown does not fire on trigger colliders in many setups).
/// </summary>
public class ClickSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] [Tooltip("World ray length from the camera.")]
    private float maxRayDistance = 150f;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
            return;

        if (!IsHitOnThisObject(hit.collider.transform))
            return;

        PlayClick();
    }

    /// <summary>
    /// True if the collider belongs to this object or a child (e.g. mesh child holds the collider).
    /// </summary>
    private bool IsHitOnThisObject(Transform hitTransform)
    {
        return hitTransform == transform
               || hitTransform.IsChildOf(transform)
               || transform.IsChildOf(hitTransform);
    }

    private void PlayClick()
    {
        if (audioSource == null)
        {
            Debug.LogWarning($"ClickSound on '{name}': assign an Audio Source.", this);
            return;
        }

        if (clickClip != null)
        {
            audioSource.PlayOneShot(clickClip);
            return;
        }

        if (audioSource.clip != null)
        {
            audioSource.Play();
            return;
        }

        Debug.LogWarning(
            $"ClickSound on '{name}': assign Click Clip or put a clip on the Audio Source — nothing to play.",
            this);
    }
}
