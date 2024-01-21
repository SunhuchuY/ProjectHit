using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInventory : MonoBehaviour
{
    public SkillBase[] skills { get; private set; } = new SkillBase[4];

    public void InsertSkill(int index, SkillBase skill)
    {
        skills[index] = skill;
    }

    public void RemoveSkill(int index)
    {
        if (skills[index] == null)
        {
            return;
        }
        else
        {
            skills[index].Dispose();
            skills[index] = null;
        }
    }
}
