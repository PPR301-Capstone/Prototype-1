using System;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField] public LayerMask DetectionLayers;
    [SerializeField] string DetectionTag = "";

    public bool isTriggered = false;
    //  Sensor Events
    public event Action<bool> OnSensorTriggered;

    public void DispatchEvent()
    {
		OnSensorTriggered?.Invoke(isTriggered);
	}

    public bool CheckLayers(GameObject gobject)
    {
        return (((1 << gobject.layer) & DetectionLayers) != 0);
    }

    public bool CheckTag(GameObject gobject)
    {
        if (gobject.tag == DetectionTag || DetectionTag == "")
            return true;

        return false;
    }
}
