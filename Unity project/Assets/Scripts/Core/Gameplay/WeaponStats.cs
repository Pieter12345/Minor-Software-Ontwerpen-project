using UnityEngine;
using System.Collections;

public enum WeaponStats {
	PISTOL,
	RIFLE,
	SHOTGUN,
	RPG
}
public static class WeaponStatsExtension{

	public static int ClipSize(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 10;
		case WeaponStats.RIFLE   : return 30;
		case WeaponStats.SHOTGUN : return 2;
		case WeaponStats.RPG     : return 1;
		default                  : return 0;
		}
	}

	public static AmmoTypes AmmoType(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return AmmoTypes.PISTOL_BULLET;
		case WeaponStats.RIFLE   : return AmmoTypes.RIFLE_BULLET;
		case WeaponStats.SHOTGUN : return AmmoTypes.SHELL;
		case WeaponStats.RPG     : return AmmoTypes.ROCKET;
		default                  : return AmmoTypes.PISTOL_BULLET;
		}
	}

	public static float BaseDamage(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 1f;
		case WeaponStats.RIFLE   : return 1.2f;
		case WeaponStats.SHOTGUN : return 10f;
		case WeaponStats.RPG     : return 50f;
		default                  : return 1f;
		}
	}

	public static float FireInterval(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 0.2f;
		case WeaponStats.RIFLE   : return 0.05f;
		case WeaponStats.SHOTGUN : return 1f;
		case WeaponStats.RPG     : return 3f;
		default                  : return 1f;
		}
	}

	public static float ReloadTime(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 1f;
		case WeaponStats.RIFLE   : return 0.2f;
		case WeaponStats.SHOTGUN : return 1f;
		case WeaponStats.RPG     : return 3f;
		default                  : return 1f;
		}
	}

	public static float Recoil(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 0.5f;
		case WeaponStats.RIFLE   : return 0.5f;
		case WeaponStats.SHOTGUN : return 2f;
		case WeaponStats.RPG     : return 4f;
		default                  : return 1f;
		}
	}

}