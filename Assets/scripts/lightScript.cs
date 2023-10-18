using UnityEngine;

public class LightControl : MonoBehaviour
{
    private Light myLight;
    public VRFade vrFade;

    private void Start()
    {
        // Get the Light component attached to this GameObject
        myLight = GetComponent<Light>();
        vrFade.FadeToBlack();
    }


    public void TurnOffLight()
    {
        if (myLight != null)
        {
            myLight.enabled = false;
        }
    }

    // Optional: To turn the light back on
    public void TurnOnLight()
    {
        if (myLight != null)
        {
            myLight.enabled = true;
        }
    }
}
