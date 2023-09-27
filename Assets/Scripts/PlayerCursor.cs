using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    #region Properties
    Vector3 direction;
    [SerializeField] private Rigidbody rb;
    private Enemy target;
    public int cursorPoint;
    [Header("ACTION BUTTON")]
    public KeyCode actionButton;
    #endregion

    #region Methods
    public void Shoot()
    {
        if (target != null)
        {
            cursorPoint += target.data.scoreOnKill;
            target.Damage(1);
        }
    }
    #endregion

    #region Unity API
    private void OnTriggerEnter(Collider collision)
    {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy != null)
        {
            GPSingleton.Instance.SetColor(_enemy.mesh, EnemyData.Color.White);
            target = _enemy;

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy != null)
        {
            GPSingleton.Instance.SetColor(_enemy.mesh, _enemy.currentColor);
            if (_enemy == target)
            {
                target = null;
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(actionButton))
        {
            Shoot();
        }
    }

    public void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        Shoot();
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * Time.deltaTime * 200;
    }
    #endregion
}
