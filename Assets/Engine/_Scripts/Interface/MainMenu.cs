using UnityEngine;
using System.Collections;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Start Flash")]
    [SerializeField] TMP_Text StartText;
    [SerializeField] float FlashDuration = 0.5f;
    [SerializeField] AnimationCurve FlashCurve;

    private bool active = true;

    IEnumerator StartFlash()
    {
        float t = 0.0f;

        while (active)
        {
			yield return null;

            t += Time.deltaTime / FlashDuration;

            float flashValue = FlashCurve.Evaluate(t % 1f);

            StartText.color = new Color(StartText.color.r, StartText.color.g, StartText.color.b, flashValue);
		}
    }

    void EnterPressed()
    {
        Debug.Log("Enter Pressed");
        GameSceneManager.Instance.ChangeScene(GameSceneManager.Scenes.Level);
    }

	private void OnEnable()
	{
        InputHandler.Instance.OnAction += EnterPressed;
	}

	private void OnDisable()
	{
		InputHandler.Instance.OnAction -= EnterPressed;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        if (StartText != null)
        {
			StartCoroutine(StartFlash());
		}
        else
        {
            Debug.LogError("Start Flashing Text Not Assigned");
        }
    }

	private void Update()
	{
		
	}
}
