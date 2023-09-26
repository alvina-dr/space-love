using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Orbitals : Enemy
{
    #region Methods
    public override void Move()
    {
        transform.RotateAround(target.transform.position, Vector3.forward, Time.deltaTime * 100);
        mesh.transform.forward = Vector3.RotateTowards(mesh.transform.forward, target.transform.position - transform.position, 10 * Time.deltaTime, 0);
        if (Vector3.Distance(transform.position, target.transform.position) > 3)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
    }
    #endregion
}
