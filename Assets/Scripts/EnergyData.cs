﻿using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public class EnergyData : MonoBehaviour
    {
        public int ActionThreshold { get; private set; }
        public int Attack { get; private set; }
        public int DrainHigh { get; private set; }
        public int DrainLow { get; private set; }
        public int DrainMedium { get; private set; }
        public int Maximum { get; private set; }
        public int Minimum { get; private set; }
        public int Move { get; private set; }
        public int Restore { get; private set; }

        private void Awake()
        {
            DrainLow = 200;
            DrainMedium = 400;
            DrainHigh = 600;

            ActionThreshold = 3000;
            Maximum = 4000;
            Minimum = 0;

            Attack = 1400;
            Move = 1000;
            Restore = 1000;
        }
    }
}
