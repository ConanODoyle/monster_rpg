if (!isObject($RPGData_Goblin))
{
	$RPGData_Goblin = new ScriptObject(RPGDataObject) {};
	MissionCleanup.add($RPGData_Goblin);
}

if (!isObject($RPGData_GoblinAttack))
{
	$RPGData_GoblinAttack = new ScriptObject(GoblinAttackType) {
		attackImage = "SwordImage";
		triggerTime = "1 1";
		minRange = 0;
		maxRange = 4;
		stopRange = 1;
	};

	$RPGData_GoblinRangedAttack = new ScriptObject(GoblinRangedAttackType) {
		attackImage = "bowImage";
		triggerTime = "1 1";
		minRange = 6;
		maxRange = 15;
		stopRange = 9;
		backingAttack = 1;
	};

	MissionCleanup.add($RPGData_GoblinAttack);
	MissionCleanup.add($RPGData_GoblinRangedAttack);
}

$RPGData_Goblin.name = "Goblin";
$RPGData_Goblin.maxDamage = 80;
$RPGData_Goblin.armor = 0;
$RPGData_Goblin.resist = 0;
$RPGData_Goblin.level = -500;

$RPGData_Goblin.searchType = "NearRadiusLOSSearchType";

$RPGData_Goblin.attackType = "GoblinAttackType";

$RPGData_Goblin.botLogicFunc = "MRPG_simpleLogic";
$RPGData_Goblin.botActionFunc = "MRPG_simpleAction";
$RPGData_Goblin.damageCallbackFunc = "MRPG_simpleDamageCallback";

$RPGData_Goblin.expReward = 10;

$RPGData_Goblin.maxForwardSpeed = 4.5;
$RPGData_Goblin.maxBackwardSpeed = 2.8;
$RPGData_Goblin.maxSideSpeed = 2.8;
$RPGData_Goblin.maxYawSpeed = 5;
$RPGData_Goblin.maxPitchSpeed = 10;
$RPGData_Goblin.scale = "1 1 1";
$RPGData_Goblin.botDB = "PlayerStandardArmor";