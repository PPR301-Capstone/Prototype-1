using UnityEngine;

public class EnemyAnim : SpriteAnim
{
    Animator animator;

    public void PlayIdle()
    {

    }

    public void PlayWalk(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }

    public void PlayAttack(bool isAttack)
    {
        animator.SetBool("isAttacking", isAttack);
    }

	private void Awake()
	{
		animator = GetComponent<Animator>();
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
