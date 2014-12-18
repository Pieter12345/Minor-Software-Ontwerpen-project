using UnityEngine;
using System.Collections;

public class MultiLoot : MonoBehaviour {

	private GameObject spawner;
	private LootSpawner ls;
	[Range(0.0f, 1.0f)]
	public float masterProbability = 0.4f;
	public GameObject[] spawnable;

	// Use this for initialization
	void Start () {
		spawner = GameObject.FindGameObjectWithTag("PickupSpawner");
		ls = spawner.GetComponent<LootSpawner>();
	}
	
	// Update is called once per frame
	void OnDisable () {
		if(ls==null){
			Debug.LogWarning("NO SPAWNER FOUND!");
			return;
		}
		if(Random.Range(0f, 1f) < masterProbability){
			int spawnindex = Mathf.FloorToInt(Random.Range(0,spawnable.Length-1));
			ls.Spawn(spawnable[spawnindex], transform.position, Quaternion.identity);
		}
	}
}
