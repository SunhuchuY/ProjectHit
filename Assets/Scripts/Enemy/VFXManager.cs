using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Enemy
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private VisualEffect _footStepVFX;
        [SerializeField] private VisualEffect _attackVFX;
        [SerializeField] private ParticleSystem _beingHitVFX;
        [SerializeField] private VisualEffect _beingHitSplashVFX;

        public void PlayAttackVFX()
        {
            _attackVFX.SendEvent("OnPlay");
        }

        public void BurstFootStep()
        {
            _footStepVFX.SendEvent("OnPlay");
        }

        public void BeingHitVFX(Vector3 attackerPos)
        {
            // Note: BeingHit Effect
            Vector3 direction = transform.position - attackerPos;
            direction.Normalize();
            direction.y = 0;
            _beingHitVFX.transform.rotation = Quaternion.LookRotation(direction);
            _beingHitVFX.Play();

            // Note: Splash Effect
            Vector3 splashPos = transform.position;
            splashPos.y += 2;
            VisualEffect newSplashVFX = Instantiate(_beingHitSplashVFX, splashPos, Quaternion.identity);
            newSplashVFX.Play();
            Destroy(newSplashVFX, 10f);
        }
    }
}
