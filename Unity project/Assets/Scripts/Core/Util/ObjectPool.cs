using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {

	protected GameObject toPool;

	public int PoolAmount { get; set; }

	public bool CanGrow { get; set; }

	protected List<GameObject> pool;

	public ObjectPool(GameObject toPool, int poolAmount = 5, bool canGrow = true){
		this.toPool = toPool;
		PoolAmount = poolAmount;
		CanGrow = canGrow;
		pool = new List<GameObject>();

		for(int i = 0; i < poolAmount; i++){
			GameObject o = GameObject.Instantiate(toPool) as GameObject;
			o.SetActive(false);
			pool.Add(o);
		}
	}

	public GameObject GetFreeObject(){
		foreach(GameObject o in pool){
			if(!o.activeInHierarchy)
				return o;
		}

		if(CanGrow){
			GameObject o = (GameObject) GameObject.Instantiate(toPool);
			pool.Add(o);
			return o;
		}

		return null;
	}

}
