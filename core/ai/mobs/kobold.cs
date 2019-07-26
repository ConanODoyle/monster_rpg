if (!isObject($RPGData_Kobold))
{
	$RPGData_Kobold = new ScriptObject(RPGDataObject) {};
	$RPGData_KoboldThief = new ScriptObject(RPGDataObject) {};
	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_Kobold);");
	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_KoboldThief);");
}

if (!isObject($RPGData_KoboldAttack))
{
	$RPGData_KoboldAttack = new ScriptObject(KoboldAttackType) {
		attackImage = "KoboldFistsImage";
		triggerTime = "1 1";
		minRange = 0;
		maxRange = 4;
		stopRange = 1;
	};

	$RPGData_KoboldRangedAttack = new ScriptObject(KoboldRangedAttackType) {
		attackImage = "KoboldDaggerImage";
		triggerTime = "1 1";
		minRange = 0;
		maxRange = 30;
		stopRange = 10;
		backingAttack = 1;
	};

	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_KoboldAttack);");
	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_KoboldRangedAttack);");
}

$RPGData_Kobold.name = "Kobold";
$RPGData_Kobold.maxDamage = 100;
$RPGData_Kobold.armor = 0;
$RPGData_Kobold.resist = 5;
$RPGData_Kobold.level = 35;
$RPGData_Kobold.thinkTime = 1;
$RPGData_Kobold.resetTargetTime = 10;

$RPGData_Kobold.searchType = "NearRadiusLOSSearchType";

$RPGData_Kobold.attackType = "KoboldAttackType";

// $RPGData_Kobold.wanderSpeed = 10;
// $RPGData_Kobold.leashDistance = 10;
$RPGData_Kobold.autoCutoffRange = 40;

$RPGData_Kobold.botLogicFunc = "MRPGBot_simpleLogic";
$RPGData_Kobold.botActionFunc = "MRPGBot_simpleAction";
$RPGData_Kobold.damageCallbackFunc = "MRPGBot_simpleDamageCallback";

$RPGData_Kobold.expReward = "12 16";
$RPGData_Kobold.goldReward = "4 6";

$RPGData_Kobold.maxForwardSpeed = 5.5;
$RPGData_Kobold.maxBackwardSpeed = 5;
$RPGData_Kobold.maxSideSpeed = 4;
$RPGData_Kobold.maxYawSpeed = 20;
$RPGData_Kobold.maxPitchSpeed = 20;
$RPGData_Kobold.scale = "0.4 0.4 0.4";
$RPGData_Kobold.botDB = "PlayerStandardArmor";
$RPGData_Kobold.canJump = 1;






$RPGData_KoboldThief.name = "Kobold Thief";
$RPGData_KoboldThief.maxDamage = 80;
$RPGData_KoboldThief.armor = 0;
$RPGData_KoboldThief.resist = 3;
$RPGData_KoboldThief.level = 45;
$RPGData_KoboldThief.thinkTime = 1;
$RPGData_KoboldThief.resetTargetTime = 4;

$RPGData_KoboldThief.searchType = new ScriptObject(KoboldThiefLOSSearch : RadiusLOSSearchType) { searchRadius = 40; };

$RPGData_KoboldThief.attackType = "KoboldRangedAttackType";

// $RPGData_Kobold.wanderSpeed = 10;
// $RPGData_Kobold.leashDistance = 10;
// $RPGData_Kobold.autoCutoffRange = 40;

$RPGData_KoboldThief.botLogicFunc = "MRPGBot_simpleLogic";
$RPGData_KoboldThief.botActionFunc = "MRPGBot_simpleAction";
$RPGData_KoboldThief.damageCallbackFunc = "MRPGBot_simpleDamageCallback";

$RPGData_KoboldThief.expReward = "16 20";
$RPGData_KoboldThief.goldReward = "1 2";

$RPGData_KoboldThief.maxForwardSpeed = 6.5;
$RPGData_KoboldThief.maxBackwardSpeed = 5;
$RPGData_KoboldThief.maxSideSpeed = 4;
$RPGData_KoboldThief.maxYawSpeed = 10;
$RPGData_KoboldThief.maxPitchSpeed = 10;
$RPGData_KoboldThief.scale = "0.35 0.35 0.35";
$RPGData_KoboldThief.botDB = "PlayerStandardArmor";



datablock ProjectileData(KoboldFistsProjectile : FistsProjectile)
{
	directDamage = 10;
	type = "Physical";
};

datablock ShapeBaseImageData(KoboldFistsImage : FistsImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	armReady = false;
	colorShiftColor = "0.47 0.33 0.33 1";
	projectile = KoboldFistsProjectile;

	stateTimeoutValue[3]		= 2;
};

function KoboldFistsImage::onFire(%this, %obj, %slot)
{
	parent::onFire(%this, %obj, %slot);
	%obj.playThread(0, activate2);
}




datablock ProjectileData(KoboldDaggerProjectile : DaggerProjectile)
{
	//shapeFile = "./daggerProjectile.dts";
	directDamage = 8;
	type = "Physical";
};

datablock ShapeBaseImageData(KoboldDaggerImage : DaggerImage)
{
	//shapeFile = "./dagger.dts";
	colorShiftColor = "0.53 0.43 0 1";
	projectile = KoboldDaggerProjectile;

	stateTimeoutValue[3]		= 2.8;
};