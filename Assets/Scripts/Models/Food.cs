using System;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Food
	{
		public int Index { get; set; }
		public GameObject Object { get; set; }
		public bool IsEaten { get; set; }
		public DateTime EatenTime { get; set; }
		public float Size { get { return Object.transform.localScale.x + Object.transform.localScale.y / 2; } }
		public Vector3 Position { get { return Object.transform.localPosition; } }

	}
}
