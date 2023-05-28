using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Camera
	{
		public GameObject Object { get; set; }
		public Vector3 SpawnPosition { get; set; }
		public Quaternion SpawnRotation { get; set; }
		public Vector3 SpawnOffset { get; set; }
		public CameraController Controller { get { return Object.GetComponent<CameraController>(); } }

	}
}
