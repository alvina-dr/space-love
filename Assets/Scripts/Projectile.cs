using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Properties
    private Transform target;
    private float speed;
    private int damage;
    [SerializeField] private MeshRenderer mesh;
    #endregion

    #region Methods
    public void SetupProjectile(EnemyData _data, EnemyData.Color _color)
    {
        target = GPSingleton.Instance.Planet.transform;
        speed = _data.projectileSpeed;
        damage = _data.damage;
        GPSingleton.Instance.SetColor(mesh, _color);
    }
    #endregion

    #region Unity API
    void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed); 
        transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 5 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlanetBehavior _planet = other.GetComponent<PlanetBehavior>();
        if (_planet != null)
        {
            _planet.InflictDamage(damage);
            Destroy(gameObject);
        }
    }
    #endregion
}