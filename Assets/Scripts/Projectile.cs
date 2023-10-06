using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Properties
    private Transform target;
    private float speed;
    private int damage;
    [SerializeField] private MeshRenderer meshTop;
    [SerializeField] private MeshRenderer meshDown;
    #endregion

    #region Methods
    public void SetupProjectile(EnemyData _data, EnemyData.Color _color)
    {
        target = GPCtrl.Instance.Planet.transform;
        speed = _data.projectileSpeed;
        damage = _data.damage;
        GPCtrl.Instance.SetShaderColor(meshTop, _color);
        GPCtrl.Instance.SetShaderColor(meshDown, _color);
    }
    #endregion

    #region Unity API
    void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed); 
        transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 5 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        PlanetBehavior _planet = collision.GetComponent<PlanetBehavior>();
        if (_planet != null)
        {
            _planet.InflictDamage(damage);
            Destroy(gameObject);
        }
    }
    #endregion
}
