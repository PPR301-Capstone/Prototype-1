using System.Collections;
using UnityEngine;

public class Weapon : Item
{
    Animator animator;
    public enum WeaponType
    {
        SWORD_1H,
        SWORD_2H,
        AXE_1H,
        AXE_2H,
        POLEARM,
        RANGED
    }

    public WeaponType weaponType;
    public float WeaponSpeed;
    public int WeaponDamage;
    private bool canAttack = true;

    IEnumerator AttackRoutine()
    {
        canAttack = false;
		animator.SetBool("IsAttacking", true);

		yield return new WaitForSeconds(WeaponSpeed);

		animator.SetBool("IsAttacking", false);
        canAttack = true;
	}

    public void Attack()
    {
        if (canAttack)
            StartCoroutine(AttackRoutine());
    }

    public void Charge(float duration)
    {

    }

	private void Start()
	{
		animator = GetComponent<Animator>();
	}
}
