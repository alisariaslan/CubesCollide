using UnityEngine;

public class Random
{
	public GameObject Object;
	public Randomizer Controller { get { return Object.GetComponent<Randomizer>(); } }
}

