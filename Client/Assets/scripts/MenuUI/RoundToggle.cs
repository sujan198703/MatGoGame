using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class RoundToggle : MonoBehaviour
{
    [SerializeField] private bool isOn = false;
    [SerializeField] private GameObject isOnObject;
    [SerializeField] private GameObject isOffObject;
    private void Awake()
    {
        UpdateToggleState();
        GetComponent<Button>().onClick.AddListener(()=> SetToggleState());
    }

    public void SetToggleState()
    {
        isOn = !isOn;
        UpdateToggleState();
    }

    private void UpdateToggleState()
    {
        if (isOn)
        {
            isOnObject.SetActive(false);
            isOffObject.SetActive(true);
        }
        else
        {
            isOnObject.SetActive(true);
            isOffObject.SetActive(false);
        }
    }
}
