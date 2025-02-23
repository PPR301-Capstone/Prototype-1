using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlay : MonoBehaviour
{
    [SerializeField] float fadeDuration = 1.0f;
    Image overlayImage;

    [SerializeField] Color StartColor = Color.black;
    [SerializeField] Color EndColor = Color.white;

    public void Show()
    {
        if (overlayImage == null)
            return;

        StartCoroutine(Fade(EndColor, StartColor));
    }

    public void Hide()
    {
		if (overlayImage == null)
			return;

		StartCoroutine(Fade(StartColor, EndColor));
	}

    IEnumerator Fade(Color source, Color destination)
    {
        float t = 0.0f;

        while (t < fadeDuration)
        {
            yield return null;
            t += Time.deltaTime;

            overlayImage.color = Color.Lerp(source, destination, t);
        }
    }

	private void Awake()
	{
		overlayImage = GetComponent<Image>();
	}

	private void OnValidate()
	{
        this.GetComponent<Image>().color = StartColor;
	}
}
