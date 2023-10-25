using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour
{
    private Light myLight;
    private float originalIntensity = 1.5f;
    private float minFlickerDuration = 0.02f; // Minimum time for the flicker duration
    private float maxFlickerDuration = 0.2f; // Maximum time for the flicker duration
    private int flickerCount = 5; // Number of times to flicker

    [Header("Creepy Effect")]
    public bool useCreepyEffect = true;  // This will appear as a checkbox in the inspector

    private void Start()
    {
        // Get the Light component attached to this GameObject
        myLight = GetComponent<Light>();
        originalIntensity = myLight.intensity;

        // Initialize the light state based on checkbox value
        if (useCreepyEffect)
            TurnOnLight();
        else
            TurnOffLight();
    }

    private void OnValidate()
    {
        // This will ensure the script finds the Light component even in the editor
        if (myLight == null)
            myLight = GetComponent<Light>();

        if (useCreepyEffect)
            TurnOnLight();
        else
            TurnOffLight();
    }

    public void TurnOffLight()
    {
        if (myLight != null)
        {
            if (useCreepyEffect)
                StartCoroutine(CreepyFlicker(false));
            else
                myLight.enabled = false;
        }
    }

    public void TurnOnLight()
    {
        if (myLight != null)
        {
            if (useCreepyEffect)
                StartCoroutine(CreepyFlicker(true));
            else
                myLight.enabled = true;
        }
    }

    private IEnumerator CreepyFlicker(bool turningOn)
    {
        for (int i = 0; i < flickerCount; i++)
        {
            myLight.enabled = !myLight.enabled; // Toggle the light state
            yield return new WaitForSeconds(Random.Range(minFlickerDuration, maxFlickerDuration)); // Wait for a random duration
        }

        if (turningOn)
        {
            myLight.enabled = true;
            myLight.intensity = originalIntensity * 0.5f; // Dim the light slightly for added effect
            yield return new WaitForSeconds(0.5f); // Wait for half a second
            myLight.intensity = originalIntensity; // Restore original intensity
        }
        else
        {
            myLight.enabled = false;
        }
    }
}
