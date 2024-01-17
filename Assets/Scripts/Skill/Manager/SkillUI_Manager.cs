using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI_Manager : MonoBehaviour
{
    [SerializeField] private SkillUI_Components[] skillUI_Components = new SkillUI_Components[4];

    public void SkillCoolTimeUpdate(int index, float fill)
    {
        skillUI_Components[index].coolDownImage.fillAmount = fill;
    }

    public void OnSkillCoolTIme(int index)
    {
        skillUI_Components[index].coolDownImage.gameObject.SetActive(true);
        skillUI_Components[index].coolDownImage.fillAmount = 1;
    }

    public void OffSkillCoolTIme(int index)
    {
        skillUI_Components[index].coolDownImage.gameObject.SetActive(false);
    }
}
