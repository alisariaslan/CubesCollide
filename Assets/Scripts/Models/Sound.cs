using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Sound
	{
		public GameObject Object { get; set; }
		public SoundController Controller { get { return Object.GetComponent<SoundController>(); } }
	}
}
