using UnityEngine;

public class UnderMaintenancePanel : MonoBehaviour
{
    public bool IsUnderMaintenance()
    {
        return false;
    }

    public void ShowUnderMaintenancePanel()
    {
        gameObject.SetActive(true);
    }
}
