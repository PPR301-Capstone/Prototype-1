using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    bool gameOver = false;
    public void GameOver(int score)
    {
        ScoreText.text = score.ToString();
        gameOver = true;

    }

    public void Continue()
    {
        if (!gameOver)
            return;

		Time.timeScale = 1.0f;
		GameSceneManager.Instance.ChangeScene(GameSceneManager.Scenes.MainMenu);
	}

	private void OnEnable()
	{
		InputHandler.Instance.OnUISubmit += Continue;
	}

	private void OnDisable()
	{
		InputHandler.Instance.OnUISubmit -= Continue;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
