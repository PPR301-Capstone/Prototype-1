using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player Instance;

    PlayerController playerController;
	public int Health;
	public int MaxHealth = 300;

	[SerializeField] GameObject WeaponSlot;
	public Weapon currentWeapon;

	IEnumerator DeathRoutine()
	{
		this.GetComponent<CircleCollider2D>().enabled = false;
		this.GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * 5f;
		yield return new WaitForSeconds(1.5f);
		this.GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * 5f;
		GameManager.Instance.GameOver();
	}

	public void Die()
	{

		CameraController.Instance.StopFollowing();
		Debug.Log("Game Over");

		StartCoroutine(DeathRoutine());
	}

	public void TakeDamage(int damage)
	{
		if (playerController.IsPlayerControlDisabled)
			return;

		Health -= damage;
		UI_HUD.Instance.SetHealth(Health);

		if (Health <= 0)
		{
			Die();
		}
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

		if (WeaponSlot == null)
			Debug.LogError($"Player does not have weapon slot");

		if (WeaponSlot.transform.childCount > 0)
		{
			currentWeapon = WeaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>();
			Debug.Log($"Player equipped: {currentWeapon.name} ({currentWeapon.ID})");
		}
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
