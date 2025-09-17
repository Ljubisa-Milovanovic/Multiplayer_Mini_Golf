using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Drag your three audio clips here in the Unity Inspector
    public AudioClip[] songs;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
            return;
        }

        if (songs.Length < 3)
        {
            Debug.LogError("Please assign at least 3 songs to the 'songs' array in the Inspector.");
            return;
        }
        StartCoroutine(PlaySongsInSequence());
    }

    private IEnumerator PlaySongsInSequence()
    {
        foreach (AudioClip song in songs)
        {
            audioSource.clip = song;

            audioSource.Play();

            yield return new WaitForSeconds(song.length);
        }

        Debug.Log("All songs have finished playing.");
    }
}