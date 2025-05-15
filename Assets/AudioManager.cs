using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;

    public AudioClip musicClip;
    public AudioClip coinClip;
    public AudioClip winClip;
    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    // Update is called once per frame
   public void PlaySFX(AudioClip sfxClip)
    {
        vfxAudioSource.clip = sfxClip;
        musicAudioSource.PlayOneShot(sfxClip);
    }
}
