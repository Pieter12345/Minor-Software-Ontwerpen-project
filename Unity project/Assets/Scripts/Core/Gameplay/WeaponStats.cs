using UnityEngine;
using System.Collections;

public enum WeaponStats {
	PISTOL,
	RIFLE,
	SHOTGUN,
	RPG,
	LANDMINE
}
public static class WeaponStatsExtension{

	public static int ClipSize(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 10;
		case WeaponStats.RIFLE   : return 30;
		case WeaponStats.SHOTGUN : return 2;
		case WeaponStats.RPG     : return 1;
		case WeaponStats.LANDMINE: return 1;
		default                  : return 0;
		}
	}

	public static AmmoTypes AmmoType(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return AmmoTypes.PISTOL_BULLET;
		case WeaponStats.RIFLE   : return AmmoTypes.RIFLE_BULLET;
		case WeaponStats.SHOTGUN : return AmmoTypes.SHELL;
		case WeaponStats.RPG     : return AmmoTypes.ROCKET;
		case WeaponStats.LANDMINE: return AmmoTypes.LANDMINE;
		default                  : return AmmoTypes.PISTOL_BULLET;
		}
	}

	public static float BaseDamage(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 1f;
		case WeaponStats.RIFLE   : return 1.2f;
		case WeaponStats.SHOTGUN : return 10f;
		case WeaponStats.RPG     : return 50f;
		case WeaponStats.LANDMINE: return 40f;
		default                  : return 1f;
		}
	}

	public static float FireInterval(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 0.2f;
		case WeaponStats.RIFLE   : return 0.05f;
		case WeaponStats.SHOTGUN : return 1f;
		case WeaponStats.RPG     : return 3f;
		case WeaponStats.LANDMINE: return 0.1f;
		default                  : return 1f;
		}
	}

	public static float ReloadTime(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 1f;
		case WeaponStats.RIFLE   : return 0.2f;
		case WeaponStats.SHOTGUN : return 1f;
		case WeaponStats.RPG     : return 3f;
		case WeaponStats.LANDMINE: return 0.2f;
		default                  : return 1f;
		}
	}

	public static float Recoil(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return 0.5f;
		case WeaponStats.RIFLE   : return 0.2f;
		case WeaponStats.SHOTGUN : return 2f;
		case WeaponStats.RPG     : return 4f;
		case WeaponStats.LANDMINE: return 0.01f;
		default                  : return 1f;
		}
	}

	// -----------------------------------------------------------------------------
	// Used to align the weapon model with the camera in first person (position).
	// -----------------------------------------------------------------------------
	public static Vector3 firstPersonModelPosition(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return new Vector3(0.342f, -0.303f, 0.656f);
		case WeaponStats.RIFLE   : return new Vector3(0.468f, -0.358f, 0.878f);
		case WeaponStats.SHOTGUN : return new Vector3(0.22f, -0.169f, 0.703f);
		case WeaponStats.RPG     : return new Vector3(0.241f, -0.31f, 0.776f);
		case WeaponStats.LANDMINE: return new Vector3(0.485f, -0.421f, 1.089f);
		default                  : return new Vector3(0f, 0f, 0f);
		}
	}

	// -----------------------------------------------------------------------------
	// Used to align the weapon model with the camera in first person (rotation).
	// -----------------------------------------------------------------------------
	public static Vector3 firstPersonModelRotation(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return new Vector3(270f, 270f, 0f);
		case WeaponStats.RIFLE   : return new Vector3(270f, 270f, 0f);
		case WeaponStats.SHOTGUN : return new Vector3(270f, 270f, 0f);
		case WeaponStats.RPG     : return new Vector3(0f, 90f, 0f);
		case WeaponStats.LANDMINE: return new Vector3(30f, 290f, 20f);
		default                  : return new Vector3(0f, 0f, 0f);
		}
	}

}