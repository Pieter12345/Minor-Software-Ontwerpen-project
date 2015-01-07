using UnityEngine;
using System.Collections;

[System.Serializable]
public class BlockType {

	public Texture texture;
	public bool canWalkThrough = false;
	public bool hasCollider = true;
	public bool isTransparent = false;

}
