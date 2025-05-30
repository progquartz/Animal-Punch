using System;
using System.Collections.Generic;

[Serializable]
public class BoxInventory
{
    public List<BoxCount> boxCounts = new();

    [Serializable]
    public class BoxCount
    {
        public string boxId;
        public int count;
    }

    public int GetCount(string id)
    {
        var box = boxCounts.Find(b => b.boxId == id);
        return box != null ? box.count : 0;
    }

    public void Add(string id, int amount = 1)
    {
        var box = boxCounts.Find(b => b.boxId == id);
        if (box == null)
        {
            box = new BoxCount { boxId = id, count = amount };
            boxCounts.Add(box);
        }
        else
        {
            box.count += amount;
        }
    }

    public bool Use(string id)
    {
        var box = boxCounts.Find(b => b.boxId == id);
        if (box != null && box.count > 0)
        {
            box.count--;
            return true;
        }
        return false;
    }

    public List<BoxCount> GetAllOwnedBoxes()
    {
        return boxCounts.FindAll(b => b.count > 0);
    }
}
