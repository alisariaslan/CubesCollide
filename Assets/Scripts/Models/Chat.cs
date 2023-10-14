using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Chat
	{
		public GameObject Object { get; set; }
		public float ClearInterval { get; set; }
		public ChatController Controller { get { return Object?.GetComponent<ChatController>(); } }

	}
}
