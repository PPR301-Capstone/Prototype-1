using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Loading")]
    [SerializeField] LevelSequence levelSequence;

    [Header("Level Config")]
    public Level level;

    public Player player;

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

	private void Awake()
	{
		Instance = this;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
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
