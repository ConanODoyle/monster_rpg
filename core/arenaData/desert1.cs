
$Arena_LocLevel[0, 1] = new ScriptObject(ArenaMobData) {
	stageCount = 2;
	
	stage0_Count = 5 SPC 8;
	stage0_CountScaleFactor = 1;
	stage0_type = "goblin";
	stage0_spawnBrickName = "_small _small _archer";
	stage0_timeoutValue = 0;

	stage1_Count = 3 SPC 5;
	stage1_CountScaleFactor = 1;
	stage1_type = "goblinArcher";
	stage1_spawnBrickName = "_archer _archer _small";
};

$Arena_LocLevel[0, 2] = new ScriptObject(ArenaMobData) {
	stageCount = 4;
	
	stage0_Count = 5 SPC 8;
	stage0_CountScaleFactor = 2;
	stage0_type = "goblin";
	stage0_spawnBrickName = "_small _small _archer";
	stage0_timeoutValue = 0;

	stage1_Count = 3;
	stage1_CountScaleFactor = 0;
	stage1_type = "goblinArcher";
	stage1_spawnBrickName = "_archer _archer _small";
	stage1_timeoutValue = 5;
	
	stage2_Count = 3;
	stage2_CountScaleFactor = 1;
	stage2_type = "goblin";
	stage2_spawnBrickName = "_small _small _archer";
	stage2_timeoutValue = 0;

	stage3_Count = 5 SPC 8;
	stage3_CountScaleFactor = 2;
	stage3_type = "goblinArcher";
	stage3_spawnBrickName = "_archer _archer _small";
};
