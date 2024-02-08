using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

    [BoxGroup("FX")]
    public GameObject explosionDeathEffect;
    [BoxGroup("FX")]
    public FrenzyFX frenzyFX;

    [Header("GAMEPLAY")]
    public float loveFrenzyDuration;
}
