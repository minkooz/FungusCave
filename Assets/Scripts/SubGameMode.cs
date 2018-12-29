﻿using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class SubGameMode : MonoBehaviour
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;

        public GameObject ExamineTarget
        {
            get
            {
                int[] pos = coord.Convert(
                    FindObjects.Examiner.transform.position);

                return actor.GetActor(pos[0], pos[1]);
            }
        }

        public void SwitchModeExamine(bool switchOn)
        {
            SwitchUIExamineModeline(switchOn);
            SwitchUIExamineMessage(false);

            GetComponent<SchedulingSystem>().PauseTurn(switchOn);

            FindObjects.Examiner.transform.position
                = FindObjects.PC.transform.position;
            FindObjects.Examiner.SetActive(switchOn);
        }

        public void SwitchUIExamineMessage(bool switchOn)
        {
            FindObjects.GetUIObject(UITag.ExamineMessage).SetActive(switchOn);
            FindObjects.GetUIObject(UITag.Message).SetActive(!switchOn);
        }

        public void SwitchUIExamineModeline(bool switchOn)
        {
            FindObjects.GetUIObject(UITag.ExamineModeline).SetActive(switchOn);
        }

        private void Start()
        {
            actor = GetComponent<ActorBoard>();
            coord = GetComponent<ConvertCoordinates>();
        }
    }
}
