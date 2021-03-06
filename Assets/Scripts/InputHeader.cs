﻿using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public class InputHeader : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            if (Input.GetKey(KeyCode.LeftShift)
                || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    return Command.Previous;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                return Command.Next;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                return Command.Cancel;
            }
            return Command.INVALID;
        }
    }
}
