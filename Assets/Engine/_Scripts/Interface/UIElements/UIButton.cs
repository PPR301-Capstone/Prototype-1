using UnityEngine;
using TMPro;

public class UIButton : MonoBehaviour
{

    [Header("Button Text")]
    [SerializeField] TMP_Text buttonText;
    [SerializeField] string ButtonString = "Button";

    [Header("Highlights")]
    [SerializeField] Color DefaultColour = Color.white;
    [SerializeField] Color FocusColour = Color.green;
    [SerializeField] Color DisabledColour = Color.gray;

    public void Focus()
    {
        buttonText.color = Color.green;
    }

    public void Unfocus()
    {
        buttonText.color = Color.white;
    }

	private void Awake()
	{
		
	}

	private void OnValidate()
	{
        if (buttonText != null)
        {
			this.buttonText.text = ButtonString;
		}
        else
        {
            Debug.LogError($"{this.name} TMP_Text not assigned");
        }
	}
}
