using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public enum ItemType
    {
        Dash,
        Shield,
        Heal,
        Magnet,
        Coin
    }

    public ItemType type;

    public bool isDashing;
}
