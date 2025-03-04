using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour
{
    [SerializeField] Image Heart;
    public int HeartAmount = 100;
    public void SetHealth(int value)
    {
        HeartAmount = value;
        Heart.fillAmount = value/100f;
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
