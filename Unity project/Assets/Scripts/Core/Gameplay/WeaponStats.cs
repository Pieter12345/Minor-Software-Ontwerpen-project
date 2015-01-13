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

	public static string WeaponName(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return "PISTOL";
		case WeaponStats.RIFLE   : return "RIFLE";
		case WeaponStats.SHOTGUN : return "SHOTGUN";
		case WeaponStats.RPG     : return "RPG";
		case WeaponStats.LANDMINE: return "LANDMINE";
		default                  : return "None (or unimplemented in WeaponStats.cs)";
		}
	}

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

	// -----------------------------------------------------------------------------
	// Used to align the weapon model with the camera in third person (position).
	// -----------------------------------------------------------------------------
	public static Vector3 thirdPersonModelPosition(this WeaponStats w) {
		switch(w) {
//		case WeaponStats.PISTOL  : return new Vector3(-0.508f, 0.225f, 0.967f);
//		case WeaponStats.RIFLE   : return new Vector3(-0.463f, 0.248f, 1.112f);
//		case WeaponStats.SHOTGUN : return new Vector3(-0.48f, 0.262f, 1.035f);
//		case WeaponStats.RPG     : return new Vector3(-0.5959f, 0.273f, 0.897f);
//		case WeaponStats.LANDMINE: return new Vector3(-0.391f, 0.328f, 0.789f);
		case WeaponStats.PISTOL  : return new Vector3(0.037f, 0.167f, -0.085f);
		case WeaponStats.RIFLE   : return new Vector3(0.153f, 0.228f, -0.143f);
		case WeaponStats.SHOTGUN : return new Vector3(0.08f, 0.32f, -0.12f);
		case WeaponStats.RPG     : return new Vector3(-0.023f, 0.173f, -0.092f);
		case WeaponStats.LANDMINE: return new Vector3(-0.03f, 0.277f, 0.097f);
		default                  : return new Vector3(0f, 0f, 0f);
		}
	}
	
	// -----------------------------------------------------------------------------
	// Used to align the weapon model with the camera in third person (rotation).
	// -----------------------------------------------------------------------------
	public static Vector3 thirdPersonModelRotation(this WeaponStats w) {
		switch(w) {
//		case WeaponStats.PISTOL  : return new Vector3(312.83f, 276.26f, 7.61f);
//		case WeaponStats.RIFLE   : return new Vector3(298.43f, 281.08f, 355.61f);
//		case WeaponStats.SHOTGUN : return new Vector3(298.06f, 272.52f, 16.42f);
//		case WeaponStats.RPG     : return new Vector3(333.54f, 96.04f, 3.30f);
//		case WeaponStats.LANDMINE: return new Vector3(0f, 0f, 0f);
		case WeaponStats.PISTOL  : return new Vector3(333.13f, 270.00f, 108.92f);
		case WeaponStats.RIFLE   : return new Vector3(330.21f, 238.32f, 107.05f);
		case WeaponStats.SHOTGUN : return new Vector3(356.60f, 188.72f, 126.27f);
		case WeaponStats.RPG     : return new Vector3( 21.73f, 165.05f, 297.10f);
		case WeaponStats.LANDMINE: return new Vector3(  2.87f,  25.17f,  80.21f);
		default                  : return new Vector3(     0f,      0f,      0f);
		}
	}

	// -----------------------------------------------------------------------------
	// Used to align the weapon model with the camera in first person when aiming down sight (position).
	// -----------------------------------------------------------------------------
	public static Vector3 firstPersonAimDownSightModelPosition(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return new Vector3( 0.026f, -0.149f, 0.550f);
		case WeaponStats.RIFLE   : return new Vector3( 0.120f, -0.210f, 0.800f);
		case WeaponStats.SHOTGUN : return new Vector3(-0.031f, -0.050f, 0.810f);
		case WeaponStats.RPG     : return new Vector3(     0f, -0.190f, 0.700f);
		case WeaponStats.LANDMINE: return new Vector3(     0f, -0.200f, 1.100f);
		default                  : return new Vector3(0f, 0f, 0f);
		}
	}
	
	// -----------------------------------------------------------------------------
	// Used to align the weapon model with the camera in first person when aiming down sight (rotation).
	// -----------------------------------------------------------------------------
	public static Vector3 firstPersonAimDownSightModelRotation(this WeaponStats w) {
		switch(w) {
		case WeaponStats.PISTOL  : return new Vector3(270f, 270f, 0f);
		case WeaponStats.RIFLE   : return new Vector3(270f, 270f, 0f);
		case WeaponStats.SHOTGUN : return new Vector3(270f, 270f, 0f);
		case WeaponStats.RPG     : return new Vector3(0f, 90f, 358.51f);
		case WeaponStats.LANDMINE: return new Vector3(0f, 0f, 0f);
		default                  : return new Vector3(0f, 0f, 0f);
		}
	}

}