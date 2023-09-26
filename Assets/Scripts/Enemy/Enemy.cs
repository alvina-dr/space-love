using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Properties
    [Header("COMPONENTS")]
    public MeshRenderer mesh;
    [HideInInspector] public PlanetBehavior target;

    [Header("STATS")]
    public EnemyData data;
    [SerializeField] private int currentHealth;
    [SerializeField] public EnemyData.Color currentColor;
    #endregion

    #region Methods
    public virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
        mesh.transform.forward = Vector3.RotateTowards(mesh.transform.forward, target.transform.position - transform.position, 10 * Time.deltaTime, 0);
    }

    public virtual void Attack() //example suicide attack
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
        currentColor = data.startColor;
        GPSingleton.Instance.SetColor(mesh, currentColor);
    }

    private void Update()
    {
        Move();
    }
    #endregion
}
