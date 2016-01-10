using System.Collections.Generic;
using UnityEngine;

public class BattleDataAsset : ScriptableObject {

    public BattleData[] BattleData;
    public string assetName;

    public void AddBattleData (BattleData data) {
        List<BattleData> tmp;
        if (BattleData == null) {
            tmp = new List<BattleData>();
        } else {
            tmp = new List<BattleData>(BattleData);
        }
        tmp.Add(data);
        BattleData = tmp.ToArray();
    }
}
