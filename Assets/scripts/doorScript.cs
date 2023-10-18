using UnityEngine;

public class DoorController : MonoBehaviour
{
    private enum DoorState { Closed, Opening, Opened, Closing }
    private DoorState doorState = DoorState.Closed;

    private float openAngle = -90f;
    private float initialAngle = 0;
    private float slamShutSpeed = 150f;
    private float creakOpenSpeed = 10f;

    private void Start()
    {
        initialAngle = transform.eulerAngles.y;
        ActivateDoor();
    }

    private void Update()
    {
        switch (doorState)
        {
            case DoorState.Opening:
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, initialAngle + openAngle, creakOpenSpeed * Time.deltaTime);

                if (Mathf.Approximately(transform.eulerAngles.y, initialAngle + openAngle))
                {
                    doorState = DoorState.Opened;
                }
                break;

            case DoorState.Opened:
                doorState = DoorState.Closing; // transition to closing state immediately after fully opened
                break;

            case DoorState.Closing:
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, initialAngle, slamShutSpeed * Time.deltaTime);

                if (Mathf.Approximately(transform.eulerAngles.y, initialAngle))
                {
                    doorState = DoorState.Closed;
                }
                break;
        }
    }

    public void ActivateDoor()
    {
        if (doorState == DoorState.Closed)
            doorState = DoorState.Opening;
    }
}
