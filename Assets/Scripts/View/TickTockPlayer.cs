using UnityEngine;
using System.Collections;

public class TickTockPlayer : MonoBehaviour {
    public AudioClip tick, tock;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTick() {
        audioSource.clip = tick;
        audioSource.Play();
    }

    public void PlayTock() {
        audioSource.clip = tock;
        audioSource.Play();
    }

}
