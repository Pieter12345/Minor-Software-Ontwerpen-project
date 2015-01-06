using UnityEngine;
using System.Collections;

public class MultiLoot : MonoBehaviour {

	private GameObject spawner;
	private LootSpawner ls;
	private float cumulativeProb;

	[Range(0.0f, 1.0f)]
	public float masterProbability = 0.4f;
	public LootAble[] lootAble;

	// Use this for initialization
	void Start () {
		spawner = GameObject.FindGameObjectWithTag("PickupSpawner");
		ls = spawner.GetComponent<LootSpawner>();
		cumulativeProb = 0;
		if(lootAble.Length > 0){
			foreach(LootAble l in lootAble){
				cumulativeProb += l.probability;
			}
		}
	}
	
	// Update is called once per frame
	void OnDisable () {
		if(ls==null){
			Debug.LogWarning("NO SPAWNER FOUND!");
			return;
		}
		if(Random.Range(0f, 1f) < masterProbability){
//			int spawnindex = Mathf.FloorToInt(Random.Range(0,lootAble.Length));
			ls.Spawn(GetToSpawn(), transform.position, Quaternion.identity);
		}
	}

	GameObject GetToSpawn(){
		float probSum = 0f;
		float rnd = Random.Range(0f, cumulativeProb);
		if(lootAble.Length > 0){
			foreach(LootAble l in lootAble){
				probSum += l.probability;
				if (rnd < probSum)
					return l.pickUp;
			}
		}
		return lootAble[lootAble.Length-1].pickUp;
	}
}
