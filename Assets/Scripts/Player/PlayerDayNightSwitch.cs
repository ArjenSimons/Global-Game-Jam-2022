using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDayNightSwitch : MonoBehaviour
{
    public bool isNightMonster;
    [SerializeField]
    private GameObject monsterNight, monsterDay;
    [SerializeField] AudioSource daysong, nightsong, gong, squeek;

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
        DayNightManager.Instance.OnDayEnd += OnDayEnd;
        DayNightManager.Instance.OnNightEnd += OnNightEnd;
    }

    private void OnDay ()
    {
        daysong.Play();
        monsterDay.SetActive(true);
        monsterNight.SetActive(false);
        isNightMonster = false;
    }

    private void OnDayEnd ()
    {
        daysong.Stop();
        gong.Play();
    }

    private void OnNight()
    {
        nightsong.Play();
        monsterDay.SetActive(false);
        monsterNight.SetActive(true);
        isNightMonster = true;
    }

    private void OnNightEnd()
    {
        nightsong.Stop();
        squeek.Play();
    }
}
