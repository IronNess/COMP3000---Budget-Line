using UnityEngine;

/// <summary>
/// Plays the <see cref="AudioSource"/> on the same GameObject as an <see cref="IInteractable"/>, if present.
/// Called immediately before <see cref="IInteractable.Interact"/> from click and keyboard paths.
/// </summary>
public static class InteractAudio
{
    public static void PlayBeforeInteract(IInteractable interactable)
    {
        if (interactable == null)
            return;

        var mb = interactable as MonoBehaviour;
        if (mb == null)
            return;

        var source = mb.GetComponent<AudioSource>();
        if (source == null)
            return;

        if (source.clip != null)
            source.PlayOneShot(source.clip);
    }
}
