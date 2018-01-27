using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSwitch : SwitchBehaviour
{
    public float interval = 2f;
    private float timeUntilNextAction;

    private void Start()
    {
        timeUntilNextAction = interval;
    }

    private void Update()
    {
        timeUntilNextAction -= Time.deltaTime;
        if (timeUntilNextAction < 0)
        {
            timeUntilNextAction = interval;
            Target.GetComponent<GearBehaviour>().Action();
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
