using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUIController : MonoBehaviour
{
    public Image socketStatusImage;
    public Color statusColorConnected = new Color32(15, 195, 115, 255);
    public Color statusColorDisconnected = new Color32(255, 50, 50, 255);

    public RectTransform socketOptionsPanel;

    private RectTransform activePanel;

    public InputField simUrlInputField;
    private GameManager gameManager;

    public RectTransform menuPanel;
    private bool initialized = false;

    public void Init()
    {
        SetSocketState(false);
        if (socketOptionsPanel != null)
        {
            socketOptionsPanel.gameObject.SetActive(false);
        }
        initialized = true;
        gameManager = GetComponent<GameManager>();
    }

    public void SetSocketState(bool connected)
    {
        if (socketStatusImage != null)
        {
            socketStatusImage.color = connected ? statusColorConnected : statusColorDisconnected;
        }
    }

    void Update()
    {
    }

    public void HandleEscapeEvent(int sceneIndex)
    {
        if (activePanel != null)
        {
            activePanel.gameObject.SetActive(false);
            activePanel = null;
            if (sceneIndex != 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleControls>().SetControlType(VehicleControls.ControlType.Mouse);
            }
        }
        else
        {
            if (menuPanel != null && sceneIndex != 0)
            {
                menuPanel.gameObject.SetActive(true);
                activePanel = menuPanel;
                GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleControls>().SetControlType(VehicleControls.ControlType.Off);
            }
        }
    }

    public void CloseMenu()
    {
        menuPanel.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleControls>().SetControlType(VehicleControls.ControlType.Mouse);
        activePanel = null;
    }

    public void ToggleSocketOptions()
    {
        if (socketOptionsPanel != null)
        {
            socketOptionsPanel.gameObject.SetActive(!socketOptionsPanel.gameObject.activeSelf);
            if (socketOptionsPanel.gameObject.activeSelf)
            {
                activePanel = socketOptionsPanel;
            }
            else
            {
                activePanel = null;
            }
        }
    }

    public void ApplySocketUrl()
    {
        if (simUrlInputField != null)
        {
            if (simUrlInputField.text.Length > 0 && gameManager != null)
            {
                gameManager.SetSimulatorURL(simUrlInputField.text);
                print(simUrlInputField.text);
            }
        }
    }
}
