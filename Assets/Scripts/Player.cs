using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

    public float speed = 2f; // bay ngang
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 2f;
    

    private float targetAmplitude;
    private float targetFrequency;

    public float smoothSpeed = 2f; // tốc độ chuyển mượt
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
        int score = GameManager.Instance != null ? GameManager.Instance.CurrentScore : 0;

        if (score >= 30)
        {
            floatAmplitude = 1.0f;  // dao động mạnh hơn nữa
            floatFrequency = 4f;    // dao động rất nhanh
        }
        else if (score >= 20)
        {
            floatAmplitude = 0.8f;  // dao động cao hơn
            floatFrequency = 3f;    // dao động nhanh hơn
        }
        else
        {
            floatAmplitude = 0.5f;  // bình thường
            floatFrequency = 2f;
        }

        float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, startPos.y + y, transform.position.z);

    }
    //private void Update()
    //{
    //    int score = GameManager.Instance != null ? GameManager.Instance.CurrentScore : 0;

    //    float t = Mathf.Clamp01(score / 30f); // mốc max 30

    //    // Tính target dựa trên điểm số
    //    targetAmplitude = Mathf.Lerp(0.5f, 1.0f, t);
    //    targetFrequency = Mathf.Lerp(2f, 4f, t);

    //    // Di chuyển giá trị hiện tại dần dần đến target
    //    floatAmplitude = Mathf.Lerp(floatAmplitude, targetAmplitude, Time.deltaTime * smoothSpeed);
    //    floatFrequency = Mathf.Lerp(floatFrequency, targetFrequency, Time.deltaTime * smoothSpeed);

    //    // Áp dụng chuyển động
    //    float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
    //    transform.position = new Vector3(transform.position.x, startPos.y + y, transform.position.z);
    //}


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
            Debug.Log(" đã đi qua ống! Tăng điểm.");
            FindFirstObjectByType<GameManager>().IncreaseScore();
        }
    }
}
