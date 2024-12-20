using DG.Tweening;
using FMODUnity;
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

    [Header("STATS")]
    public EnemyData data;
    [SerializeField] private int currentHealth;
    [SerializeField] public EnemyData.Color currentColor;

    [Header("FX")]
    public VisualEffect visualEffect;
    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;
    public VisualEffect loadingEffect;
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
                    _cursor.targetList.Remove(this);
                    Kill(_cursor);
                } else
                {
                    Instantiate(DataHolder.Instance.GeneralData.explosionDeathEffect).transform.position = transform.position;
                }
            });
        });
    }

    public virtual void Kill(PlayerCursor _cursor = null)
    {
        if (_cursor != null)
        {
            _cursor.GainPoints(data.scoreOnKill);
            ScorePopper _scorePopper = Instantiate(DataHolder.Instance.GeneralData.scorePopperFX, GPCtrl.Instance.UICtrl.transform);
            _scorePopper.SetupScorePopper(data.scoreOnKill, transform.position);
        }
        if (meshParent == null) return;
        if (GPCtrl.Instance.spawnerMode == GPCtrl.SpawnerMode.Game)
        {
            var audioEvent = RuntimeManager.CreateInstance(data.deathSound);
            audioEvent.start();
            if (GPCtrl.Instance.PlayerBlue.targetList.Contains(this))
            {
                GPCtrl.Instance.PlayerBlue.targetList.Remove(this);
                GPCtrl.Instance.PlayerBlue.StopCrossAnimation();
            }
            if (GPCtrl.Instance.PlayerRed.targetList.Contains(this))
            {
                GPCtrl.Instance.PlayerRed.targetList.Remove(this);
                GPCtrl.Instance.PlayerRed.StopCrossAnimation();
            }
        }
        meshParent.transform.DOScale(0f, .1f).OnComplete(() => {
            Instantiate(DataHolder.Instance.GeneralData.explosionDeathEffect).transform.position = transform.position;
            Destroy(gameObject);
        });
    }

    public void ChangeColor(EnemyData.Color _color, bool _transition = false)
    {
        if (mesh != null && !_transition) GPCtrl.Instance.SetColor(mesh, _color);
        if (visualEffect != null) GPCtrl.Instance.SetVFX(visualEffect, _color);
        if (leftTrail != null) GPCtrl.Instance.SetVFX(leftTrail, currentColor);
        if (rightTrail != null) GPCtrl.Instance.SetVFX(rightTrail, currentColor);
        if (loadingEffect != null) GPCtrl.Instance.SetVFX(loadingEffect, _color);
    }

    public void SetOutline(bool _value)
    {
        if (mesh == null) return;
        mesh.materials[1].SetFloat("_Is_Visible", _value ? 1.0f : 0);
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
        currentColor = (EnemyData.Color) Random.Range(1, 3);
        if (GPCtrl.Instance.loveFrenzy) ChangeColor(EnemyData.Color.White);
        else ChangeColor(currentColor);
        SetOutline(false);
        healthBar.SetSliderValue(currentHealth, data.maxHealth);
    }

    private void Update()
    {
        Move();
    }
    #endregion
}
