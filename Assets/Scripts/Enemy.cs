using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Properties
    [Header("COMPONENTS")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private PlanetBehavior target;

    [Header("STATS")]
    [SerializeField] private EnemyData data;
    [SerializeField] private int currentHealth;
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

    public void Kill()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Unity API
    private void OnTriggerEnter(Collider collision)
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
        currentHealth = data.maxHealth;
        GPSingleton.Instance.SetColor(mesh, data.startColor);
    }

    private void Update()
    {
        Move();
    }
    #endregion
}
