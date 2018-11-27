﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum PowerSlotTag { Slot1, Slot2, Slot3 }

public enum PowerTag
{
    INVALID,
    Energy1, Energy2,
    Immunity1, Immunity2,
    Damage1, Damage2,
    Poison1, Poison2
}

public interface ICheckActorPower
{
    bool HasPower(PowerTag tag);

    bool PowerIsActive(PowerTag tag);
}

public class PCPowers : MonoBehaviour, ICheckActorPower
{
    private Dictionary<PowerTag, string> nameDict;
    private Dictionary<PowerSlotTag, PowerTag> powerDict;

    public void GainPower(PowerSlotTag slot, PowerTag power)
    {
        powerDict[slot] = power;
    }

    public string GetPowerName(PowerSlotTag slot)
    {
        if (powerDict[slot] == PowerTag.INVALID)
        {
            return "";
        }
        return nameDict[powerDict[slot]];
    }

    public bool HasPower(PowerTag tag)
    {
        // TODO: Update after Unity 2018.3.
        bool isActive;

        return HasPower(tag, out isActive);
    }

    public bool PowerIsActive(PowerTag tag)
    {
        bool isActive;

        if (HasPower(tag, out isActive))
        {
            return isActive;
        }
        return false;
    }

    public bool SlotIsActive(PowerSlotTag slot)
    {
        int currentSlot = (int)slot + 1;
        int currentStress = gameObject.GetComponent<Stress>().CurrentStress;

        return currentSlot <= currentStress;
    }

    private void Awake()
    {
        powerDict = new Dictionary<PowerSlotTag, PowerTag>();
        nameDict = new Dictionary<PowerTag, string>
        {
            { PowerTag.Energy1, "Energy I" },
            { PowerTag.Energy2, "Energy II" },
            { PowerTag.Immunity1, "Immunity I" },
            { PowerTag.Immunity2, "Immunity II" },
            { PowerTag.Damage1, "Damage I" },
            { PowerTag.Damage2, "Damage II" },
            { PowerTag.Poison1, "Poison I" },
            { PowerTag.Poison2, "Poison II" }
        };
    }

    private bool HasPower(PowerTag tag, out bool isActive)
    {
        foreach (var slotPower in powerDict)
        {
            if (slotPower.Value == tag)
            {
                isActive = SlotIsActive(slotPower.Key);
                return true;
            }
        }

        isActive = false;
        return false;
    }

    private void Start()
    {
        foreach (PowerSlotTag tag in Enum.GetValues(typeof(PowerSlotTag)))
        {
            powerDict.Add(tag, PowerTag.INVALID);
        }
    }
}