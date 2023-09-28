using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Orbitals : Enemy
{
    #region Methods
    public override void Move()
    {
        transform.RotateAround(target.transform.position, Vector3.back, Time.deltaTime * 50);
        meshParent.forward = Vector3.RotateTowards(meshParent.forward, target.transform.position - transform.position, 10 * Time.deltaTime, 0);
        if (Vector3.Distance(transform.position, target.transform.position) > 3)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
    }
    #endregion
}
