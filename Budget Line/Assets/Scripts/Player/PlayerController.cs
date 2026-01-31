using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(x, y).normalized;
        transform.Translate(move * speed * Time.deltaTime);
    }
}
