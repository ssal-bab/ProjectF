using System;
using System.Collections.Generic;
using H00N.DataTables;
using H00N.Stats;
using ProjectF.DataTables;
using ProjectF.Farms;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Farm/FarmerIncreaseStat")]
public class FarmerIncreaseStatSO : DataTableSO<FarmerIncreaseStatTable, FarmerIncreaseStatTableRow>
{
    private Dictionary<EFarmerStatType, float> statDictionary;
    public float this[EFarmerStatType indexer]
    {
        get
        {
            if (statDictionary.ContainsKey(indexer) == false)
            {
                Debug.LogWarning("Stat of Given Type is Doesn't Existed");
                return float.NaN;
            }

            return statDictionary[indexer];
        }
    }

    protected override void OnTableInitialized()
    {
        base.OnTableInitialized();

        statDictionary.Add(EFarmerStatType.MoveSpeed, TableRow.moveSpeed);
        statDictionary.Add(EFarmerStatType.Health, TableRow.health);
        statDictionary.Add(EFarmerStatType.FarmingSkill, TableRow.farmingSkill);
        statDictionary.Add(EFarmerStatType.AdventureSkill, TableRow.adventureSkill);
    }
}

