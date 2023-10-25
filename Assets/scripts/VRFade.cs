using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VRFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.0f;

    private bool isFading = false;

    public void FadeToBlack()
    {
        if (!isFading)
            StartCoroutine(FadeCoroutine(0f, 1f));  // Fade from transparent to black
    }

    public void FadeFromBlack()
    {
        if (!isFading)
            StartCoroutine(FadeCoroutine(1f, 0f));  // Fade from black to transparent
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha)
    {
        isFading = true;
        float elapsed = 0f;

        Color initialColor = fadeImage.color;
        initialColor.a = startAlpha;

        Color targetColor = fadeImage.color;
        targetColor.a = endAlpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = Color.Lerp(initialColor, targetColor, elapsed / fadeDuration);
            yield return null;
        }

        fadeImage.color = targetColor;
        isFading = false;
    }
}
    