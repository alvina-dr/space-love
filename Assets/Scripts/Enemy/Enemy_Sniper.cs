using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sniper : Enemy
{
    float timer = 0;
    [SerializeField] private Projectile projectilePrefab;

    #region Methods
    public override void Move()
    {
        meshParent.forward = Vector3.RotateTowards(meshParent.forward, target.transform.position - transform.position, 10 * Time.deltaTime, 0);
        if (Vector3.Distance(transform.position, target.transform.position) > data.shootingDistance)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
        else
        {
            timer += Time.deltaTime;
            if (timer >= data.reloadTime)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        Projectile _projectile = Instantiate(projectilePrefab);
        _projectile.SetupProjectile(data, currentColor);
        _projectile.transform.position = transform.position;
        timer = 0;
    }
    #endregion
}