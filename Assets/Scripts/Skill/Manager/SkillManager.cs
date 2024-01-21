using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillInventory skillInventory;
    [SerializeField] private SKillInputManager sKillInputManager;
    [SerializeField] private SkillUI_Manager skillUI_Manager;

    private void Awake()
    {
        skillInventory.InsertSkill(0, new InvincibleSkill());
        skillInventory.InsertSkill(1, new InvincibleSkill());
        skillInventory.InsertSkill(2, new InvincibleSkill());
        skillInventory.InsertSkill(3, new InvincibleSkill());
    }

    private void Update()
    {
        for (int i = 0; i < sKillInputManager.isInput.Length; i++)
        {
            // condition: skill can use
            if (sKillInputManager.isInput[i] && !skillInventory.skills[i].isCoolTime)
            {
                sKillInputManager.isInput[i] = false;
                skillInventory.skills[i].isCoolTime = true;
                skillInventory.skills[i].Current = 0;
                skillInventory.skills[i].Start();
                skillUI_Manager.OnSkillCoolTIme(i);
            }

            if (skillInventory.skills[i].isCoolTime)
            {
                // update: cool time
                skillInventory.skills[i].Current += Time.deltaTime;
                skillInventory.skills[i].Update();
                skillUI_Manager.SkillCoolTimeUpdate(i, 1 - skillInventory.skills[i].CoolProgress);

                // condition: ending coolTime
                if (skillInventory.skills[i].Current > skillInventory.skills[i].Cooldown)
                {
                    sKillInputManager.isInput[i] = false;
                    skillInventory.skills[i].isCoolTime = false;
                    skillInventory.skills[i].Current = 0;
                    skillUI_Manager.OffSkillCoolTIme(i);
                    skillInventory.skills[i].End();
                }
            }
        }
    }
}
