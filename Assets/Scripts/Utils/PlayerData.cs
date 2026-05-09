using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    const int InitialLives = 5;
    const int InitialLumiberries = 0;

    public int CurrentLives;
    public int CurrentLumiberries;
    public int TotalLumiberries;

    public List<string> collectedBeaconCrystals = new List<string>();

    public bool IsBeaconCrystalCollected(string levelName) => collectedBeaconCrystals.Contains(levelName);

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        CurrentLives = InitialLives;
        CurrentLumiberries = InitialLumiberries;
        TotalLumiberries = InitialLumiberries;
        collectedBeaconCrystals.Clear();
    }
}