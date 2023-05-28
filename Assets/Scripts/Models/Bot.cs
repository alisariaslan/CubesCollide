using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Bot
	{
		public string Name { get; set; }
		public GameObject Object { get; set; }
		public Vector3 SpawnPosition { get; set; }
		public float Size { get { return Object.transform.localScale.x + Object.transform.localScale.y / 2; } }
		public Vector3 Position { get { return Object.transform.localPosition; } }
		public BotController Controller { get { return Object.GetComponent<BotController>(); } }
	}
}