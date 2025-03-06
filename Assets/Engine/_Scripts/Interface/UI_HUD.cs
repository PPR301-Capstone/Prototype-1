using UnityEngine;
using TMPro;

public class UI_HUD : MonoBehaviour
{
    public static UI_HUD Instance;

    //  Health UI
    [SerializeField] GameObject HeartContainer;
    [SerializeField] GameObject heartPrefab;

    [SerializeField] TextMeshProUGUI ScoreText;
    private UIHeart[] Hearts;


    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString("D5");
    }

    public void RefreshHearts(int hearts)
    {
        Hearts = new UIHeart[hearts];

        for (int i = 0; i < hearts; i++)
        {
            GameObject _heart = Instantiate(heartPrefab, HeartContainer.transform);
            _heart.name = $"Heart_{i}";

            Hearts[i] = _heart.GetComponent<UIHeart>();
            Hearts[i].SetHealth(100);
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