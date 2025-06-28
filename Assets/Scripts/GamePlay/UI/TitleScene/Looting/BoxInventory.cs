using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

[Serializable]
public class BoxInventory
{
    public List<BoxCount> boxCounts = new();

    [Serializable]
    public class BoxCount
    {
        public BoxRankType boxId;
    }

    public void Add(BoxRankType id)
    {
        var box = new BoxCount { boxId = id };
        boxCounts.Add(box);
    }

    public bool Use(int index)
    {
        var box = boxCounts[index];
        if (box != null)
        {
            boxCounts.Remove(box);
            return true;
        }
        return false;
    }

    public BoxRankType GetType(int index)
    {
        return boxCounts[index].boxId;
    }

    public List<BoxCount> GetAllOwnedBoxes()
    {
        return boxCounts;
    }
}
