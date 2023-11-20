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
    public DoorController doorController;


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

        // Play static and humming sounds simultaneously
        PlayStaticAndHummingAudio();

        // Open the door with a slight creak
        yield return StartCoroutine(OpenDoorWithCreak());
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

    void PlayStaticAndHummingAudio()
    {
        if (staticAndHummingAudio != null)
        {
            staticAndHummingAudio.Play();
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
            doorController.HandleDoorAction();


            doorController.HandleDoorMovement();

            // Play the door creak sound
            doorCreakAudio.Play();

            // Wait for the creak sound to finish before continuing
            yield return new WaitForSeconds(doorCreakAudio.clip.length);
        }
    }
}
