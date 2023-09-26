using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    public enum Color
    {
        White = 0,
        Red = 1,
        Blue = 2
    }
    public Enemy enemyPrefab;
    public Color startColor;
    public int maxHealth;
    public float speed;
    public int damage;
}
