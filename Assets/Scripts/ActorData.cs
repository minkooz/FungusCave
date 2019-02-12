﻿using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public enum DataTag
    {
        HP, Stress, Damage, DropPotion,
        MaxInfections, InfectionAttack, InfectionDefend
    }

    public class ActorData : MonoBehaviour
    {
        private Dictionary<DataTag, Dictionary<SubObjectTag, int>> intData;
        private int invalidData;

        public string Version { get; private set; }

        public int GetIntData(SubObjectTag oTag, DataTag dTag)
        {
            Dictionary<SubObjectTag, int> dataDict;
            int gameData;

            if (intData.TryGetValue(dTag, out dataDict))
            {
                if (dataDict.TryGetValue(oTag, out gameData))
                {
                    return gameData;
                }
                else if (dataDict.TryGetValue(SubObjectTag.DEFAULT, out gameData))
                {
                    return gameData;
                }
            }
            return invalidData;
        }

        private void AddIntData(SubObjectTag oTag, DataTag dTag, int data)
        {
            if (!intData.ContainsKey(dTag))
            {
                intData.Add(dTag, new Dictionary<SubObjectTag, int>());
            }

            if (!intData[dTag].ContainsKey(oTag))
            {
                intData[dTag].Add(oTag, data);
            }
        }

        private void Awake()
        {
            invalidData = -99999;
            intData = new Dictionary<DataTag, Dictionary<SubObjectTag, int>>();

            InitializeData();

            Version = "0.0.1";
        }

        private void InitializeData()
        {
            AddIntData(SubObjectTag.PC, DataTag.HP, 10);
            AddIntData(SubObjectTag.PC, DataTag.Stress, 3);
            AddIntData(SubObjectTag.PC, DataTag.Damage, 2);

            AddIntData(SubObjectTag.Dummy, DataTag.HP, 3);

            AddIntData(SubObjectTag.DEFAULT, DataTag.Damage, 1);
            AddIntData(SubObjectTag.DEFAULT, DataTag.DropPotion, 1);
            AddIntData(SubObjectTag.DEFAULT, DataTag.MaxInfections, 1);
            AddIntData(SubObjectTag.DEFAULT, DataTag.Stress, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionAttack, 0);
            AddIntData(SubObjectTag.DEFAULT, DataTag.InfectionDefend, 0);
        }
    }
}
