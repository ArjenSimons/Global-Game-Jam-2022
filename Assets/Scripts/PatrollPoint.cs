using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PatrollPoint : MonoBehaviour
{
    [SerializeField] private float enabledDistanceToPlayer = 10;

    public static List<PatrollPoint> Instances = new List<PatrollPoint>();
    public static List<PatrollPoint> AvailableParollPoints = new List<PatrollPoint>();

    private bool inPlayerRange;
    private Player player;

    private void Start()
    {
        player = Player.Instance;
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (Vector3.SqrMagnitude(transform.position - player.transform.position) < player.PatrollPointRange * player.PatrollPointRange)
            {
                if (!inPlayerRange)
                {
                    AvailableParollPoints.Add(this);
                    inPlayerRange = true;
                }
            }
            else if (inPlayerRange)
            {
                AvailableParollPoints.Remove(this);
                inPlayerRange = false;
            }
        }
    }


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
