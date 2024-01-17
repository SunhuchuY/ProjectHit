using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInventory : MonoBehaviour
{
    public Skill[] skills { get; private set; } = new Skill[4];

    public void InsertSkill(int index, Skill skill)
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
