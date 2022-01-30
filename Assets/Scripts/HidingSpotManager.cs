using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpotManager : MonoBehaviour
{
    public static HidingSpotManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HidingSpotManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("HidingSpotManager");
                    instance = go.AddComponent<HidingSpotManager>();
                }
            }
            return instance;
        }
    }

    public List<HidingSpot> AvailableHidingSpots = new List<HidingSpot>();

    private static HidingSpotManager instance;

    private List<HidingSpot> allHidingSpots = new List<HidingSpot>();

    public void AddHidingSpot(HidingSpot hidingSpot)
    {
        if (!allHidingSpots.Contains(hidingSpot))
        {
            allHidingSpots.Add(hidingSpot);
            AvailableHidingSpots.Add(hidingSpot);
        }
    }

    public void RemoveHidingSpot(HidingSpot hidingSpot)
    {
        if (allHidingSpots.Contains(hidingSpot))
        {
            allHidingSpots.Remove(hidingSpot);
        }
        if (AvailableHidingSpots.Contains(hidingSpot))
        {
            AvailableHidingSpots.Remove(hidingSpot);
        }
    }

    public HidingSpot ClaimRandomHidingSpot()
    {
        if (AvailableHidingSpots.Count == 0)
        {
            Debug.Log("yeet");
            return null;
        }

        HidingSpot spot = AvailableHidingSpots[Random.Range(0, AvailableHidingSpots.Count)];
        AvailableHidingSpots.Remove(spot);
        return spot;
    }

    public void StopHidingSpotUsage(HidingSpot hidingSpot)
    {
        if (!AvailableHidingSpots.Contains(hidingSpot))
        {
            AvailableHidingSpots.Add(hidingSpot);
        }
    }
}
