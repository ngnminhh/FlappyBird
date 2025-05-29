using UnityEngine;

public class BomController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float triggerDistance = 5f;
    public Transform player;

    private Camera mainCamera;
    private bool isDragging = false;
    private float startMouseY;

    public float minY = -3f;
    public float maxY = 3f;

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

        if (transform.position.x - player.position.x < triggerDistance)
        {
            Vector3 mousePos = Input.mousePosition;

            // ✅ Bắt đầu thao tác ở bất kỳ đâu
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                startMouseY = mousePos.y;
            }

            // Đang giữ và vuốt
            if (Input.GetMouseButton(0) && isDragging)
            {
                float currentMouseY = mousePos.y;
                float deltaY = currentMouseY - startMouseY;

                Vector3 currentPosition = transform.position;
                currentPosition.y += Mathf.Sign(deltaY) * moveSpeed * Time.deltaTime;

                // Giới hạn vùng di chuyển
                currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

                transform.position = currentPosition;

                // Cập nhật để giữ cảm giác mượt
                startMouseY = currentMouseY;
            }

            // Thả chuột => dừng thao tác
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }
}
