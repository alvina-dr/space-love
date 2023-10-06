using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
using FMODUnity;
public class Enemy : MonoBehaviour
{
    #region Properties
    [Header("COMPONENTS")]
    public Transform meshParent;
    public MeshRenderer mesh;
    public ValueSlider healthBar;
    [HideInInspector] public PlanetBehavior target;

    [Header("STATS")]
    public EnemyData data;
    [SerializeField] private int currentHealth;
    [SerializeField] public EnemyData.Color currentColor;

    [Header("FX")]
    public VisualEffect visualEffect;
    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;

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
        Kill();
    }

    public virtual void Damage(int _value, PlayerCursor _cursor)
    {
        if (currentHealth <= 0) return;
        currentHealth -= _value;
        healthBar.SetSliderValue(currentHealth, data.maxHealth);
        if (meshParent == null) return;
        meshParent.transform.DOScale(1.1f, .1f).OnComplete( () =>
        {
            meshParent.transform.DOScale(1f, .1f).OnComplete(() =>
            {
                if (currentHealth <= 0)
                {
                    Kill(_cursor);
                    _cursor.targetList.Remove(this);
                }
            });
        });
    }

    public virtual void Kill(PlayerCursor _cursor = null)
    {
        Instantiate(DataHolder.Instance.GeneralData.explosionDeathEffect).transform.position = transform.position;
        if (_cursor != null) _cursor.GainPoints(data.scoreOnKill);
        if (meshParent == null) return;
        if (GPCtrl.Instance.spawnerMode == GPCtrl.SpawnerMode.Game)
        {
            var audioEvent = RuntimeManager.CreateInstance(data.deathSound);
            audioEvent.start();
        }
        meshParent.transform.DOScale(0f, .1f).OnComplete(() => { 
            Destroy(gameObject);
        });
    }

    public void ChangeColor(EnemyData.Color _color)
    {
        if (mesh != null) GPCtrl.Instance.SetColor(mesh, _color);
        if (visualEffect != null) GPCtrl.Instance.SetVFX(visualEffect, _color);
        if (leftTrail != null) GPCtrl.Instance.SetVFX(leftTrail, currentColor);
        if (rightTrail != null) GPCtrl.Instance.SetVFX(rightTrail, currentColor);
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
    public virtual void Start()
    {
        target = GPCtrl.Instance.Planet;
        currentHealth = data.maxHealth;
        currentColor = (EnemyData.Color)Random.Range(1, 3);
        if (GPCtrl.Instance.loveFrenzy) ChangeColor(EnemyData.Color.White);
        else ChangeColor(currentColor);
        healthBar.SetSliderValue(currentHealth, data.maxHealth);
    }

    private void Update()
    {
        Move();
    }
    #endregion
}
