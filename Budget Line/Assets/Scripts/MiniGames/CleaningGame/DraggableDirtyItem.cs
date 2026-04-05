using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DraggableDirtyItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CleaningGameController controller;

    [Header("Drag Settings")]
    [SerializeField] private LayerMask dragSurfaceLayer;
    [SerializeField] private float dragHeightOffset = 0.1f;
    [SerializeField] private float dropCheckRadius = 1.25f;

    private Camera mainCamera;
    private Collider itemCollider;
    private Vector3 startPosition;
    private Vector3 dragOffset;
    private bool isDragging;
    private bool isCleaned;

    private void Awake()
    {
        mainCamera = Camera.main;
        itemCollider = GetComponent<Collider>();
        startPosition = transform.position;

        if (controller == null)
            controller = FindObjectOfType<CleaningGameController>();
    }

    private void Start()
    {
        if (controller != null)
            controller.RegisterItem();
    }

    private void OnMouseDown()
    {
        if (isCleaned || controller == null || controller.IsGameEnded || mainCamera == null)
            return;

        isDragging = true;

        if (TryGetDragPoint(out Vector3 dragPoint))
        {
            dragOffset = transform.position - dragPoint;
        }
        else
        {
            dragOffset = Vector3.zero;
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging || isCleaned || mainCamera == null)
            return;

        if (TryGetDragPoint(out Vector3 dragPoint))
        {
            Vector3 targetPosition = dragPoint + dragOffset;
            targetPosition.y = startPosition.y + dragHeightOffset;
            transform.position = targetPosition;
        }
    }

    private void OnMouseUp()
    {
        if (!isDragging || isCleaned)
            return;

        isDragging = false;

        if (IsOverBin())
        {
            CleanItem();
        }
        else
        {
            ReturnToStartPosition();
        }
    }

    private bool TryGetDragPoint(out Vector3 dragPoint)
    {
        dragPoint = Vector3.zero;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, dragSurfaceLayer))
        {
            dragPoint = hit.point;
            return true;
        }

        return false;
    }

    private bool IsOverBin()
    {
        Vector3 checkCentre = itemCollider != null ? itemCollider.bounds.center : transform.position;

        Collider[] hits = Physics.OverlapSphere(checkCentre, dropCheckRadius);

        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<BinDropZone>() != null)
                return true;
        }

        return false;
    }

    private void CleanItem()
    {
        isCleaned = true;

        if (controller != null)
            controller.ItemCleaned();

        gameObject.SetActive(false);
    }

    private void ReturnToStartPosition()
    {
        transform.position = startPosition;
    }
}