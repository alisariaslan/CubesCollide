using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Sound
	{
		public GameObject Object { get; set; }
		public SoundController Controller { get { return Object.GetComponent<SoundController>(); } }
		public GameObject SoundObject { get; set; }
		public AudioSource SoundSource { get { return SoundObject.GetComponent<AudioSource>(); } }
		public void PlayQuickSound(AudioClip audioClip)
		{
			SoundSource.PlayOneShot(audioClip);
		}

	}
}
