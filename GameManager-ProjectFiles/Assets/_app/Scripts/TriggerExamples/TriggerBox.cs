using System;
using _app.Scripts.Managers;
using UnityEngine;

namespace _app.Scripts.TriggerExamples
{
    public class TriggerBox : MonoBehaviour
    {
        [SerializeField]
        public int scoreAmount;
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (AudioManager.instance != null && GameManager.instance != null)
                {
                    AudioManager.instance.PlayAudio();
                    GameManager.instance.ChangeScore(scoreAmount);
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("Player Entered Trigger, No AudioManager");
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            Debug.Log(message:other.transform.name + " Exited Trigger");
        }

        public void OnCollisionEnter(Collision other)
        {
            Debug.Log(message:"Collided with Object");
        }

        public void OnCollisionExit(Collision other)
        {
            Debug.Log(message:"No Longer Colliding With Object");
        }
    }

}
