using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary>
/// Loads minigame scenes. Optionally shows/hides a <see cref="menuPanel"/> overlay.
/// Keep your always-visible Room button <b>outside</b> <see cref="menuPanel"/> so it is not hidden on play.
/// </summary>
public class MiniGameMenuUI : MonoBehaviour
{
    [Header("Optional overlay (hidden until Open)")]
    [Tooltip("Assign only the popup/panel that lists minigames — not the GameObject with the always-visible button.")]
    [FormerlySerializedAs("panel")]
    [SerializeField] private GameObject menuPanel;

    private void Awake()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }

    public void Open()
    {
        if (menuPanel != null)
            menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private static void LoadMiniGameScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void OpenCleaningGame() => LoadMiniGameScene("MiniGame_Cleaning");

    public void OpenMissingWordsGame() => LoadMiniGameScene("MiniGame_MissingWords");

    public void OpenSandwichGame() => LoadMiniGameScene("MiniGame_Sandwich");

    public void OpenEnergyBarGame() => LoadMiniGameScene("MiniGame_EnergyBar");
}
