﻿using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using System;
using System.Text;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IEnergy
    {
        int Drain { get; }
        int RestoreTurn { get; }
    }

    public class Energy : MonoBehaviour, ITurnCounter
    {
        private EnergyData energyData;

        public int CurrentEnergy { get; private set; }

        public void Count()
        {
            throw new NotImplementedException();
        }

        public void GainEnergy(int energy)
        {
            CurrentEnergy += energy;
            CurrentEnergy = Math.Min(energyData.Maximum, CurrentEnergy);
        }

        public bool HasEnoughEnergy()
        {
            return CurrentEnergy >= energyData.ActionThreshold;
        }

        public void LoseEnergy(int energy)
        {
            CurrentEnergy -= energy;
            CurrentEnergy = Math.Max(energyData.Minimum, CurrentEnergy);
        }

        public void PrintEnergy()
        {
            StringBuilder printText = new StringBuilder();
            int[] testPosition
                = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                .Convert(transform.position);

            printText.Remove(0, printText.Length);
            printText.Append("[");
            printText.Append(testPosition[0].ToString());
            printText.Append(",");
            printText.Append(testPosition[1].ToString());
            printText.Append("] ");
            printText.Append("Energy: ");
            printText.Append(CurrentEnergy.ToString());

            FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
                printText.ToString());
        }

        public void Trigger()
        {
            // An actor gains 1000 energy at the start of every turn under any
            // cirsumstances.
            GainEnergy(energyData.Restore);
            // An actor might gain bonus energy from PC's power or NPC's innate
            // ability.
            GainEnergy(GetComponent<IEnergy>().RestoreTurn);
        }

        private void Start()
        {
            energyData = FindObjects.GameLogic.GetComponent<EnergyData>();
            CurrentEnergy = energyData.ActionThreshold;
        }
    }
}
