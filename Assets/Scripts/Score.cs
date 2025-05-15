using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    private void OnTriggerEnter2D(Collider2D collistion)
    {
        if (collistion.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.coinClip);
            Destroy(gameObject);
        }
    }
}
