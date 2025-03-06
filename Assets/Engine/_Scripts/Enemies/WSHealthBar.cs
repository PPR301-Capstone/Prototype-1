using UnityEngine;

public class WSHealthBar : MonoBehaviour
{
    [SerializeField] GameObject HeartContainer;
    [SerializeField] GameObject HeartPrefab;

    int health;
    UIHeart[] Hearts;

	public void RefreshHearts(int hearts)
	{
		Hearts = new UIHeart[hearts];

		for (int i = 0; i < hearts; i++)
		{
			GameObject _heart = Instantiate(HeartPrefab, HeartContainer.transform);
			_heart.name = $"Heart_{i}";

			Hearts[i] = _heart.GetComponent<UIHeart>();
			Hearts[i].SetHealth(100);
		}
	}

	public void SetHealth(int health)
	{
		Debug.Log("Set HP: " + health);

		int remainingHealth = health;

		for (int i = 0; i < Hearts.Length; i++)
		{
			if (remainingHealth >= 100)
			{
				Hearts[i].SetHealth(100);
				remainingHealth -= 100;
			}
			else
			{
				Hearts[i].SetHealth(remainingHealth);
				remainingHealth = 0;
			}
		}
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
