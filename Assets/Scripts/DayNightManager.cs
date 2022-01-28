using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public enum DayState
{
    NONE,
    DAY,
    NIGHT,
    TRANSITION
}

public class DayNightManager : MonoBehaviour
{
    [SerializeField] float dayTime = 30;
    [SerializeField] float nightTime = 30;
    [SerializeField] Image imgNightShader;

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

    public DayState CurrentDayState { get; private set; }
    public event Action OnDayStart;
    public event Action OnDayEnd;
    public event Action OnNightStart;
    public event Action OnNightEnd;


    private static DayNightManager instance;
    private bool gameStateActive { get { return CurrentDayState != DayState.NONE && CurrentDayState != DayState.TRANSITION; } }

    private float time;
    private int dayCount;

    private void Start()
    {
        CurrentDayState = DayState.DAY;

        Color tempColor = imgNightShader.color;
        tempColor.a = 0f;
        imgNightShader.color = tempColor;
    }

    private void Update()
    {
        if (gameStateActive)
        {
            time += Time.deltaTime;

            if (CurrentDayState == DayState.DAY)
            {
                HandleDayState();
            }
            else if (CurrentDayState == DayState.NIGHT)
            {
                HandleNightState();
            }
        }
    }

    private void HandleDayState()
    {
        if (time >= dayTime)
        {
            EndDayState();
            time = 0;
        }
    }

    private void HandleNightState()
    {
        if (time >= nightTime)
        {
            EndNightState();
            time = 0;
        }
    }

    private void TransitionToDay()
    {
        CurrentDayState = DayState.TRANSITION;
        dayCount++;

        imgNightShader.DOFade(0, 1f).OnComplete(() =>
        {
            CurrentDayState = DayState.DAY;
            OnDayStart?.Invoke();
        });
    }

    private void TransitionToNight()
    {
        CurrentDayState = DayState.TRANSITION;
        imgNightShader.DOFade(.3f, 1f).OnComplete(() =>
        {
            CurrentDayState = DayState.NIGHT;
            OnDayStart?.Invoke();
        });
    }

    private void EndDayState()
    {
        OnDayEnd?.Invoke();
        TransitionToNight();
    }

    private void EndNightState()
    {
        OnNightEnd?.Invoke();
        TransitionToDay();
    }
}
