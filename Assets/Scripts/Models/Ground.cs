using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Ground
	{
		public GameObject Object { get; set; }
		public List<Vector3> Points { get; set; }
		public float Size { get { return (Object.transform.localScale.x + Object.transform.localScale.z) / 2; } }
		public Vector3 Position { get { return Object.transform.localPosition; } }
	}
}
