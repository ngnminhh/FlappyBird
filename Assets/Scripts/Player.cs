using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

    public float speed = 2f; // bay ngang
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 2f;

    private Vector3 startPos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startPos = transform.position;
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void Update()
    {
        float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        //transform.position += Vector3.right * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, startPos.y + y, transform.position.z);
    }

    private void AnimateSprite()
    {
        spriteIndex = (spriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            FindFirstObjectByType<GameManager>().GameOver();
        }
        else if (other.CompareTag("Scoring"))
        {
            Debug.Log("Chim đã đi qua ống! Tăng điểm.");
            FindFirstObjectByType<GameManager>().IncreaseScore();
        }
    }
}
