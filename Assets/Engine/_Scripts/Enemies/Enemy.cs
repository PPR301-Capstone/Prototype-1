using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string Name = "Enemy";
    public int Health = 100;
	public int MaxHealth = 100;
	public int ScoreValue = 50;

	[SerializeField] WSHealthBar healthBar;
    [SerializeField] GameObject WeaponSlot;
	[SerializeField] SpriteRenderer spriteRenderer;

    public Weapon CurrentWeapon;

    CapsuleCollider2D cc2d;

	IEnumerator DeathRoutine()
	{
		float t = 0;

		Color P = new Color(1, 1, 1, 1);
		Color Q = new Color(1, 1, 1, 0);

		while (t <= 1)
		{
			t += Time.deltaTime;

			spriteRenderer.color = Color.Lerp(P, Q, t);
			CurrentWeapon.GetComponent<SpriteRenderer>().color = Color.Lerp(P, Q, t);
			
			yield return null;
		}
		Destroy(this.gameObject);
	}

	public void Die()
	{
		GameManager.Instance.AddScore(ScoreValue);
		this.GetComponent<EnemyAgent>().SetCurrentState(EnemyAgent.AgentState.Dead);
		StartCoroutine(DeathRoutine());
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;

		healthBar.SetHealth(Health);

		if (Health <= 0)
		{
			Die();
		}
	}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc2d = GetComponent<CapsuleCollider2D>();

		if (WeaponSlot == null)
			Debug.LogError($"{this.name} does not have weapon slot");

        if (healthBar == null)
            Debug.LogError($"{this.name} does not have a health bar assigned");

		if (WeaponSlot.transform.childCount > 0)
		{
			CurrentWeapon = WeaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>();
			Debug.Log($"{Name} equipped: {CurrentWeapon.name} ({CurrentWeapon.ID})");
		}

		healthBar.RefreshHearts(MaxHealth / 100);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "WeaponHitbox")
		{
			Weapon weapon = collision.gameObject.transform.parent.GetComponent<Weapon>();
			TakeDamage(weapon.WeaponDamage);

			Debug.Log($"Enemy hit by {weapon.name}");
		}
	}
}
