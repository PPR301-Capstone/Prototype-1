using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Loading")]
    [SerializeField] LevelSequence levelSequence;

    [Header("Level Config")]
    public Level level;
    [SerializeField] GameObject GameOverUI;

    public Player player;

    public event Action OnPlayerDisable;
    public event Action OnPlayerEnable;

    int score = 0;
    public bool isGameOver = false;

    public void AddScore(int value)
    {
        score += value;

        UI_HUD.Instance.UpdateScore(score);
    }

    void LoadLevel()
    {
        if (levelSequence != null)
        {
            levelSequence.LoadLevel();
        }
        else
        {

        }
    }

    public void GameOver()
    {
		OnPlayerDisable?.Invoke();
        isGameOver = true;
        GameOverUI.SetActive(true);

        GameOverUI.GetComponent<UIGameOver>().GameOver(score);
		Pause();
    }

    public void Pause()
    {
        OnPlayerDisable?.Invoke();

        Time.timeScale = 0.0f;
    }

    public void Unpause()
    {
        OnPlayerEnable?.Invoke();

		Time.timeScale = 1.0f;
	}

	private void Awake()
	{
		Instance = this;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        GameOverUI.SetActive(false);

        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDestroy()
	{
        Instance = null;
	}
}
