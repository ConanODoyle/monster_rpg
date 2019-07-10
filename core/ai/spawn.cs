function MRPGBot_Spawn(%brick, %RPGData, %disableAI)
{
	if (!isObject($MRPG_AISet))
	{
		echo("ERROR (spawn.cs): AI Set nonexistent! Executing logic.cs...");
		talk("ERROR (spawn.cs): AI Set nonexistent! Executing logic.cs...");
		exec("./logic.cs");
	}
	else if (isObject(%brick.hBot))
	{
		%brick.hBot.delete();
	}

	%pos = vectorAdd(%brick.getPosition(), "0 0 1"); // @ getWord(%RPGData.scale, 2) * 3 / 2);
	%rot = getWords(%brick.getSpawnPoint(), 3, 6);
	%spawn = %pos SPC %rot;

	%bot = new AIPlayer(MRPGBot) {
		dataBlock = isObject(%RPGData.botDB) ? %RPGData.botDB : "PlayerStandardArmor";
		RPGData = %RPGData;
		spawnBrick = %brick;
		maxYawSpeed = %RPGData.maxYawSpeed;
		maxPitchSpeed = %RPGData.maxPitchSpeed;
		canJump = %RPGData.canJump;
		spawnBrick = %brick;
		name = %RPGData.name;
	};
	%bot.setMaxForwardSpeed(%RPGData.maxForwardSpeed);
	%bot.setMaxBackwardSpeed(%RPGData.maxBackwardSpeed);
	%bot.setMaxSideSpeed(%RPGData.maxSideSpeed);
	%bot.setScale(%RPGData.scale);
	%bot.mountImage(%RPGData.attackType.attackImage, 0);

	%brick.hBot = %bot;
	%bot.isBot = 1;
	%bot.client = %bot;
	%bot.player = %bot;
	%brick.onBotSpawn();

	%bot.setTransform(%spawn);
	%bot.setMoveTolerance(0.5);
	%bot.spawnExplosion("spawnProjectile", getWord(%RPGData.scale, 2));

	if (!%disableAI)
	{
		$MRPG_AISet.add(%bot);
		%bot.nextThink = ($Sim::Time + 3 | 0) | 0;
	}
}




if (!isObject(MRPG_SpawnSet))
{
	$MRPG_SpawnSet = new SimSet(MRPG_SpawnSet) {};
	schedule(1000, 0, eval, "MissionCleanup.add($MRPG_SpawnSet);");
}


function spawnTick(%idx)
{
	cancel($masterSpawnSchedule);
	if (!isObject(MRPG_SpawnSet))
	{
		return;
	}

	%count = MRPG_SpawnSet.getCount();

	if (%idx >= %count)
	{
		%idx = 0;
	}

	for (%i = 0; %i < 16; %i++)
	{
		if (%idx >= %count)
		{
			break;
		}
		%curr = MRPG_SpawnSet.getObject(%idx);

		%name = strReplace(getSubStr(%curr.getName(), 1, 100), "_", " ");
		if (%name $= "")
		{
			%idx++;
			continue;
		}

		%dataType = getWord(%name, 0);
		%time = getMax(1, getWord(%name, 1));
		%disableAI = getWord(%name, 2);
		if (isObject(%data = $RPGData_[%dataType]))
		{
			if (isObject(%curr.hBot))
			{
				%curr.lastHadBot = $Sim::Time;
			}
			else if ($Sim::Time - %curr.lastHadBot > %time)
			{
				MRPGBot_Spawn(%curr, %data, %disableAI);
				%curr.lastHadBot = $Sim::Time;
			}
		}

		%idx++;
	}

	$masterSpawnSchedule = schedule(1, 0, spawnTick, %idx);
}

package MRPGBot_SpawnPackage 
{
	function fxDTSBrickData::onAdd(%this, %obj)
	{
		if (%this.isMRPGSpawn && %obj.isPlanted)
		{
			MRPG_SpawnSet.add(%obj);
		}

		return parent::onAdd(%this, %obj);
	}

	function serverCmdSetWrenchData(%cl, %data)
	{
		%brick = %client.wrenchBrick;
		if (isObject(%brick))
		{
			%name = %brick.getName();
		}

		%ret = parent::serverCmdSetWrenchData(%cl, %data);

		if (isObject(%brick) && %brick.getName() !$= %name && isObject(%brick.hBot))
		{
			%brick.hBot.delete();
			%cl.centerprint("Deleted Monster RPG Bot due to brick name change");
		}

		return %ret;
	}
};
activatePackage(MRPGBot_SpawnPackage);



datablock fxDTSBrickData (BrickMRPGBotSpawn_SmallData)
{
	brickFile = "Add-ons/Bot_Hole/4xspawn.blb";
	category = "Special";
	subCategory = "Monster RPG";
	uiName = "Small Monster Spawn";
	iconName = "";

	orientationfix = 1;
	indestructable = 1;

	isMRPGSpawn = 1;
};

datablock fxDTSBrickData (BrickMRPGBotSpawn_MediumData)
{
	brickFile = "Add-ons/Bot_Hole/6xspawn.blb";
	category = "Special";
	subCategory = "Monster RPG";
	uiName = "Medium Monster Spawn";
	iconName = "";

	orientationfix = 1;
	indestructable = 1;

	isMRPGSpawn = 1;
};

datablock fxDTSBrickData (BrickMRPGBotSpawn_LargeData)
{
	brickFile = "Add-ons/Bot_Hole/8xspawn.blb";
	category = "Special";
	subCategory = "Monster RPG";
	uiName = "Large Monster Spawn";
	iconName = "";

	orientationfix = 1;
	indestructable = 1;

	isMRPGSpawn = 1;
};
