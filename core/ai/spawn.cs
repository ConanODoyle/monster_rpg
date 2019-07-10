function MRPGBot_Spawn(%brick, %RPGData)
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

	%pos = vectorAdd(%brick.getPosition(), "0 0 " @ getWord(%RPGData.scale, 2) * 3 / 2);
	%rot = getWords(%brick.getSpawnPoint(), 3, 6);
	%spawn = %pos SPC %rot;

	%bot = new AIPlayer(MRPGBot) {
		dataBlock = isObject(%RPGData.botDB) ? %RPGData.botDB : "PlayerStandardArmor";
		RPGData = %RPGData;
		spawnBrick = %brick;
		maxYawSpeed = %RPGData.maxYawSpeed;
		maxPitchSpeed = %RPGData.maxPitchSpeed;
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

	$MRPG_AISet.add(%bot);
	%bot.nextThink = ($Sim::Time + 3 | 0) | 0;
}




if (!isObject(MRPG_SpawnSet))
{
	$MRPG_SpawnSet = new SimSet(MRPG_SpawnSet) {};
	MissionCleanup.add($MRPG_SpawnSet);
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

		%name = strReplace(getSubStr(%curr, 1, 100), "_", " ");
		%dataType = getWord(%name, 0);
		%time = getMax(1, getWord(%name, 1));
		if (isObject(%data = $RPGData_[%dataType]))
		{
			if (isObject(%curr.hBot))
			{
				%curr.lastHadBot = $Sim::Time;
			}
			else if ($Sim::Time - %curr.lastHadBot > %time)
			{
				MRPGBot_Spawn(%curr, %data);
				%curr.lastHadBot = $Sim::Time;
			}
		}

		%idx++;
	}

	$masterSpawnSchedule = schedule(1, 0, logicTick, %idx);
}

package MRPG_SpawnPackage 
{
	function fxDTSBrickData::onAdd(%this, %obj)
	{
		if (%this.isMRPGSpawn && %obj.isPlanted)
		{
			MRPG_SpawnSet.add(%obj);
		}
		return parent::onAdd(%this, %obj);
	}
};
activatePackage(MRPG_SpawnPackage);