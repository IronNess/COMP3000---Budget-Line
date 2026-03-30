using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 4f;
    public float turnSpeed = 10f;

    private Vector3 input;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        input = new Vector3(x, 0f, z).normalized;

        if (animator != null)
            animator.SetFloat("Speed", input.magnitude);

        transform.position += input * speed * Time.deltaTime;

        if (input.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}