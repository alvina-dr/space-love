using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Cameleon : Enemy
{
    #region Methods
    public override void Move()
    {
        meshParent.forward = Vector3.RotateTowards(meshParent.forward, target.transform.position - transform.position, 10 * Time.deltaTime, 0);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * data.speed);
    }
    public override void Damage(int _value, PlayerCursor _cursor)
    {
        base.Damage(_value, _cursor);
        switch(currentColor)
        {
            case EnemyData.Color.Blue:
                currentColor = EnemyData.Color.Red;
                break;
            case EnemyData.Color.Red:
                currentColor = EnemyData.Color.Blue;
                break;
        }
        ChangeColor(currentColor);
    }
    #endregion
}
