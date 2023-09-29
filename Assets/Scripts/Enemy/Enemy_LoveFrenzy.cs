using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_LoveFrenzy : Enemy
{
    float timerX;
    float timerY;
    float changeDirectionTimer = .1f;
    Vector3 randomDirection;
    bool checkX = true;
    bool checkY = true;
    [SerializeField] private Rigidbody rb;

    #region Methods
    public override void Move()
    {
        timerX += Time.deltaTime;
        timerY += Time.deltaTime;
        if (timerX >= changeDirectionTimer)
        {
            timerX = 0;
            checkX = true;
        }
        if (timerY >= changeDirectionTimer)
        {
            timerY = 0;
            checkY = true;
        }

        if (checkX)
        {
            if (transform.position.x > 8.36f || transform.position.x < -8.33f)
            {
                randomDirection = new Vector3(-randomDirection.x, randomDirection.y);
                checkX = false;
                timerX = 0;
            }
        }
        if (checkY) { 
            if (transform.position.y > 4.48f || transform.position.y < -4.46f)
            {
                randomDirection = new Vector3(randomDirection.x, -randomDirection.y);
                checkY = false;
                timerY = 0;
            }
        }
        rb.velocity = randomDirection * Time.deltaTime * data.speed * 100;
    }

    public void SetStartingDirection()
    {
        randomDirection = new Vector3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), 0).normalized;
        timerX = 0;
        timerY = 0;
    }

    public override void Attack()
    {
        //don't do anything
    }

    public override void Kill(PlayerCursor _cursor = null)
    {
        GPSingleton.Instance.SpecialAttack();
        base.Kill(_cursor);
    }

    public override void Damage(int _value, PlayerCursor _cursor)
    {
        base.Damage(_value, _cursor);
    }
    #endregion

    #region Unity API
    public override void Start()
    {
        base.Start();
        currentColor = EnemyData.Color.White;
        ChangeColor(currentColor);
        SetStartingDirection();
    }
    #endregion
}
