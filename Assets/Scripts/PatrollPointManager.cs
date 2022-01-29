using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class PatrollPointManager : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (PatrollPoint p in PatrollPoint.Instances)
        {
            Handles.DrawLine(transform.position, p.transform.position);
        }
    }
#endif
}
