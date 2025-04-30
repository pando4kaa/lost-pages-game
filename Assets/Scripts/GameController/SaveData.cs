using UnityEngine;
using System.Collections.Generic;
[System.Serializable]

public class SaveData 
{
    public Vector3 kentPosition;
    public string mapBoundary;
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
    
}
