using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player Instance;

    PlayerController playerController;
	public int Health;
	public int MaxHealth = 300;

	public void TakeDamage(int damage)
	{
		Health -= damage;

		UI_HUD.Instance.SetHealth(Health);
	}

	public void GiveHealth(int amount)
	{
		if (Health >= MaxHealth || Health + amount >= MaxHealth)
		{
			amount = MaxHealth;
		}
		else
		{
			Health += amount;
		}
	}

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
		Instance = this;
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

		Instance = null;
	}
}
