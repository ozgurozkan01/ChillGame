using GameManager.Base;
using SoundManager;
using UnityEngine;
    
namespace GameManager.SoundManager
{
    public class SoundManager : ManagerBase
    {
        
        public SoundSo soundSo;
        public GameObject soundPrefab;
        
        public void PlaySound(int soundIndex)
        {
            var createdSound = Instantiate(soundPrefab, transform.position, Quaternion.identity);
            var audioSource = createdSound.GetComponent<AudioSource>();
            var audioClip = soundSo.GetAudioClip(soundIndex);
            
            audioSource.PlayOneShot(audioClip);
        }
        
    }
}