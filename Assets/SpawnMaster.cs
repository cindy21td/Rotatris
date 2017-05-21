using UnityEngine;
using System.Collections;

public class SpawnMaster : MonoBehaviour {

	public GameObject[] blocks;

	private GameObject activeBlock;

	// Use this for initialization
	void Start () {
		//SpawnRandomBlock ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnRandomBlock(int gravity) {
		int idx = Random.Range(0, blocks.Length - 1);
		GameObject obj = blocks [idx];
		Vector3 pos = transform.position;

		float offsetX = 0;
		if (gravity == 1) {
			offsetX = 3f;
		} else if (gravity == 2) {
			offsetX = -3f;
		}

		if (obj.tag != "O" && obj.tag != "I") {
			pos = new Vector3 (pos.x - 0.5f + offsetX, pos.y + 0.5f, pos.z);
		}
		activeBlock = (GameObject) Instantiate (blocks [idx], pos, Quaternion.identity);
	}

	public GameObject getActiveBlock() {
		return activeBlock;
	}
}
