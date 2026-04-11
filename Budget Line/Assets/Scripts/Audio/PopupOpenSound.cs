using UnityEngine;

/// <summary>
/// Plays a one-shot when informational / event popups open.
/// Uses <paramref name="clip"/> if set; otherwise uses <see cref="AudioSource.clip"/> on the source.
/// If the <see cref="AudioSource"/> is missing, disabled, or on an inactive object, falls back to
/// <see cref="AudioSource.PlayClipAtPoint"/> (Unity cannot call <see cref="AudioSource.PlayOneShot"/> on a disabled source).
/// </summary>
public static class PopupOpenSound
{
    /// <param name="clip">Optional override. If null, falls back to <paramref name="source"/>.clip.</param>
    /// <param name="source">Optional; uses mixer/output on this component only when it can play.</param>
    public static void TryPlay(AudioClip clip, AudioSource source = null)
    {
        AudioClip toPlay = clip;
        if (toPlay == null && source != null)
            toPlay = source.clip;

        if (toPlay == null)
            return;

        if (source != null && source.isActiveAndEnabled)
        {
            source.PlayOneShot(toPlay);
            return;
        }

        Vector3 pos = Camera.main != null ? Camera.main.transform.position : Vector3.zero;
        AudioSource.PlayClipAtPoint(toPlay, pos);
    }
}
