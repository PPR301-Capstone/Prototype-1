using UnityEngine;

public class UI_HUD : MonoBehaviour
{
    public static UI_HUD Instance;

    //  Health UI
    [SerializeField] GameObject HeartContainer;
    [SerializeField] GameObject heartPrefab;

    private UIHeart[] Hearts;

    public void RefreshHearts(int hearts)
    {
        Hearts = new UIHeart[hearts];

        for (int i = 0; i < hearts; i++)
        {
            GameObject _heart = Instantiate(heartPrefab);
            _heart.name = $"Heart_{i}";
            _heart.transform.parent = HeartContainer.transform;

            Hearts[i] = _heart.GetComponent<UIHeart>();
        }
    }

    UIHeart GetCurrentHearts()
    {
        for (int i = 3; i >= 0; i--)
        {
            UIHeart heart = Hearts[i];

            if (heart.HeartAmount > 0)
                return heart;
        }

        return null;
    }

    public void SetHealth(int health)
    {

    }

	private void Awake()
	{
        Instance = this;
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