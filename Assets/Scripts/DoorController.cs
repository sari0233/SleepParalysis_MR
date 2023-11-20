using UnityEngine;

public class DoorController : MonoBehaviour
{
    private enum DoorState { Closed, Opening, Opened, Closing }
    private DoorState doorState = DoorState.Closed;

    private float openAngle = -90f;
    private float initialAngle = 0;
    private float slamShutSpeed = 250f;

    public bool isOpen = false;

    public AudioClip doorSlamClip;
    public AudioClip doorOpenClip;

    public AnimationCurve openingSpeedCurve;
    private float openingTimeElapsed = 0.0f;
    private float totalOpeningTime = 2.0f;  // Total time for door to open, adjust as needed.

    private bool hasPlayedSlamSound = false;
    private bool hasPlayedOpenSound = false;

    private void Start()
    {
        initialAngle = transform.eulerAngles.y;
    }

    private void OnValidate()
    {
        HandleDoorAction();
    }

    private void Update()
    {
        HandleDoorMovement();
    }

    public void HandleDoorMovement()
    {
        switch (doorState)
        {
            case DoorState.Opening:
                if (!hasPlayedOpenSound)
                {
                    hasPlayedOpenSound = true;
                }

                openingTimeElapsed += Time.deltaTime;
                float t = openingTimeElapsed / totalOpeningTime;
                float currentSpeed = openingSpeedCurve.Evaluate(t) * 90f;  // Assuming 90� total rotation
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, initialAngle + openAngle, currentSpeed * Time.deltaTime);

                if (Mathf.Approximately(transform.eulerAngles.y, initialAngle + openAngle))
                {
                    doorState = DoorState.Opened;
                    openingTimeElapsed = 0; // Reset for next time door opens
                }
                break;

            case DoorState.Closing:
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, initialAngle, slamShutSpeed * Time.deltaTime);

                if (!hasPlayedSlamSound && Mathf.Abs(transform.eulerAngles.y - initialAngle) < 25f)
                {
                    hasPlayedSlamSound = true;
                }

                if (Mathf.Approximately(transform.eulerAngles.y, initialAngle))
                {
                    doorState = DoorState.Closed;
                    hasPlayedSlamSound = false;
                }
                break;
        }
    }

    public void HandleDoorAction()
    {
        if (isOpen && (doorState == DoorState.Closed || doorState == DoorState.Closing))
        {
            doorState = DoorState.Opening;
        }
        else if (!isOpen && (doorState == DoorState.Opened || doorState == DoorState.Opening))
        {
            doorState = DoorState.Closing;
        }
    }
}
