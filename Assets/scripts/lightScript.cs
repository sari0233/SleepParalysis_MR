using UnityEngine;

public class LightControl : MonoBehaviour
{
    private Light myLight;
    

    private void Start()
    {
        // Get the Light component attached to this GameObject
        myLight = GetComponent<Light>();
        
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
