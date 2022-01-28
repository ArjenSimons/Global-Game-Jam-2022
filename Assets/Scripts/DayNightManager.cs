using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DayPart
{
    DAY,
    NIGHT
}

public class DayNightManager : MonoBehaviour
{
    [SerializeField] float dayTime = 30;
    [SerializeField] float nightTime = 30;

    public static DayNightManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DayNightManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("DayNightManager");
                    instance = go.AddComponent<DayNightManager>();
                }
            }
            return instance;
        }
    }

    private static DayNightManager instance;

}
