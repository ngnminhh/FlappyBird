using UnityEngine;

public class PipeController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float triggerDistance = 5f; // Kho?ng cách ?? ?ng b?t ??u di chuy?n
    public Transform player; // Gán t? Unity Editor ho?c t? tìm

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Ch? di chuy?n n?u player ??n g?n (theo tr?c X)
        if (transform.position.x - player.position.x < triggerDistance)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 currentPosition = transform.position;

                currentPosition.y = Mathf.Lerp(currentPosition.y, mousePosition.y, moveSpeed * Time.deltaTime);
                transform.position = currentPosition;
            }
        }
    }
}
