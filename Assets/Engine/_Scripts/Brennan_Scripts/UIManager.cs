using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Text keyText;
    public Image[] hearts;

    private void Awake()
    {
        if(Instance ==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateKeyUI(int keyCount)
    {
        keyText.text = keyCount.ToString();

    }

    public void UpdateHeartsUI(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i < health); // show hearts only if health > i
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
