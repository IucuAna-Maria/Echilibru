using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip buttonSelect;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlayButtonSound()
    {
        SFXSource.PlayOneShot(buttonSelect);
    }

}
