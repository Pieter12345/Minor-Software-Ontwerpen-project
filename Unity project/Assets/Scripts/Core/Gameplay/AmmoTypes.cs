using UnityEngine;
using System.Collections;

public enum AmmoTypes {

	PISTOL_BULLET,
	RIFLE_BULLET,
	SHELL,
	ROCKET,
	LANDMINE

}

public static class AmmoTypesExtension{

	public static int GetMaxAmmo(this AmmoTypes t){
		switch(t){
		case AmmoTypes.PISTOL_BULLET:	return 80;
		case AmmoTypes.RIFLE_BULLET:	return 200;
		case AmmoTypes.SHELL:			return 20;
		case AmmoTypes.ROCKET:			return 2;
		case AmmoTypes.LANDMINE:		return 20;
		
		default:
			return 1;
		}
	}

}