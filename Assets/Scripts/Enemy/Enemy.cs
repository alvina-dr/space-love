using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    #region Properties
    [Header("COMPONENTS")]
    public Transform meshParent;
    public MeshRenderer mesh;
    public ValueSlider healthBar;
    [HideInInspector] public PlanetBehavior target;
    public VisualEffect visualEffect;

    [Header("STATS")]
    public EnemyData data;
    [SerializeField] private int currentHealth;
    [SerializeField] public EnemyData.Color currentColor;
    #endregion

    #region Methods
    public virtual void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
        meshParent.forward = Vector3.RotateTowards(meshParent.forward, target.transform.position - transform.position, 10 * Time.deltaTime, 0);
    }

    public virtual void Attack() //example suicide attack
    {
        target.InflictDamage(data.damage);
        Destroy(gameObject);
    }

    public virtual void Damage(int _value)
    {
        currentHealth -= _value;
        healthBar.SetSliderValue(currentHealth, data.maxHealth);
        if (currentHealth <= 0)
        {
            Kill();
        }
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
        currentColor = (EnemyData.Color)Random.Range(1, 3);
        GPSingleton.Instance.SetColor(mesh, currentColor);
        healthBar.SetSliderValue(currentHealth, data.maxHealth);
        if (visualEffect != null) GPSingleton.Instance.SetVFX(visualEffect, currentColor);
    }

    private void Update()
    {
        Move();
    }
    #endregion
}
