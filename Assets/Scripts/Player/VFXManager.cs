using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private VisualEffect _footStep;


        public void Update_FootStep(bool state)
        {
            if(state)
            {
                _footStep.Play();
            }
            else
            {
                _footStep.Stop();
            }
        }
    }
}

