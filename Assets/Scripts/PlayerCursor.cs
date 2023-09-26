using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    #region Properties
    Vector3 direction;
    [SerializeField] private Rigidbody2D rb2d;
    private Enemy target;
    #endregion

    #region Methods
    public void Shoot()
    {
        if (target != null)
        {
            target.Kill();
        }
    }
    #endregion

    #region Unity API
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy != null)
        {
            target = _enemy;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy _enemy = collision.GetComponent<Enemy>();
        if (_enemy == target)
        {
            target = null;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = direction * Time.deltaTime * 200;
    }
    #endregion
}
