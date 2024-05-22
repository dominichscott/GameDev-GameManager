using System;
using UnityEngine;

namespace _app.Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public new AudioClip audio;
        public AudioSource audioSource;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        public void PlayAudio()
        {
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
}