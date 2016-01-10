using UnityEngine;
using System.Collections;

[System.Serializable]
public class BattleData {

    public int tickCount;
    public float startTime;
    public float endTime;
    public BattleSystem.TentativeDamage[] damageList;

    public BattleData (int tickCount, float startTime, float endTime, BattleSystem.TentativeDamage[] damageList) {
        this.tickCount = tickCount;
        this.startTime = startTime;
        this.endTime = endTime;
        this.damageList = damageList;
    }

}
