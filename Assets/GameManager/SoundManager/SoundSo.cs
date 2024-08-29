using UnityEngine;

namespace SoundManager
{
    [CreateAssetMenu(fileName = "SoundSo", menuName = "SoundSO", order = 0)]
    public class SoundSo : ScriptableObject
    {
        public AudioClip[] audioClips;

        public AudioClip GetAudioClip(int id)
        {
            return audioClips[id];
        }
    }
}