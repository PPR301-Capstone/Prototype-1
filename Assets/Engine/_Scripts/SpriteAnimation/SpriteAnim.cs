using UnityEngine;

public class SpriteAnim : MonoBehaviour
{
    [SerializeField] SpriteRenderer SpriteRenderer;
    public void FlipSprite(Vector3 movement)
    {
        if (movement.x == 0)
            return;

        if (movement.x < 0)
        {
            this.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
			this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

	private void Awake()
	{
		
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
