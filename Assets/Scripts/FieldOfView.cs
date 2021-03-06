﻿using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor.FOV
{
    public enum FOVStatus { INVALID, TEST, Unknown, Visited, Insight };

    public interface IFOVAlgorithm { void UpdateFOV(); }

    public class FieldOfView : MonoBehaviour, IResetData, ISaveLoadActorData
    {
        private ActorData actorData;
        private DungeonBoard board;
        private FOVStatus[,] fovBoard;

        public bool LoadedActorData { get; private set; }

        public int MaxRange
        {
            get
            {
                return actorData.GetIntData(GetComponent<MetaInfo>().SubTag,
                    DataTag.SightRange);
            }
        }

        public void ChangeFOVBoard(FOVStatus status, int[] position)
        {
            int x = position[0];
            int y = position[1];

            ChangeFOVBoard(status, x, y);
        }

        public void ChangeFOVBoard(FOVStatus status, int x, int y)
        {
            if (board.IndexOutOfRange(x, y))
            {
                return;
            }
            fovBoard[x, y] = status;
        }

        public bool CheckFOV(FOVStatus status, int[] position)
        {
            int x = position[0];
            int y = position[1];

            return CheckFOV(status, x, y);
        }

        public bool CheckFOV(FOVStatus status, int x, int y)
        {
            if (board.IndexOutOfRange(x, y))
            {
                return false;
            }
            return fovBoard[x, y] == status;
        }

        public FOVStatus GetFOVStatus(int x, int y)
        {
            if (board.IndexOutOfRange(x, y))
            {
                return FOVStatus.INVALID;
            }
            return fovBoard[x, y];
        }

        public void Load(DTActor data)
        {
            if (GetComponent<MetaInfo>().IsPC)
            {
                fovBoard = data.FovBoard;
                LoadedActorData = true;
            }
        }

        public void Reset()
        {
            fovBoard = new FOVStatus[board.Width, board.Height];

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    ChangeFOVBoard(FOVStatus.Unknown, i, j);
                }
            }
        }

        public void Save(DTActor data)
        {
            if (GetComponent<MetaInfo>().IsPC)
            {
                data.FovBoard = fovBoard;
            }
        }

        public void UpdateFOV()
        {
            UpdateMemory();

            // You can enable multiple FOV algorithms at the same time for
            // testing. FOVSimple covers a larger area than FOVRhombus and it is
            // painted red.
            // 1) In `ObjectPool.cs`, attach FOVSimple component to the testing
            // actor.
            // 2) In `FOVSimple.cs`, set `fovTest` to `true`.
            // 3) Uncomment the following line.

            //GetComponent<FOVSimple>().UpdateFOV();
            GetComponent<FOVRhombus>().UpdateFOV();
        }

        private void Start()
        {
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();

            if (!LoadedActorData)
            {
                Reset();
            }
        }

        private void UpdateMemory()
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (CheckFOV(FOVStatus.Insight, i, j)
                        || CheckFOV(FOVStatus.TEST, i, j))
                    {
                        ChangeFOVBoard(FOVStatus.Visited, i, j);
                    }
                }
            }
        }
    }
}
