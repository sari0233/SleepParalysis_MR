using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{
    private Light myLight;
    private float originalIntensity = 1.5f;
    private float minFlickerDuration = 0.02f;
    private float maxFlickerDuration = 0.2f;
    private int flickerCount = 5;

    public bool useCreepyEffect = true;

    private void Start()
    {
        myLight = GetComponent<Light>();
        originalIntensity = myLight.intensity;

        if (useCreepyEffect)
            TurnOnLight();
        else
            TurnOffLight();
    }

    private void OnValidate()
    {
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

    public IEnumerator CreepyFlicker(bool turningOn)
    {
        for (int i = 0; i < flickerCount; i++)
        {
            myLight.enabled = !myLight.enabled;
            yield return new WaitForSeconds(Random.Range(minFlickerDuration, maxFlickerDuration));
        }

        if (turningOn)
        {
            myLight.enabled = true;
            myLight.intensity = originalIntensity * 0.5f;
            yield return new WaitForSeconds(0.5f);
            myLight.intensity = originalIntensity;
        }
        else
        {
            myLight.enabled = false;
        }
    }
}
