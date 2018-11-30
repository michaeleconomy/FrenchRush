using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {
    public AudioClip correct, wrong, timeUp, start;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCorrect() {
        audioSource.clip = correct;
        audioSource.Play();
    }

    public void PlayWrong() {
        audioSource.clip = wrong;
        audioSource.Play();
    }

    public void PlayTimeUp() {
        audioSource.clip = timeUp;
        audioSource.Play();
    }

    public void PlayStart() {
        audioSource.clip = start;
        audioSource.Play();
    }

}
