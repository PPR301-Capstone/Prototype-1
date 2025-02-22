using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Parent;

    [Header("Buttons")]
    [SerializeField] UIButton[] Buttons;

    private UIButton currentButton;

    private int currentIndex = 0;
    private bool isActive = true;

    // PauseMenubuttons

    public void FocusButton()
    {
        if (!isActive || !currentButton) 
            return;

		currentButton.Focus();
    }

    public void SelectButton(int index)
    {
        if (currentButton != null)
            currentButton.Unfocus();

		currentButton = Buttons[index];

        currentButton.Focus();

        Debug.Log(currentButton);
	}

    void RefreshButtons()
    {
        if (isActive)
        {
            Buttons = Parent.GetComponentsInChildren<UIButton>();
        }
    }

    public void Show()
    {
        Parent.SetActive(true);
		RefreshButtons();

		Time.timeScale = 0.0f;

        currentIndex = 0;
        SelectButton(currentIndex);
    }

    public void Hide()
    {
        if (currentButton != null)
            currentButton.Unfocus();

		Parent.SetActive(false);

        Time.timeScale = 1.0f;
        currentButton = null;
        currentIndex = 0;
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

        currentIndex = (currentIndex + (int) -input.y + Buttons.Length) % Buttons.Length;

        SelectButton(currentIndex);
		Debug.Log($"Navigate: {input} {currentIndex}");
    }

    private void Toggle()
    {
        Debug.Log("Escape");
		isActive = !isActive;

		if (isActive)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

	private void OnEnable()
	{
        if (InputHandler.Instance != null)
        {
			InputHandler.Instance.OnUINavigate += Navigate;
			InputHandler.Instance.OnEscapePressed += Toggle;
		}
        else
        {
            Debug.LogError("Make sure InputHandler is in the scene");
        }
	}

	private void OnDisable()
	{
        if (InputHandler.Instance != null)
        {
			InputHandler.Instance.OnUINavigate -= Navigate;
			InputHandler.Instance.OnEscapePressed -= Toggle;
		}
        else
        {
			//Debug.LogError("Make sure InputHandler is in the scene");
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        Debug.Log("Pause Menu Toggling: " + isActive);
        Toggle();
    }
}
