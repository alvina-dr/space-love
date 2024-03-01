using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using DG.Tweening;

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
        if (_cursor.targetList.Contains(this)) _cursor.targetList.Remove(this);
        switch (currentColor)
        {
            case EnemyData.Color.Blue:
                currentColor = EnemyData.Color.Red;
                break;
            case EnemyData.Color.Red:
                currentColor = EnemyData.Color.Blue;
                break;
        }
        ChangeColor(currentColor, true);
        if (currentColor == EnemyData.Color.Red) mesh.material.SetFloat("_Spawns_Red", 1);
        else mesh.material.SetFloat("_Spawns_Red", 0);
        mesh.material.SetFloat("_Is_Magenta", 0);
        SetOutline(false);
        mesh.material.DOFloat(2.0f, "_Transition_Strength", .3f);
        if (GPCtrl.Instance.spawnerMode == GPCtrl.SpawnerMode.Game)
        {
            var audioEvent = RuntimeManager.CreateInstance(data.hitSound);
            audioEvent.start();
        }
    }
    #endregion
}
