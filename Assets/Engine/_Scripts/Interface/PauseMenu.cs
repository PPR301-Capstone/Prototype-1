using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Parent;

    [Header("Buttons")]
    [SerializeField] GameObject[] Buttons;

    private int currentIndex = 0;
    private bool isActive = true;

    // PauseMenubuttons

    public void Show()
    {
        Parent.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Hide()
    {
        Parent.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {

    }

    public void Options()
    {

    }

    public void Quit()
    {

    }

    private void Navigate(Vector2 input)
    {
        if (!isActive)
            return;

        currentIndex = (currentIndex + (int) input.y + Buttons.Length) % Buttons.Length;

		Debug.Log($"Navigate: {input} {currentIndex}");
    }

    private void Toggle()
    {
        Debug.Log("Escape");

        if (!isActive)
        {
            Show();
        }
        else
        {
            Hide();
        }

        isActive = !isActive;
    }

	private void OnEnable()
	{
        InputHandler.Instance.OnUINavigate += Navigate;
        InputHandler.Instance.OnEscapePressed += Toggle;
	}

	private void OnDisable()
	{
		InputHandler.Instance.OnUINavigate -= Navigate;
        InputHandler.Instance.OnEscapePressed -= Toggle;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        Debug.Log("Pause Menu Toggling: " + isActive);
        Toggle();
    }
}
