using UnityEngine;

public class PlayerAnim : SpriteAnim
{
    [SerializeField] Animator animator;

    public void Walk(bool isMoving)
    {
        animator.SetBool("IsMoving", isMoving);
    }

    public void Jump(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (animator == null)
        {
            Debug.LogWarning("Please attach animator from sprite component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
