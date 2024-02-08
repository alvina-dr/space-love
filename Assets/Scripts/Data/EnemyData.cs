using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    public enum Color
    {
        White = 0,
        Red = 1,
        Blue = 2
    }
    [EnumToggleButtons]
    public enum EnemyType
    {
        Normal = 0,
        Distance = 1,
        Temporary = 2
    }
    public EnemyType enemyType;
    [Header("How often the enemy spawns (in seconds)")]
    public float spawnRate;
    [Header("When the enemy starts to spawn")]
    public float spawnTime;
    public Enemy enemyPrefab;
    public int maxHealth;
    public float speed;
    public int damage;
    public int scoreOnKill;

    [BoxGroup("SOUNDS")]
    public string deathSound;
    [BoxGroup("SOUNDS")]
    public string hitSound;

    [ShowIf("enemyType", EnemyType.Distance)]
    [BoxGroup("SHOOTING")]
    public float shootingDistance;
    [ShowIf("enemyType", EnemyType.Distance)]
    [BoxGroup("SHOOTING")]
    public float projectileSpeed;
    [ShowIf("enemyType", EnemyType.Distance)]
    [BoxGroup("SHOOTING")]
    public float reloadTime;
    [ShowIf("enemyType", EnemyType.Distance)]
    [BoxGroup("SHOOTING")]
    public string shootSound;


    [ShowIf("enemyType", EnemyType.Temporary)]
    [BoxGroup("TEMPORARY UNIT")]
    public float lifeDuration;
}
