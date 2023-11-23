using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item {
    public int id;
    public string name;
    public Sprite sprite;
    public int maxStack;
    public const int max_unstackable = 1;
    public const int max_stackSmall = 50;
    public const int max_stackLarge = 99;

    public override string ToString() {
        return name;
    }
}
