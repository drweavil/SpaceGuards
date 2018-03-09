using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {
	public static SpawnerController instance;
	public Spawner topSpawner;
	public Spawner bottomSpawner;
	public Spawner parallaxSpawnerBottom;
	public Spawner parallaxSpawnerTop;

	public Spawner boxPathSpawner1;
	public Spawner boxPathSpawner2;
	public Spawner boxPathSpawner3;

	public Spawner allScreenSpawner;


	public Spawner leftTorpedoShipSpawner;
	public Spawner rightTorpedoShipSpawner;
	public Spawner torpedoLaunchShipSpawner;

	public Spawner leftArmorShipSpawner;
	public Spawner centerArmorShipSpawner;
	public Spawner rightArmorShipSpawner;

	public Spawner leftCarrierShipSpawner;
	public Spawner centerCarrierShipSpawner;
	public Spawner rightCarrierShipSpawner;

	public Spawner laserShipPositionSpawner;

	public Spawner topLeftElusiveSpawner;
	public Spawner topElusiveSpawner;
	public Spawner topRightElusiveSpawner;
	public Spawner leftElusiveBossSpawner;
	public Spawner centerElusiveBossSpawner;
	public Spawner rightElusiveBossSpawner;

	public Spawner leftPolynomialBossSpawner;
	public Spawner centerPolynomialBossSpawner;
	public Spawner rightPolynomialBossSpawner;

	public Spawner leftCarrierBossSpawner;
	public Spawner centerCarrierBossSpawner;
	public Spawner rightCarrierBossSpawner;
	public Spawner carrierBossShipSpawnPosition;
	public SpawnerGroup firstCarrirerBossGroup;
	public SpawnerGroup secondCarrirerBossGroup;
	public SpawnerGroup thirdCarrirerBossGroup;



	void Awake(){
		instance = this;
	}
}
