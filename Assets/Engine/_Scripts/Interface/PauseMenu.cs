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
        Debug.Log("Resume");
        TogglePause();
    }

    public void Options()
    {
        Debug.Log("Options");
    }

    public void Quit()
    {
        Debug.Log("Quit");

        UnsubscribeFromEvents();

		Time.timeScale = 1.0f;
        GameSceneManager.Instance.ChangeScene(GameSceneManager.Scenes.MainMenu);
    }

    private void Submit()
    {
        switch (currentIndex)
        {
            case 0:
                Resume();
                break;
            case 1:
                Options();
                break;
            case 2:
                Quit();
                break;
        }
    }

    private void Navigate(Vector2 input)
    {
        if (!isActive)
            return;

        currentIndex = (currentIndex + (int) -input.y + Buttons.Length) % Buttons.Length;

        SelectButton(currentIndex);
    }

    private void TogglePause()
    {
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

    void SubscribeToEvents()
    {
		InputHandler.Instance.OnUINavigate += Navigate;
		InputHandler.Instance.OnEscapePressed += TogglePause;
		InputHandler.Instance.OnAction += Submit;
	}

    void UnsubscribeFromEvents()
    {
		InputHandler.Instance.OnUINavigate -= Navigate;
		InputHandler.Instance.OnEscapePressed -= TogglePause;
		InputHandler.Instance.OnAction -= Submit;
	}

	private void OnEnable()
	{
        if (InputHandler.Instance != null)
        {
            SubscribeToEvents();

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
            UnsubscribeFromEvents();
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        Debug.Log("Pause Menu Toggling: " + isActive);
        TogglePause();
    }
}
