using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Properties
    [SerializeField] private SpriteRenderer sprite;
    #endregion

    #region Methods
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, GPSingleton.Instance.Planet.transform.position, Time.deltaTime * 5f);
    }
    #endregion

    #region Unity API
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLLISION");
        PlanetBehavior _planet = collision.GetComponent<PlanetBehavior>();
        if (_planet != null)
        {
            Debug.Log("COLLISION");
            _planet.InflictDamage(10);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Move();
    }
    #endregion
}
