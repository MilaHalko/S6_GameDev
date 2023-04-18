using System;
using System.Collections.Generic;
using Core.Enums;
using Core.Enums.Item;
using StatsSystem;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class WeaponDescriptor : StatChangingItemDescriptor
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }

        public WeaponDescriptor(WeaponType weaponType, ItemId itemId, ItemType type, Sprite itemSprite, 
            ItemRarity itemRarity, float price, float level, List<StatModificator> stats) : 
            base(itemId, type, itemSprite, itemRarity, price, level, stats)
        {
            WeaponType = weaponType;
        }
    }
}