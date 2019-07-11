if (!isObject($RPGData_Goblin))
{
	$RPGData_Goblin = new ScriptObject(RPGDataObject) {};
	$RPGData_GoblinArcher = new ScriptObject(RPGDataObject) {};
	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_Goblin);");
	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_GoblinArcher);");
}

if (!isObject($RPGData_GoblinAttack))
{
	$RPGData_GoblinAttack = new ScriptObject(GoblinAttackType) {
		attackImage = "goblinSwordImage";
		triggerTime = "1 1";
		minRange = 0;
		maxRange = 4;
		stopRange = 1;
	};

	$RPGData_GoblinRangedAttack = new ScriptObject(GoblinRangedAttackType) {
		attackImage = "goblinBowImage";
		triggerTime = "1 1";
		minRange = 0;
		maxRange = 100;
		stopRange = 1000;
		backingAttack = 1;
	};

	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_GoblinAttack);");
	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_GoblinRangedAttack);");
}

$RPGData_Goblin.name = "Goblin";
$RPGData_Goblin.maxDamage = 80;
$RPGData_Goblin.armor = 0;
$RPGData_Goblin.resist = 0;
$RPGData_Goblin.level = -500;
$RPGData_Goblin.thinkTime = 1;
$RPGData_Goblin.resetTargetTime = 10;

$RPGData_Goblin.searchType = "NearRadiusLOSSearchType";

$RPGData_Goblin.attackType = "GoblinAttackType";

// $RPGData_Goblin.wanderSpeed = 10;
// $RPGData_Goblin.leashDistance = 10;
$RPGData_Goblin.autoCutoffRange = 40;

$RPGData_Goblin.botLogicFunc = "MRPGBot_simpleLogic";
$RPGData_Goblin.botActionFunc = "MRPGBot_simpleAction";
$RPGData_Goblin.damageCallbackFunc = "MRPGBot_simpleDamageCallback";

$RPGData_Goblin.expReward = "6 8";
$RPGData_Goblin.goldReward = "1 2";

$RPGData_Goblin.maxForwardSpeed = 4.5;
$RPGData_Goblin.maxBackwardSpeed = 2.8;
$RPGData_Goblin.maxSideSpeed = 2.8;
$RPGData_Goblin.maxYawSpeed = 10;
$RPGData_Goblin.maxPitchSpeed = 10;
$RPGData_Goblin.scale = "1 1 1";
$RPGData_Goblin.botDB = "PlayerStandardArmor";
$RPGData_Goblin.canJump = 1;






$RPGData_GoblinArcher.name = "Goblin Archer";
$RPGData_GoblinArcher.maxDamage = 80;
$RPGData_GoblinArcher.armor = 0;
$RPGData_GoblinArcher.resist = 0;
$RPGData_GoblinArcher.level = -500;
$RPGData_GoblinArcher.thinkTime = 1;
$RPGData_GoblinArcher.resetTargetTime = 4;

$RPGData_GoblinArcher.searchType = "RadiusLOSSearchType";

$RPGData_GoblinArcher.attackType = "GoblinRangedAttackType";

// $RPGData_Goblin.wanderSpeed = 10;
// $RPGData_Goblin.leashDistance = 10;
// $RPGData_Goblin.autoCutoffRange = 40;

$RPGData_GoblinArcher.botLogicFunc = "MRPGBot_simpleLogic";
$RPGData_GoblinArcher.botActionFunc = "MRPGBot_simpleAction";
$RPGData_GoblinArcher.damageCallbackFunc = "MRPGBot_simpleDamageCallback";

$RPGData_Goblin.expReward = "3 5";
$RPGData_Goblin.goldReward = "1 2";

$RPGData_GoblinArcher.maxForwardSpeed = 0;
$RPGData_GoblinArcher.maxBackwardSpeed = 0;
$RPGData_GoblinArcher.maxSideSpeed = 0;
$RPGData_GoblinArcher.maxYawSpeed = 5;
$RPGData_GoblinArcher.maxPitchSpeed = 5;
$RPGData_GoblinArcher.scale = "1 1 1";
$RPGData_GoblinArcher.botDB = "PlayerStandardArmor";



datablock ProjectileData(goblinSwordProjectile : swordProjectile)
{
	directDamage = 5;
	type = "Physical";
};

datablock ShapeBaseImageData(goblinSwordImage : swordImage)
{
	colorShiftColor = "0.47 0.33 0.33 1";
	projectile = goblinSwordProjectile;

	stateTimeoutValue[3]		= 2;
};




datablock ProjectileData(goblinArrowProjectile : arrowProjectile)
{
	directDamage = 2;
	type = "Physical";
};

datablock ShapeBaseImageData(goblinBowImage : BowImage)
{
	colorShiftColor = "0.53 0.43 0 1";
	projectile = goblinArrowProjectile;

	stateTimeoutValue[3]		= 1.8;
};