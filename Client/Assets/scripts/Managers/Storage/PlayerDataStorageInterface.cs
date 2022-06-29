using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerDataStorageInterface
{
    public void LoadData(PlayerDataManager data);

    public void SaveData(ref PlayerDataManager data);
}
