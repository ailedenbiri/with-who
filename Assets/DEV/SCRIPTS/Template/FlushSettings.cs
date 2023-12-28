using UnityEngine;
using UnityEngine.UI;

public class FlushSettings : MonoBehaviour
{
    public GameObject hapticEnabledObject;
    public GameObject hapticDisabledObject;

    private bool isOpen = true; // Variable to store the open/closed state of the setting

    // Define a unique key for PlayerPrefs
    private string hapticPlayerPrefsKey = "HapticEnabled";

    void Start()
    {

        // Load the setting state from PlayerPrefs when the game starts
        if (PlayerPrefs.HasKey(hapticPlayerPrefsKey))
        {
            isOpen = PlayerPrefs.GetInt(hapticPlayerPrefsKey) == 1;
        }
        UpdateSettingState();

        // Add the ToggleOpenCloseSetting method to the buttons
        Button hapticEnabledButton = hapticEnabledObject.GetComponent<Button>();
        Button hapticDisabledButton = hapticDisabledObject.GetComponent<Button>();

        if (hapticEnabledButton != null)
        {
            hapticEnabledButton.onClick.AddListener(ToggleOpenCloseSetting);
        }

        if (hapticDisabledButton != null)
        {
            hapticDisabledButton.onClick.AddListener(ToggleOpenCloseSetting);
        }
    }

    // Function called when the setting is opened or closed
    public void ToggleOpenCloseSetting()
    {
        // Toggle the state of the setting
        isOpen = !isOpen;
        UpdateSettingState();

        // Save the setting state to PlayerPrefs
        PlayerPrefs.SetInt(hapticPlayerPrefsKey, isOpen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Update the active status of the objects based on the setting's state
    private void UpdateSettingState()
    {
        if (isOpen)
        {
            Taptic.tapticOn = true;
            Debug.Log("Haptic is enabled.");
        }
        else
        {
            Taptic.tapticOn = false;
            Debug.Log("Haptic is disabled.");
        }
        hapticEnabledObject.SetActive(isOpen);
        hapticDisabledObject.SetActive(!isOpen);
    }
}