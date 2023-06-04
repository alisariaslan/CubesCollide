using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
	public class Canvas
	{
		public GameObject Object { get; set; }
		public Animator Animator { get { return Object.GetComponent<Animator>(); } }
		public CanvasController Controller { get { return Object.GetComponent<CanvasController>(); } }

	}
}
