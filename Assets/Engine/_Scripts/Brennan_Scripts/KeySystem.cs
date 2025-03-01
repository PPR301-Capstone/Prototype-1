using UnityEngine;
using UnityEngine.UI;

public class KeySystem : MonoBehaviour
{
    public int keysCollected = 0;
    public Text keyText;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateKeyUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.UpdateKeyUI(1);
            Destroy(gameObject);
        }
    }

    public void CollectKey()
    {
        keysCollected = 1; // only one key per level for now
        UpdateKeyUI();
    }

    void UpdateKeyUI()
    {
        keyText.text = keysCollected.ToString();
    }
}
