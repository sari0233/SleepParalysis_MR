using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainController : MonoBehaviour
{
    public LightController lightControl;
    public CanvasGroup blackCanvas;
    public AudioSource breathingAudio;
    public AudioSource staticAndHummingAudio;
    public AudioSource doorCreakAudio;
    public AudioSource footstepsAudio; // Reference to the AudioSource for footsteps
    public DoorController doorController;

    public float roomFadeDuration = 5.0f; // Adjust as needed

    void Start()
    {
        StartCoroutine(StartExperience());
    }

    IEnumerator StartExperience()
    {
        // Turn off lights slightly
        lightControl.TurnOffLight();

        // Wait for a short duration before starting the fade to black
        yield return new WaitForSeconds(2.0f);

        // Fade to black
        yield return StartCoroutine(FadeToBlack(5.0f));

        // Play breathing audio for 15 seconds
        yield return StartCoroutine(PlayAudioForDuration(breathingAudio, 15.0f));

        // Play static and humming audio
        yield return StartCoroutine(PlayAudioForDuration(staticAndHummingAudio, 10.0f));

        // Open the door with a slight creak
        yield return StartCoroutine(OpenDoorWithCreak());

        // Play footsteps audio
        yield return StartCoroutine(PlayAudioForDuration(footstepsAudio, 5.0f));

        // Fade back to the usual state
        yield return StartCoroutine(FadeToUsualState());

        // Turn on lights
        lightControl.TurnOnLight();

    }

    IEnumerator FadeToBlack(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            blackCanvas.alpha = Mathf.Lerp(0, 1, timer / duration);
            yield return null;
        }
    }

    IEnumerator PlayAudioForDuration(AudioSource audioSource, float duration)
    {
        if (audioSource != null)
        {
            audioSource.Play();

            // Wait for the specified duration
            yield return new WaitForSeconds(duration);

            // Stop the audio after the duration has passed
            audioSource.Stop();
        }
    }

    IEnumerator OpenDoorWithCreak()
    {
        if (doorController != null && doorCreakAudio != null)
        {
            // Activate the door, and the DoorController script will handle the opening with a creak
            doorController.ActivateDoor();

            // Play the door creak audio
            StartCoroutine(PlayAudioForDuration(doorCreakAudio, 4));

            // Wait for the creak sound to finish before continuing
            yield return new WaitForSeconds(doorCreakAudio.clip.length);
        }
    }

    IEnumerator FadeToUsualState()
    {
        float timer = 0f;
        float startAlpha = blackCanvas.alpha;

        while (timer < roomFadeDuration)
        {
            timer += Time.deltaTime;
            blackCanvas.alpha = Mathf.Lerp(startAlpha, 0, timer / roomFadeDuration);
            yield return null;
        }
    }
}
