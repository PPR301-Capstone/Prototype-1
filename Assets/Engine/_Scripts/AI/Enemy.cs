using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string Name = "Enemy";
    public int Health = 100;

    [SerializeField] GameObject WeaponSlot;
    public Weapon CurrentWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		if (WeaponSlot == null)
			Debug.LogError($"Player does not have weapon slot");

		if (WeaponSlot.transform.childCount > 0)
		{
			CurrentWeapon = WeaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>();
			Debug.Log($"{Name} equipped: {CurrentWeapon.name} ({CurrentWeapon.ID})");
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
