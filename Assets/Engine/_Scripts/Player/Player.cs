using UnityEngine;

public class Player : MonoBehaviour
{
	
    PlayerController playerController;
	public int Health;
	public int MaxHealth = 300;

    public void EnableControl()
    {
		Debug.Log("Player enabled");
        playerController.IsPlayerControlDisabled = false;
    }

    public void DisableControl()
    {
		Debug.Log("Player disabled");
		playerController.IsPlayerControlDisabled = true;
	}

	private void Awake()
	{
		playerController = GetComponent<PlayerController>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		Health = MaxHealth;

		UI_HUD.Instance.RefreshHearts(MaxHealth / 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnEnable()
	{
        if (GameManager.Instance != null)
        {
			GameManager.Instance.OnPlayerEnable += EnableControl;
			GameManager.Instance.OnPlayerDisable += DisableControl;
		}
	}

	private void OnDisable()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnPlayerEnable -= EnableControl;
			GameManager.Instance.OnPlayerDisable -= DisableControl;
		}
	}
}
