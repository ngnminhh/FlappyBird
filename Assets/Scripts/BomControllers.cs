using UnityEngine;

public class BomController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float triggerDistance = 5f; // Khoảng cách để ống bắt đầu di chuyển
    public Transform player; // Gán từ Unity Editor hoặc tự tìm

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

        // Chỉ di chuyển nếu player đến gần (theo trục X)
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
