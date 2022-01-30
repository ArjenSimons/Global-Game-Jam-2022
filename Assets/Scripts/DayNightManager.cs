using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

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

    [SerializeField] Camera cam;
    [SerializeField] Transform world;
    [SerializeField] Transform player;

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

    public event Action OnTransitionToNightStart;


    private static DayNightManager instance;
    private bool gameStateActive { get { return CurrentDayState != DayState.NONE && CurrentDayState != DayState.TRANSITION; } }

    private float time;
    private int dayCount;

    private void Start()
    {
        Color tempColor = imgNightShader.color;
        tempColor.a = 0f;
        imgNightShader.color = tempColor;

        player = Player.Instance.transform;
        StartCoroutine(DelayedTransitionToNight(.1f));
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

        cam.transform.DOMove(new Vector3(0, 0, -10), 1f).SetEase(Ease.OutQuad);
        cam.DOOrthoSize(30, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            StartCoroutine(ZoomInCam(2f));
        });

        imgNightShader.DOFade(0, 4f).OnComplete(() =>
        {
            CurrentDayState = DayState.DAY;
            OnDayStart?.Invoke();
        });
    }

    private void TransitionToNight()
    {
        CurrentDayState = DayState.TRANSITION;

        OnTransitionToNightStart?.Invoke();

        float targetScale = .15f;

        cam.transform.SetParent(null);
        cam.transform.DOMove(new Vector3(0, 0, -10), 1f).SetEase(Ease.OutQuad);
        cam.DOOrthoSize(30, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            StartCoroutine(ZoomInCam(2f));
        });

        imgNightShader.DOFade(.3f, 4f).OnComplete(() =>
        {
            CurrentDayState = DayState.NIGHT;
            OnNightStart?.Invoke();
        });
    }

    private IEnumerator ZoomInCam(float delay)
    {
        yield return new WaitForSeconds(delay);
        cam.transform.SetParent(player);
        cam.transform.DOLocalMove(new Vector3(0, 0, -10), 1f).SetEase(Ease.InQuad);
        cam.DOOrthoSize(5, 1f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            //if (toNight)
            //{

            //}
        });
    }

    private IEnumerator DelayedTransitionToNight(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TransitionToNight();
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
