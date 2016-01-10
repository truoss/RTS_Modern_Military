using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem {

    //Features:
    //Aufruf durch BattleTickRate (BTR) in ServerGameLogic
    // 1. Pro Einheit auf dem Spielfeld werden alle Einheiten in der Reichweite gesucht
    // 2. Entscheidung welche Einheit in Reichweite angegriffen wird????????
    // 3. Schaden auf Einheit in Reichweite ermittelt (Ausführung am Ende der Schadensermittlung)
    // 4. Schaden an alle Einheiten vergeben und ggf. löschen (Wenn 0 HP/SP)
    // 5. Schaden mit allen Clients syncen
    // 6. BattleTick Log(Timestamp, Dauer, TentitiveDamageList)
    // 7. Ergebnisanzeige der gesamten Schlacht

    List<TentativeDamage> tentativeDamageList = new List<TentativeDamage>();
    BattleDataAsset battleDataAsset;

    int currentTickIndex;

    public BattleSystem () {
        currentTickIndex = 0;

#if UNITY_EDITOR        
        battleDataAsset = ScriptableObject.CreateInstance<BattleDataAsset>();
        UnityEditor.AssetDatabase.CreateAsset(battleDataAsset, "Assets/Resources/BattleDataAsset.asset");
        UnityEditor.AssetDatabase.SaveAssets();
        
        
        battleDataAsset = Resources.Load<BattleDataAsset>("BattleDataAsset");
        battleDataAsset.name = DateTime.Now.ToShortDateString();        
#endif
    }

    public void BattleTick (Unit[] allUnits) {
        float startTime = Time.time;

        tentativeDamageList.Clear();

        for (int i = 0; i < allUnits.Length; i++) {
            // 1.
            Unit[] _unitsInRange = GetUnitsInRange(allUnits[i]);
            if (_unitsInRange == null)
                break;
            // 2.
            Unit _target = GetTargetFromUnitsInRange(_unitsInRange);
            if (_target == null)
                break;
            // 3.
            tentativeDamageList.Add(CalculateTentativeDamage(_target, allUnits[i]));
        }
        // 4. Schadensauswertung
        if (tentativeDamageList.Count > 0) {
            List<Unit> unitsToDestroy = new List<Unit>();
            for (int i = 0; i < tentativeDamageList.Count; i++) {
                tentativeDamageList[i].Target.curSoftpoints -= tentativeDamageList[i].SP;
                tentativeDamageList[i].Target.curHardpoints -= tentativeDamageList[i].HP;
                if (tentativeDamageList[i].Target.curSoftpoints <= 0 && tentativeDamageList[i].Target.curHardpoints <= 0) {
                    unitsToDestroy.Add(tentativeDamageList[i].Target);
                }
            }
            DestroyUnits(unitsToDestroy.ToArray());
        }
        float endTime = Time.time;
        // 5.
        // 6. BattleTick Log
        var data = new BattleData(currentTickIndex++, startTime, endTime, tentativeDamageList.ToArray());
        DebugBattleData(data);
        if(battleDataAsset)
            battleDataAsset.AddBattleData(data);

        //Debug.LogWarning("Tick done: " + currentTickIndex);
    }

    void DebugBattleData (BattleData data) {
        string result = "";
        result += "Tick done: " + data.tickCount + "\n";
        result += data.startTime + " " + data.endTime + "\n";
        if (data.damageList.Length > 0) {
            for (int i = 0; i < data.damageList.Length; i++) {
                result += data.damageList[i].Target.UnitID + ", SP: " + data.damageList[i].SP + " HP:" + data.damageList[i].HP + "\n";
            }
            Debug.LogWarning(result);
        }
        
    }

    Unit[] GetUnitsInRange (Unit Unit) {
        //TODO: Range > curfield
        Field field = Unit.CurrentField.GetComponent<Field>();
        List<Unit> unitsInRange = new List<Unit>();
        if (field == null)
            return null;
        for (int i = 0; i < field.Units.Count; i++) {
            if (field.Units[i].Side != Unit.Side) {
                unitsInRange.Add(field.Units[i]);
            }
        }
        foreach (var item in unitsInRange) {
            Debug.LogWarning("unitsInRange: " + item, item);
        }
        
        return unitsInRange.ToArray();
    }

    Unit GetTargetFromUnitsInRange (Unit[] UnitsInRange) {
        //TODO: ALL OF IT
        if (UnitsInRange != null && UnitsInRange.Length > 0) {
            Debug.LogWarning("target: " + UnitsInRange[0], UnitsInRange[0]);            
            return UnitsInRange[0];        
        }
        return null;
    }

    [System.Serializable]
    public class TentativeDamage {
        public float SP;
        public float HP;
        public Unit Target;
        public Unit Origin;
        //TODO Ammunition
    }

    TentativeDamage CalculateTentativeDamage (Unit Target, Unit Origin) {
        TentativeDamage result = new TentativeDamage();
        result.Origin = Origin;
        result.Target = Target;
        FieldData targetData = FieldDataAsset.I.GetFieldDataFromFieldType(Target.CurrentField.GetComponent<Field>().fieldType);
        FieldAttribute targetFieldAttribute = targetData.GetFieldAttributeFromMobilityType(Target.UnitStats.mobilityType);
        float currentVisibleSP = GetTargetVisiblity(Target).curSoftpointVis * (targetFieldAttribute.cover * Convert.ToInt32(!Target.isMoving));
        result.SP = Mathf.Ceil(Mathf.Min(currentVisibleSP, CalculateCurrentFirepower(Origin).curFirepowerSoft));
        if (float.IsNaN(result.SP))
            result.SP = 0;
        float currentVisibleHP = GetTargetVisiblity(Target).curHardpointVis * (targetFieldAttribute.cover * Convert.ToInt32(!Target.isMoving));
        result.HP = Mathf.Ceil(Mathf.Min(currentVisibleHP, CalculateCurrentFirepower(Origin).curFirepowerHard));
        if (float.IsNaN(result.HP))
            result.HP = 0;
        return result;
    }

    struct CurrentFirepower {
        public float curFirepowerSoft;
        public float curFirepowerHard;
        public Unit Origin;
    }

    CurrentFirepower CalculateCurrentFirepower (Unit Origin) {
        CurrentFirepower result = new CurrentFirepower();
        result.curFirepowerSoft = Origin.UnitStats.Attributes.FirepowerSoft / (Origin.UnitStats.Attributes.maxSoftpoint / Origin.curSoftpoints);
        result.curFirepowerHard = Origin.UnitStats.Attributes.FirepowerHard / (Origin.UnitStats.Attributes.maxHardpoint / Origin.curHardpoints);
        result.Origin = Origin;
                
        return result;
    }

    struct TargetVisiblity {
        public float curSoftpointVis;
        public float curHardpointVis;
        public Unit Target;
    }

    TargetVisiblity GetTargetVisiblity (Unit Target) {
        TargetVisiblity result = new TargetVisiblity();
        FieldData data = FieldDataAsset.I.GetFieldDataFromFieldType(Target.CurrentField.GetComponent<Field>().fieldType);
        FieldAttribute fieldAttribute = data.GetFieldAttributeFromMobilityType(Target.UnitStats.mobilityType);
        result.curSoftpointVis = Target.curSoftpoints + (Target.curSoftpoints * fieldAttribute.visibility * Convert.ToInt32(Target.isMoving));
        result.curHardpointVis = Target.curHardpoints + (Target.curHardpoints * fieldAttribute.visibility * Convert.ToInt32(Target.isMoving));
        result.Target = Target;

        return result;
    }

    void DestroyUnits (Unit[] units) {
        int[] unitIDs = new int[units.Length];
        for (int i = 0; i < unitIDs.Length; i++) {
            unitIDs[i] = units[i].UnitID;
        }
        UnitManager.I.CmdDestroyUnitsOnClients(unitIDs);
    }

}
