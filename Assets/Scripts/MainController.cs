using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainController : MonoBehaviour
{
    public LightController lightControl;
    public CanvasGroup blackCanvas;
    public AudioSource breathingAudio;
    public AudioSource staticAudio;
    public AudioSource hummingAudio;
    public AudioSource doorCreakAudio;
    public AudioSource footstepsAudio; // Reference to the AudioSource for footsteps
    public DoorController doorController;
    public AudioSource heartBeatAudio2;
    public AudioSource heartBeatAudio1;
    public GameObject tvGameObject;

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
        yield return StartCoroutine(PlayAudiosForDuration(staticAudio, hummingAudio, 10.0f));

        // Open the door with a slight creak
        yield return StartCoroutine(OpenDoorWithCreak());

        // Fade back to the usual state
        yield return StartCoroutine(FadeToUsualState());

        // Turn on lights
        lightControl.TurnOnLight();

        // Play footsteps audio
        yield return StartCoroutine(PlayAudioForDuration(footstepsAudio, 5.0f));

        // Play heartbeat audio - 2
        yield return StartCoroutine(PlayAudioForDuration(heartBeatAudio2, 15.0f));

        // Fade to black
        yield return StartCoroutine(FadeToBlack(5.0f));

        // Play breathing + heartbeat1 audio together
        yield return StartCoroutine(PlayAudiosForDuration(breathingAudio, heartBeatAudio1, 25.0f));

        // Play static humming audio with volume 3 - 2
        yield return StartCoroutine(PlayAudiosForDuration(staticAudio, hummingAudio, 10.0f));

        // Turn TV on
        PlayVideoOnTV();

        // Fade to room
        yield return StartCoroutine(FadeToUsualState());

        yield return new WaitForSeconds(10.0f);

        // Turn TV off
        StopVideoOnTV();

        // Fade to black
        yield return StartCoroutine(FadeToBlack(5.0f));

        // Play heartbeat audio - 2
        yield return StartCoroutine(PlayAudioForDuration(heartBeatAudio2, 15.0f));


        // Open door fully
        doorController.openAngle = -90;
        yield return StartCoroutine(OpenDoorWithCreak());

        // Fade to room
        yield return StartCoroutine(FadeToUsualState());


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


    IEnumerator PlayAudiosForDuration(AudioSource audioSource1, AudioSource audioSource2,float duration)
    {
        if (audioSource1 != null && audioSource2 != null)
        {
            audioSource1.Play();
            audioSource2.Play();

            // Wait for the specified duration
            yield return new WaitForSeconds(duration);

            // Stop the audio after the duration has passed
            audioSource1.Stop();
            audioSource2.Stop();
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

    void PlayVideoOnTV()
    {
        // Assuming you have a reference to the PlayVideo script on the TV GameObject
        PlayVideo playVideoScript = tvGameObject.GetComponent<PlayVideo>();

        // Check if the script reference is not null
        if (playVideoScript != null)
        {
            // Trigger the method to play the video on the TV screen
            playVideoScript.Play();
        }
        else
        {
            Debug.LogError("PlayVideo script not found on the TV GameObject.");
        }
    }

    void StopVideoOnTV()
    {
        // Assuming you have a reference to the PlayVideo script on the TV GameObject
        PlayVideo playVideoScript = tvGameObject.GetComponent<PlayVideo>();

        // Check if the script reference is not null
        if (playVideoScript != null)
        {
            // Trigger the method to stop the video on the TV screen
            playVideoScript.Stop();
        }
        else
        {
            Debug.LogError("PlayVideo script not found on the TV GameObject.");
        }
    }
}
