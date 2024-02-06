using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralData", menuName = "ScriptableObjects/GeneralData", order = 1)]
public class GeneralData : ScriptableObject
{
    [Header("DEBUG")]
    public bool computerMode;
    public bool externalDevice;

    [Header("SETUP")]
    public int scoreboardSize;
    public int planetMaxHealth;
    public int cursorSpeed;
    public List<float> gameStage = new List<float>();
    public float timeRateReduction;

    [Header("FX")]
    public GameObject explosionDeathEffect;

    [Header("GAMEPLAY")]
    public float loveFrenzyDuration;
}
