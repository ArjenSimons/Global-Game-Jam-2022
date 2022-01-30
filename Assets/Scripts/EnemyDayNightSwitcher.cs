using UnityEngine;
using UnityEngine.AI;

public class EnemyDayNightSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject day;
    [SerializeField] private GameObject night;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private SpriteRenderer dayRenderer;
    [SerializeField] private SpriteRenderer nightRenderer;

    private bool isNight;

    private void Start()
    {
        DayNightManager.Instance.OnDayStart += OnDay;
        DayNightManager.Instance.OnNightStart += OnNight;
    }

    private void FixedUpdate()
    {
        //if (agent.velocity.x != 0) Debug.Log(agent.velocity);
        if (isNight && agent.velocity.x < 0) nightRenderer.flipX = false;
        if (isNight && agent.velocity.x > 0) nightRenderer.flipX = true;

        if (!isNight && agent.velocity.x < 0) dayRenderer.flipX = false;
        if (!isNight && agent.velocity.x > 0) dayRenderer.flipX = true;
    }

    private void OnDay()
    {
        day.SetActive(true);
        night.SetActive(false);

        isNight = false;
    }
    private void OnNight()
    {
        day.SetActive(false);
        night.SetActive(true);

        isNight = true;
    }

}
