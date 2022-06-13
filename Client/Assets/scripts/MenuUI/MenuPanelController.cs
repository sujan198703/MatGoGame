using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelController : MonoBehaviour
{
    [SerializeField] private List<MenuPanel> menuPanels = new List<MenuPanel>();
    
    public void OpenMenu(string menuName) { 
        foreach(MenuPanel menuPanel in menuPanels)
        {
            if (menuPanel.panelName.Equals(menuName)){
                menuPanel.panel.SetActive(true);
                menuPanel.panelButton.SetActive(true);
            }
            else
            {
                menuPanel.panel.SetActive(false);
                menuPanel.panelButton.SetActive(false);
            }
        }
    }
}
[System.Serializable]
public class MenuPanel
{
    public string panelName;
    public GameObject panel;
    public GameObject panelButton;
}