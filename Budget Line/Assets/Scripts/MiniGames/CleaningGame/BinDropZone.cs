using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BinDropZone : MonoBehaviour
{
    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }
}