using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Audio
	{
		public GameObject Music { get; set; }
		public GameObject Sound { get; set; }
		public AudioController Controller { get { return Music.GetComponent<AudioController>(); } }
		public AudioSource MusicSource { get { return Music.GetComponent<AudioSource>(); } }
		public AudioSource SoundSource { get { return Sound.GetComponent<AudioSource>(); } }

	

	}
}
