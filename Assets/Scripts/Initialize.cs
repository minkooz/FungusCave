﻿using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using System.IO;
using UnityEngine;

namespace Fungus.GameSystem
{
    public interface IInitialize
    {
        void Initialize();
    }

    public class Initialize : MonoBehaviour
    {
        private string dungeon;

        public bool Initialized { get; private set; }

        private void InitBlueprint()
        {
            GetComponent<BlueprintSponge>().DrawBlueprint();
            GetComponent<BlueprintPool>().DrawBlueprint();
            GetComponent<BlueprintFungus>().DrawBlueprint();
        }

        private void InitializeDungeonLevel()
        {
            GetComponent<SaveLoadGame>().LoadDungeonLevel();

            InitBlueprint();
            InitWorld();
        }

        private void InitializeGame()
        {
            GetComponent<RandomNumber>().Initialize();

            InitBlueprint();
            InitWorld();
        }

        private void InitWorld()
        {
            GetComponent<CreateWorld>().Initialize();
            GetComponent<DungeonTerrain>().Initialize();

            GetComponent<SubMode>().SwitchModeOpening(
                GetComponent<GameSetting>().ShowOpening);
        }

        private void Start()
        {
            dungeon = Path.Combine(
                GetComponent<SaveLoadFile>().BinaryDirectory,
                GetComponent<SaveLoadGame>().DungeonFile);
        }

        private void Update()
        {
            if (!Initialized)
            {
                if (File.Exists(dungeon))
                {
                    InitializeDungeonLevel();
                }
                else
                {
                    InitializeGame();
                }
                Debug.Log(GetComponent<RandomNumber>().RootSeed);
                Initialized = true;
            }
        }
    }
}
