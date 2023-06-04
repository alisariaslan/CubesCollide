using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Bot
	{
		public int Index { get; set; }
		public string Name { get; set; }
		public bool IsPlayer { get; set; }
		public bool IsDead { get; set; }
		public GameObject Object { get; set; }
		public Vector3 SpawnPosition { get; set; }
		public float Size { get { return Object.transform.localScale.x + Object.transform.localScale.y / 2; } }
		public Vector3 Position { get { return Object.transform.localPosition; } }
		public BotController Controller { get { return Object.GetComponent<BotController>(); } }
	}
}