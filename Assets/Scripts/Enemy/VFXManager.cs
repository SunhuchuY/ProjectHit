using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemy
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private VisualEffect _visualEffect;
        
        public void BurstFootStep()
        {
            _visualEffect.SendEvent("OnPlay");
        }
    }
}
