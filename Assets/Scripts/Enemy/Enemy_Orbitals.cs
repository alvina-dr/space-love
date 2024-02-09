using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class Enemy_Orbitals : Enemy
{
    float timer = 0;
    [SerializeField] private Projectile projectilePrefab;

    #region Methods
    public override void Move()
    {
        transform.RotateAround(target.transform.position, Vector3.back, Time.deltaTime * 50);
            meshParent.forward = Vector3.RotateTowards(meshParent.forward, target.transform.position - transform.position, 10 * Time.deltaTime, 0);
        if (transform.position.x <= 0.001) mesh.transform.localRotation = Quaternion.Euler(180, -90, 0);
        else mesh.transform.localRotation = Quaternion.Euler(0, -90, 0);
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
        var audioEvent = RuntimeManager.CreateInstance(data.shootSound);
        audioEvent.start();
        timer = 0;
    }
    #endregion
}
