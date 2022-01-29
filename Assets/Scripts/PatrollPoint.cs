using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PatrollPoint : MonoBehaviour
{
    public static List<PatrollPoint> Instances = new List<PatrollPoint>();
    private void OnEnable()
    {
        if (!Instances.Contains(this))
        {
            Instances.Add(this);
        }
    }
    private void OnDisable()
    {
        if (Instances.Contains(this))
        {
            Instances.Remove(this);
        }
    }

}
