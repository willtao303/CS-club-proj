using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * Basic stat class for modifiable stats
 * Withercraft303 / Yuki303
 * https://github.com/withercraft303
 * Oct 25, 2023
 */

[Serializable]
public class Stat {
    // the raw value of the stat
    [SerializeField]
    private float raw_value;
    // the processed value of the stat
    private float value;
    // the modifiers on this stat
    [SerializeField]
    private HashSet<StatMod> modifiers = new HashSet<StatMod>();

    public Stat(float val){
        raw_value = val;
        value = val;
    }

    // updates modifier countdowns
    public void Update(){ 
        HashSet<StatMod> remList = null;
        foreach (StatMod modifier in modifiers) {
            modifier.Update();
            if (!modifier.IsActive()) {
                if (remList == null){
                    remList = new HashSet<StatMod>();
                }
                remList.Add(modifier);
            }
        }
        UpdateValue();
        if (remList != null){
            RemModifier(remList);
        }
    }

    // remove modifiers
    public void RemModifier(StatMod modifier){
        modifiers.Remove(modifier);
        UpdateValue();
    }

    public void RemModifier(HashSet<StatMod> modifier) {
        foreach(StatMod mod in modifier){
            modifiers.Remove(mod);
        }
        //modifiers.ExceptWith(modifier);
        UpdateValue();
    }

    // add modifier
    public void AddModifier(StatMod modifier){
        modifiers.Add(modifier);
        UpdateValue();
    }

    // updates the processed value
    private void UpdateValue() {
        // three modifier types
        float raw_multi = 1;
        float multi = 1;
        float incr = 0;

        foreach (StatMod modifier in modifiers){
            if (modifier.type == Type.MULTIPLER) {
                multi += modifier.val;
            } else if (modifier.type == Type.INCREMENT) {
                incr += modifier.val;
            } else if (modifier.type == Type.RAW_MULTIPLIER) {
                raw_multi *= modifier.val;
            } 
        }
        
        value = raw_value*raw_multi*multi + incr;
        // clamps value at 0
        if (value < 0){
            value = 0;
        }
    }

    // getters and setters
    public float Value(){
        return value;
    }

    public float RawValue(){
        return raw_value;
    }

    public void SetRawValue(int new_value){
        raw_value = new_value;
    }

}

[Serializable]
public class StatMod {
    [SerializeField]
    public float val;
    [SerializeField]
    public Type type;
    [SerializeField]
    private float duration;

    public StatMod(Type buff_type,  float value, float _duration = Mathf.Infinity) {
        type = buff_type;
        val = value;
        duration = _duration;
    }

    public void Update(){
        duration -= Time.deltaTime;
    }

    public bool IsActive(){
        return duration < 0;
    }
}

[Serializable]
public enum Type {
    // eg 2x speed and 1.5x speed gives a 3x speed boost
    // stackable multipliers
    RAW_MULTIPLIER,

    // eg -30% speed and + 40% speed gives a 10% speed boost
    // nonstackable, incremental multipliers
    MULTIPLER,

    // eg +2 atk
    // simple increment to stat
    INCREMENT
}