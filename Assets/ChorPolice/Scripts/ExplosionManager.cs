using UnityEngine;
using System.Collections;

namespace ArtboxGames
{
    //to decide explosion sound effect
    public enum ObjectDestroyed
    {
        car, enemyCar
    }

    public class ExplosionManager : MonoBehaviour
    {
        private AudioSource audioS; //ref to audioSource component
        private ParticleSystem effect;//ref to particle system
        private ParticleSystemRenderer explosionMat;

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        // Use this for initialization
        void Start()
        {   //we get the ref of component attached to this object
            effect = GetComponent<ParticleSystem>();
            audioS = GetComponent<AudioSource>();
            explosionMat = GetComponent<ParticleSystemRenderer>();
            explosionMat.material.mainTexture = vars.explosionEffect;
        }
        //method called when this object is actiated in the scene
        public void BasicSettings(ObjectDestroyed obj)
        {
            audioS = GetComponent<AudioSource>();
            effect = GetComponent<ParticleSystem>();
            //check if effect is not playing
            if (!effect.isPlaying)
                effect.Play();//then play the effect
                              //if the obj is car
            if (obj == ObjectDestroyed.car)
            {   //we play car sound
                audioS.PlayOneShot(vars.carExplosionSound);
                Invoke("StopSound", 2f);
            }//if enemyCar we play enemyCar sound
            else if (obj == ObjectDestroyed.enemyCar)
                audioS.PlayOneShot(vars.enemyExplosionSound);

            StartCoroutine(DeactivateObj());
        }

        IEnumerator DeactivateObj()
        {   //it deactivates it after 4.5f time
            yield return new WaitForSeconds(4.5f);
            gameObject.SetActive(false);
        }
        //when car is destroyed we make volume to 0
        void StopSound()
        {
            AudioListener.volume = 0;
        }
    }
}