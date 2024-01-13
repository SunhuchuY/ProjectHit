using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private VisualEffect _footStep;
        [SerializeField] private ParticleSystem _Blade01;
        [SerializeField] private ParticleSystem _Blade02;
        [SerializeField] private ParticleSystem _Blade03;
        [SerializeField] private VisualEffect _slash;
        [SerializeField] private VisualEffect _heal;



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

        public void PlayBlade01()
        {
            _Blade01.Play();
        }

        public void PlayBlade02()
        {
            _Blade02.Play();
        }

        public void PlayBlade03()
        {
            _Blade03.Play();
        }

        public void StopBlade()
        {
            _Blade01.Simulate(0);
            _Blade01.Stop();

            _Blade02.Simulate(0);
            _Blade02.Stop();

            _Blade03.Simulate(0);
            _Blade03.Stop();
        }

        public void PlaySlash(Vector3 pos)
        {
            _slash.transform.position = pos;
            _slash.Play();
        }

        public void PlayHeal()
        {
            _heal.Play();
        }
    }
}

