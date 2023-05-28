using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Food
	{
		public GameObject Object { get; set; }
		public float Size { get { return Object.transform.localScale.x + Object.transform.localScale.y / 2; } }
		public Vector3 Position { get { return Object.transform.localPosition; } }

	}
}
