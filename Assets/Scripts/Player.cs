using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private Vector3 startPos;
    private bool isDead = false;
    private Rigidbody2D rb;

    private Quaternion defaultRotation;
    private Vector3 defaultScale;

    private Coroutine dieEffectCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;
        defaultRotation = transform.rotation;
        defaultScale = transform.localScale;

        if (rb != null)
        {
            rb.gravityScale = 0f; // Tắt trọng lực khi bắt đầu
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void Update()
    {
        if (isDead) return;

        float floatAmplitude = 0.5f;
        float floatFrequency = 2f;

        float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, startPos.y + y, transform.position.z);
    }

    private void AnimateSprite()
    {
        spriteIndex = (spriteIndex + 1) % sprites.Length;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Obstacle"))
        {
            FindFirstObjectByType<GameManager>().GameOver();

            dieEffectCoroutine = StartCoroutine(DieEffectCoroutine());

        }
        else if (other.CompareTag("Scoring"))
        {
            FindFirstObjectByType<GameManager>().IncreaseScore();
        }
    }

    private IEnumerator DieEffectCoroutine()
    {
        isDead = true;
        CancelInvoke(nameof(AnimateSprite));

        float duration = 0.9f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        if (rb != null)
        {
            rb.gravityScale = 3f;
            rb.linearVelocity = Vector2.down * 2f;
        }

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.rotation = Quaternion.Euler(0, 0, t * 360f);
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void ResetPlayer()
    {
        isDead = false;

        // Dừng hiệu ứng chết nếu còn đang chạy
        if (dieEffectCoroutine != null)
        {
            StopCoroutine(dieEffectCoroutine);
            dieEffectCoroutine = null;
        }

        CancelInvoke(nameof(AnimateSprite));
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);

        transform.position = startPos;
        transform.rotation = defaultRotation;
        transform.localScale = defaultScale;

        if (spriteRenderer != null && sprites.Length > 0)
        {
            spriteIndex = 0;
            spriteRenderer.sprite = sprites[spriteIndex];
            spriteRenderer.enabled = true;
        }

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
