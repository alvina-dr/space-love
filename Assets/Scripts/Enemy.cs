using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Properties
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private EnemyData data;
    public PlanetBehavior target;
    #endregion

    #region Methods
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
    }

    private void Attack() //example suicide attack
    {
        target.InflictDamage(data.damage);
        Destroy(gameObject);
    }
    #endregion

    #region Unity API
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlanetBehavior _planet = collision.GetComponent<PlanetBehavior>();
        if (_planet != null)
        {
            Attack();

        }
    }

    private void Start()
    {
        target = GPSingleton.Instance.Planet;
    }

    private void Update()
    {
        Move();
    }
    #endregion
}
