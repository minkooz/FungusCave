﻿using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor
{
    public class Attack : MonoBehaviour
    {
        private ActorBoard actorBoard;
        private ActorData actorData;
        private ConvertCoordinates coord;
        private PotionData potionData;
        private int relieveStressAfterKill;

        public void MeleeAttack(int x, int y)
        {
            if (!GetComponent<Energy>().HasEnoughEnergy()
                || !actorBoard.HasActor(x, y))
            {
                return;
            }

            int attackEnergy = GetComponent<Energy>().GetAttackEnergy(
                coord.Convert(transform.position), new int[2] { x, y });
            GetComponent<Energy>().LoseEnergy(attackEnergy);

            GameObject target = actorBoard.GetActor(x, y);
            bool targetIsDead = target.GetComponent<HP>().LoseHP(
                 GetComponent<IDamage>().CurrentDamage);

            int potion = actorData.GetIntData(
                target.GetComponent<MetaInfo>().SubTag, DataTag.DropPotion);
            int bonusPotion = target.GetComponent<Infection>().HasInfection(
                InfectionTag.Mutated) ? potionData.BonusPotion : 0;

            if (targetIsDead)
            {
                if (GetComponent<MetaInfo>().IsPC)
                {
                    GetComponent<IHP>().RestoreAfterKill();
                    GetComponent<Stress>().LoseStress(relieveStressAfterKill);
                    GetComponent<Potion>().GainPotion(potion + bonusPotion);
                }
            }
            else
            {
                target.GetComponent<Infection>().GainInfection(gameObject);
                target.GetComponent<Energy>().LoseEnergy(
                    GetComponent<IEnergy>().Drain);
            }
        }

        private void Awake()
        {
            relieveStressAfterKill = 2;
        }

        private void Start()
        {
            actorBoard = FindObjects.GameLogic.GetComponent<ActorBoard>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            potionData = FindObjects.GameLogic.GetComponent<PotionData>();
        }
    }
}
