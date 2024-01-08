using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

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
    public AudioSource heartBeatAudio3;
    public AudioSource heartBeatAudio4;
    public GameObject tvGameObject;
    public GameObject clownGameObject;
    private Animator clownAnimator; // Animator for the Clown

    public InputActionReference primaryButtonAction; // Drag your PrimaryButtonAction here in the inspector

    private bool primaryButtonPressed = false;
    public int buttonPressCount = 0;

    public float roomFadeDuration = 5.0f; // Adjust as needed

    void Start()
    {
        primaryButtonAction.action.started += OnPrimaryButtonPress;
        StartCoroutine(StartExperience());

        if (clownGameObject != null)
        {
            clownAnimator = clownGameObject.GetComponent<Animator>();
            if (clownAnimator == null)
            {
                Debug.LogError("Animator component not found on the Clown GameObject.");
            }
        }
        else
        {
            Debug.LogError("Clown GameObject reference not set.");
        }
    }

    void OnPrimaryButtonPress(InputAction.CallbackContext context)
    {
        primaryButtonPressed = true;
    }

    IEnumerator StartExperience()
    {
        while (true)
        {
            yield return null;

            if (primaryButtonPressed)
            {
                primaryButtonPressed = false;

                switch (buttonPressCount)
                {
                    case 0:
                        yield return StartCoroutine(FirstPressActions());
                        yield return StartCoroutine(SecondPressActions());
                        yield return StartCoroutine(ThirdPressActions());
                        yield return StartCoroutine(FourthPressActions());
                        yield return StartCoroutine(FifthPressActions());
                        yield return StartCoroutine(SixthPressActions());
                        yield return StartCoroutine(SeventhPressActions());
                        yield return StartCoroutine(EighthPressActions());
                    break;
                    case 1:
                        yield return StartCoroutine(tenthPressActions());
                        break;
                    case 2:
                        yield return StartCoroutine(EleventhPressActions());
                        break;
                    case 3:
                        yield return StartCoroutine(TwelfthPressActions());
                        break;

                    // Add more cases as needed
                    default:
                        Debug.Log("No more actions defined for additional button presses.");
                        break;
                }

                // Increment the button press counter
                buttonPressCount++;
                Debug.Log(buttonPressCount);
            }
        }
    }

    // Define your actions for each press
    IEnumerator FirstPressActions()
    {
        Debug.Log("First queue started: Turn lights off and Fade to black");
        lightControl.TurnOffLight();
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(FadeToBlack(5.0f));
    }

    IEnumerator SecondPressActions()
    {
        Debug.Log("Second queue started: Play breathing audio and static + humming audio + open door creak");
        yield return StartCoroutine(PlayAudioForDuration(breathingAudio, 15.0f));
        yield return StartCoroutine(PlayAudiosForDuration(staticAudio, hummingAudio, 10.0f));
        yield return StartCoroutine(OpenDoorWithCreak());

    }

    IEnumerator ThirdPressActions()
    {
        Debug.Log("Third queue started: Fade to usual and Turn on lights");
        yield return StartCoroutine(FadeToUsualState());
        lightControl.TurnOnLight();
    }

    IEnumerator FourthPressActions()
    {
        Debug.Log("Fourth queue started: Play footsteps and heartbeat audio");
        yield return StartCoroutine(PlayAudioForDuration(heartBeatAudio2, 15.0f));
    }

    IEnumerator FifthPressActions()
    {
        Debug.Log("Fifth queue started: FadeToBlack and play breathing + static audio");
        yield return StartCoroutine(FadeToBlack(5.0f));
        yield return StartCoroutine(PlayAudiosForDuration(breathingAudio, heartBeatAudio1, 25.0f));
        yield return StartCoroutine(PlayAudiosForDuration(staticAudio, hummingAudio, 10.0f));
        yield return StartCoroutine(PlayAudioForDuration(footstepsAudio, 5.0f));
    }

    IEnumerator SixthPressActions()
    {
        Debug.Log("Sixth queue started: Play video on TV and FadeToUsualState and Stop video on TV and fade to black");
        PlayVideoOnTV();
        yield return StartCoroutine(FadeToUsualState());
        yield return new WaitForSeconds(10.0f);
        StopVideoOnTV();
        yield return StartCoroutine(FadeToBlack(5.0f));
    }

    IEnumerator SeventhPressActions()
    {
        Debug.Log("Seventh queue started: play heartbeat audio and fadetousual state");
        yield return StartCoroutine(PlayAudioForDuration(heartBeatAudio2, 15.0f));
        doorController.openAngle = -90;
        yield return StartCoroutine(OpenDoorWithCreak());
        yield return StartCoroutine(FadeToUsualState());
    }

    IEnumerator EighthPressActions()
    {
        yield return StartCoroutine(PlayAudioForDuration(heartBeatAudio3, 7.0f));
        lightControl.CreepyFlicker(true);
    }

    IEnumerator NinthPressActions()
    {
        heartBeatAudio3.volume = 0.4f;
        yield return StartCoroutine(PlayAudioForDuration(heartBeatAudio3, 10.0f));
    }
    IEnumerator tenthPressActions()
    {
        Debug.Log("Ninth queue started: Trigger Clown animation and move forward");
        yield return StartCoroutine(TriggerClownAnimationAndMove());
    }

    IEnumerator EleventhPressActions()
    {
        yield return StartCoroutine(FadeToBlack(5.0f));
        clownGameObject.transform.position = new Vector3(1.2f, 0.7f, -0.55f);
    }
    IEnumerator TwelfthPressActions()
    {
        yield return StartCoroutine(FadeToUsualState());

        clownAnimator.SetTrigger("removeGlasses");
    }


    IEnumerator FadeToBlack(float duration)
    {
        Debug.Log("Fading to black...");
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


    IEnumerator PlayAudiosForDuration(AudioSource audioSource1, AudioSource audioSource2, float duration)
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

    IEnumerator TriggerClownAnimationAndMove()
    {
        if (clownAnimator != null)
        {
            // Trigger the animation
            clownAnimator.SetTrigger("OutOfTv" ); // Replace with your actual trigger name

            float animationDuration = 3500.0f; // Duration of the animation in seconds
            

            float startTime = Time.time;

            while (Time.time - startTime < 20f)
            {
                // Calculate the normalized time for animation
                float animationTime = (Time.time - startTime) / animationDuration;

                // Interpolate the position between start and end points
                Vector3 startPosition = clownGameObject.transform.position;
                Vector3 endPosition = new Vector3(0f, 0.67f, 1f);
                Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, animationTime);

                // Apply the position to the Clown GameObject
                clownGameObject.transform.position = currentPosition;

                yield return null;
            }

            // Ensure that the Clown GameObject is at the final position
            clownGameObject.transform.position = new Vector3(0f, 0.67f, 1f);
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
