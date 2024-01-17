using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillUI_Components : MonoBehaviour
{
    [SerializeField] private Image _coolDownImage;
    public Image coolDownImage => _coolDownImage;

    [SerializeField] private Image _skillImage;
    public Image skillImage => _skillImage;
}

