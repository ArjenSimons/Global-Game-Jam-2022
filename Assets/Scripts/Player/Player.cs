using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class Player : MonoBehaviour
{
    [SerializeField] public float PatrollPointRangeMin = 13;
    [SerializeField] public float PatrollPointRangeMax = 30;

    [SerializeField] Animator dayAnimator;

    [SerializeField] Camera cam;

    public bool IsDead => isDead;
    private bool isDead;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
                if (instance == null)
                {
                    GameObject go = new GameObject("Player");
                    instance = go.AddComponent<Player>();
                }
            }
            return instance;
        }
    }

    public float PatrollPointRange => patrollPointRange;

    private static Player instance;
    private float patrollPointRange;

    public void Kill()
    {
        DayNightManager.Instance.GoToNoneState();
        cam.DOOrthoSize(1, 1.0f).SetEase(Ease.OutSine);
        StartCoroutine(DelayedKillAnim(.5f));
        isDead = true;
    }

    private IEnumerator DelayedKillAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        dayAnimator.SetTrigger("isDead");

        StartCoroutine(GoToLoseScreen(1.0f));
    }

    private IEnumerator GoToLoseScreen(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("LoseScreen");

    }

    private void Update()
    {
        float normal = Mathf.InverseLerp(0, 35, transform.position.magnitude);
        patrollPointRange = Mathf.Lerp(PatrollPointRangeMin, PatrollPointRangeMax, normal);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.DrawWireDisc(transform.position, Vector3.forward, patrollPointRange);
    }
    #endif
}