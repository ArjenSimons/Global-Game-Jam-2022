using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class Player : MonoBehaviour
{
    [SerializeField] public float PatrollPointRangeMin = 13;
    [SerializeField] public float PatrollPointRangeMax = 30;

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