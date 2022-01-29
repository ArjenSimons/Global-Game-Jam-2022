using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDayNightSwitch : MonoBehaviour
{
    public bool isNightMonster;
    [SerializeField]
    private GameObject monsterNight, monsterDay;

    public static PlayerDayNightSwitch Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerDayNightSwitch>();
                if (instance == null)
                {
                    GameObject go = new GameObject("PlayerHiding");
                    instance = go.AddComponent<PlayerDayNightSwitch>();
                }
            }
            return instance;
        }
    }

    private static PlayerDayNightSwitch instance;

    void Start()
    {
        isNightMonster = false;

        DayNightManager.Instance.OnDayStart += OnDay;
        DayNightManager.Instance.OnNightStart += OnNight;
    }

    private void OnDay ()
    {
        monsterDay.SetActive(true);
        monsterNight.SetActive(false);
        isNightMonster = false;
    }

    private void OnNight()
    {
        monsterDay.SetActive(false);
        monsterNight.SetActive(true);
        isNightMonster = true;
    }
}
