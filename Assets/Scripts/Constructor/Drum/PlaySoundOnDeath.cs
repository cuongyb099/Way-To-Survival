using UnityEngine;

namespace DefaultNamespace.Constructor.Drum
{
    public class PlaySoundOnDeath : MonoBehaviour
    {
        public AudioClip clip;

        private void Awake()
        {
            if (TryGetComponent(out IDamagable damagable))
            {
                damagable.OnDeath += () =>
                {
                    AudioManager.Instance.PlaySound(clip, volumeType: SoundVolumeType.SOUNDFX_VOLUME);
                };
            }
        }
    }
}