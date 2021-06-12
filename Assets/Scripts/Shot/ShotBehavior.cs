using UnityEngine;
using System.Collections;

public class ShotBehavior : MonoBehaviour {

	internal float speed;

	void Update ()
    {
		transform.position += transform.forward * Time.deltaTime * speed;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("OOFArea"))
		{
			transform.position = new Vector3(-150, 0, 0);
			speed = 0;
		}
	}
}