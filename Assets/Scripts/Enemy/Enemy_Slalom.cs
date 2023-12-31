using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slalom : Enemy
{
    #region Properties
    private int factor = 1;
    private float timer = 0;
    private float timeLimit = .8f;
    #endregion

    #region Methods
    public override void Move()
    {
        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            factor *= -1;
            timer = 0;
            timeLimit *= .99f;
        }
        transform.RotateAround(target.transform.position, Vector3.forward, Time.deltaTime * 25 * factor);
        meshParent.forward = Vector3.RotateTowards(meshParent.forward, target.transform.position - transform.position, 5 * Time.deltaTime * factor, 0);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
    }
    #endregion
}
