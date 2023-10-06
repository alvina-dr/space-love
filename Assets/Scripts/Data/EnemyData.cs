using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    public enum Color
    {
        White = 0,
        Red = 1,
        Blue = 2
    }
    [Header("How often the enemy spawns (in seconds)")]
    public float spawnRate;
    [Header("When the enemy starts to spawn")]
    public float spawnTime;
    public Enemy enemyPrefab;
    public int maxHealth;
    public float speed;
    public int damage;
    public int scoreOnKill;

    [Header("SOUNDS")]
    public string deathSound;
    public string hitSound;
    public string shootSound;

    [Header("SHOOTING UNITS")]
    public float shootingDistance;
    public float projectileSpeed;
    public float reloadTime;

    [Header("TEMPORARY UNITS")]
    public float lifeDuration;
}
