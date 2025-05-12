using UnityEngine;

public class Pipes : MonoBehaviour
{
    public float moveSpeed = 3f;

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < -10f) // Khi vượt qua trái màn hình
        {
            Destroy(gameObject);
        }
    }
}
