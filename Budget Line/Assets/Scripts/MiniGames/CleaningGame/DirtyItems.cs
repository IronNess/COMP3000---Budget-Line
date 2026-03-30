using UnityEngine;

public class DirtyItem : MonoBehaviour
{
    [SerializeField] private CleaningGameController controller;
    private bool isCleaned = false;

    private void OnMouseDown()
    {
        if (isCleaned) return;

        isCleaned = true;

        if (controller != null)
            controller.ItemCleaned();

        gameObject.SetActive(false);
    }
}