﻿using Fungus.Actor;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor
{
    public enum PowerSlotTag { Slot1, Slot2, Slot3 }

    public enum PowerTag
    {
        INVALID,

        DefEnergy1, DefEnergy2,
        DefInfection1, DefInfection2,
        DefHP1, DefHP2,

        AttEnergy1, AttInfection1, AttDamage1
    }
}

namespace Fungus.GameSystem.ObjectManager
{
    public class PowerData : MonoBehaviour
    {
        private Dictionary<PowerTag, string> powerNames;

        public string GetPowerName(PowerTag tag)
        {
            return powerNames[tag];
        }

        private void Awake()
        {
            powerNames = new Dictionary<PowerTag, string>
            {
                { PowerTag.DefEnergy1, "Vigor" },
                { PowerTag.DefEnergy2, "Adrenaline" },
                { PowerTag.DefInfection1, "Immunity" },
                { PowerTag.DefInfection2, "Fast Heal" },
                { PowerTag.DefHP1, "First Aid" },
                { PowerTag.DefHP2, "Reaper" },

                { PowerTag.AttEnergy1, "Siphon" },
                { PowerTag.AttInfection1, "Plague" },
                { PowerTag.AttDamage1, "Bleed" }
            };
        }
    }
}
