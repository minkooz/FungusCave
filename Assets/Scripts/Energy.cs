﻿using Fungus.Actor.ObjectManager;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
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
        private ActorData actorData;
        private DungeonBoard board;
        private Direction direction;
        private EnergyData energyData;
        private UIMessage message;
        private WizardMode wizard;

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

        public int GetAttackEnergy(int[] start, int[] end)
        {
            int baseEnergy = GetBaseEnergy(start, end);
            int attack = actorData.GetIntData(
                GetComponent<MetaInfo>().SubTag, DataTag.EnergyAttack);
            // TODO: Implement fog.
            int fog = 0;

            int final = baseEnergy + attack + fog;

            if (wizard.PrintEnergyCost && GetComponent<MetaInfo>().IsPC)
            {
                message.StoreText("Attack energy: " + final);
            }

            return final;
        }

        public int GetMoveEnergy(int[] start, int[] end)
        {
            int baseEnergy = GetBaseEnergy(start, end);
            int move = actorData.GetIntData(
                GetComponent<MetaInfo>().SubTag, DataTag.EnergyMove);
            int pool = board.CheckBlock(SubObjectTag.Pool, start)
                ? energyData.DrainMedium : 0;

            int final = baseEnergy + move + pool;

            if (wizard.PrintEnergyCost && GetComponent<MetaInfo>().IsPC)
            {
                message.StoreText("Move energy: " + final);
            }

            return final;
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

        private int GetBaseEnergy(int[] start, int[] end)
        {
            int slow = GetComponent<Infection>().HasInfection(InfectionTag.Slow)
                ? energyData.DrainHigh : 0;
            int diagonal
                = direction.CheckDirection(RelativePosition.Diagonal, start, end)
                ? energyData.DrainLow : 0;

            int final = slow + diagonal;
            return final;
        }

        private void Start()
        {
            energyData = FindObjects.GameLogic.GetComponent<EnergyData>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            direction = FindObjects.GameLogic.GetComponent<Direction>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            wizard = FindObjects.GameLogic.GetComponent<WizardMode>();

            CurrentEnergy = energyData.ActionThreshold;
        }
    }
}
