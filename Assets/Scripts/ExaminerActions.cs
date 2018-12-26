﻿using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class ExaminerActions : MonoBehaviour
    {
        private void Update()
        {
            switch (GetComponent<PlayerInput>().GameCommand())
            {
                case Command.Left:
                case Command.Right:
                case Command.Up:
                case Command.Down:
                case Command.UpLeft:
                case Command.UpRight:
                case Command.DownLeft:
                case Command.DownRight:
                    Debug.Log(GetComponent<PlayerInput>().GameCommand());
                    break;

                case Command.Cancel:
                    FindObjects.GameLogic.GetComponent<SubGameMode>()
                        .SwitchExamineMode(false);
                    break;
            }
        }
    }
}