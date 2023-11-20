using UnityEngine;

public class DoorController : MonoBehaviour
{
    private enum DoorState { Closed, Opening, Opened, Closing }
    private DoorState doorState = DoorState.Closed;

    private float openAngle = -30f; // Adjust the open angle to rotate the door
    private float initialAngle = 0;
    private float creakOpenSpeed = 10f;

    private void Start()
    {
        initialAngle = transform.eulerAngles.y;
        // Initialize other components or settings as needed
    }

    private void Update()
    {
        switch (doorState)
        {
            case DoorState.Opening:
                float targetAngle = initialAngle + openAngle;
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, creakOpenSpeed * Time.deltaTime);

                if (Mathf.Approximately(transform.eulerAngles.y, targetAngle))
                {
                    doorState = DoorState.Opened;
                }
                break;

            case DoorState.Opened:
                // Add any logic to be performed when the door is fully opened
                break;

            case DoorState.Closing:
                // Add any logic to be performed when the door is closing
                break;
        }
    }

    public void ActivateDoor()
    {
        if (doorState == DoorState.Closed)
        {
            doorState = DoorState.Opening;
        }
    }
}
