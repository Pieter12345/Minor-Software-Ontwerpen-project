using UnityEngine;
using System.Collections;

public enum WeaponStats {

	PISTOL,
	RIFLE,
	SHOTGUN,
	RPG

}
public static class WeaponStatsExtension{

	public static int ClipSize(this WeaponStats w){
		int foo = 1;
		switch(w){
		case WeaponStats.PISTOL:	foo = 10;	break;
		case WeaponStats.RIFLE:		foo = 20;	break;
		case WeaponStats.SHOTGUN:	foo = 2;	break;
		case WeaponStats.RPG:		foo = 1;	break;
		}
		return foo;
	}

	public static AmmoTypes AmmoType(this WeaponStats w){
		AmmoTypes foo = AmmoTypes.PISTOL_BULLET;
		switch(w){
		case WeaponStats.PISTOL:	foo = AmmoTypes.PISTOL_BULLET;	break;
		case WeaponStats.RIFLE:		foo = AmmoTypes.RIFLE_BULLET;	break;
		case WeaponStats.SHOTGUN:	foo = AmmoTypes.SHELL;			break;
		case WeaponStats.RPG:		foo = AmmoTypes.ROCKET;			break;
		}
		return foo;
	}

	public static float BaseDamage(this WeaponStats w){
		float foo = 1f;
		switch(w){
		case WeaponStats.PISTOL:	foo = 1f;	break;
		case WeaponStats.RIFLE:		foo = 1.2f;	break;
		case WeaponStats.SHOTGUN:	foo = 10f;	break;
		case WeaponStats.RPG:		foo = 50f;	break;
		}
		return foo;
	}

	public static float FireInterval(this WeaponStats w){
		float foo = 1f;
		switch(w){
		case WeaponStats.PISTOL:	foo = 0.2f;	break;
		case WeaponStats.RIFLE:		foo = 0.2f;	break;
		case WeaponStats.SHOTGUN:	foo = 1f;	break;
		case WeaponStats.RPG:		foo = 3f;	break;
		}
		return foo;
	}

	public static float ReloadTime(this WeaponStats w){
		float foo = 1f;
		switch(w){
		case WeaponStats.PISTOL:	foo = 1f;	break;
		case WeaponStats.RIFLE:		foo = 0.2f;	break;
		case WeaponStats.SHOTGUN:	foo = 1f;	break;
		case WeaponStats.RPG:		foo = 3f;	break;
		}
		return foo;
	}

	public static float Recoil(this WeaponStats w){
		float foo = 1f;
		switch(w){
		case WeaponStats.PISTOL:	foo = 0.5f;	break;
		case WeaponStats.RIFLE:		foo = 1f;	break;
		case WeaponStats.SHOTGUN:	foo = 3f;	break;
		case WeaponStats.RPG:		foo = 5f;	break;
		}
		return foo;
	}

}