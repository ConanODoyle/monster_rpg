// package MRPG_RoundCheck 
// {
// 	function GameConnection::onDeath(%cl, %sourceObject, %sourceClient, %damageType, %damageLoc)
// 	{
// 		schedule(1000, 0, checkGameOver, %cl);
// 		return parent::onDeath(%cl, %sourceObject, %sourceClient, %damageType, %damageLoc);
// 	}
// };
// activatePackage(MRPG_RoundCheck);




// function sendPlayersToLobby(%gameClientGroup)
// {
// 	//respawn all players, send to lobby
// }

// function sendPlayersToShop(%gameClientGroup)
// {
// 	//let them pick classes and shop
// 	echo(getDateTime() @ " - Sending players to shop...");

// 	%roundNum = %gameClientGroup.RoundNum;

// 	//initial level bonus
// 	if (%gameClientGroup.RoundNum == 1)
// 	{
// 		for (%i = 0; %i < %gameClientGroup.getCount(); %i++)
// 		{
// 			%cl = %gameClientGroup.getObject(%i);
// 			%cl.applyClassData("Swordsman");
// 			%cl.gold = 30;

// 			%gameClientGroup.TotalGoldGiven = 30;
// 		}
// 	}

// 	for (%i = 0; %i < %gameClientGroup.getCount(); %i++)
// 	{
// 		%cl = %gameClientGroup.getObject(%i);
// 		if (!isObject(%cl.player))
// 		{
// 			%cl.spawnPlayer(getArenaShopSpawn(%gameClientGroup.RoundNum));
// 		}
// 		else
// 		{
// 			%cl.player.setTransform(getArenaShopSpawn(%gameClientGroup.RoundNum));
// 		}

// 		%cl.player.setDamageLevel(0);
// 		%cl.gold += 20;

// 		%gameClientGroup.TotalGoldGiven += 20;
// 	}

// 	//schedule sending them to arena
// 	messageAll('MsgUploadStart', "\c2Shop is open for 1 minute!");
// 	schedule((60 - 30) * 1000, MissionCleanup, messageAll, 'MsgUploadEnd', "\c630 seconds remaining!");
// 	schedule((60 - 10) * 1000, MissionCleanup, messageAll, 'MsgUploadEnd', "\c610 seconds remaining!");

// 	for (%i = 1; %i <= 5; %i++)
// 	{
// 		schedule((60 - %i) * 1000, MissionCleanup, messageAll, 'MsgUploadEnd', "\c6" @ %i @ " seconds remaining!");
// 	}

// 	schedule(60 * 1000, MissionCleanup, sendPlayersToArena, %gameClientGroup);
// }

// function sendPlayersToArena(%gameClientGroup)
// {
// 	// send all players to arena
// 	echo(getDateTime() @ " - Sending players to arena...");

// 	%roundNum = %gameClientGroup.RoundNum;

// 	for (%i = 0; %i < %gameClientGroup.getCount(); %i++)
// 	{
// 		%cl = %gameClientGroup.getObject(%i);
// 		if (!isObject(%cl.player))
// 		{
// 			%cl.spawnPlayer(getArenaShopSpawn(%roundNum));
// 		}
// 		else
// 		{
// 			%cl.player.setTransform(getArenaShopSpawn(%roundNum));
// 		}
// 	}

// 	//hand off control to startRound()
// 	startRound(%gameClientGroup);
// }



// //Round controls

// function startRound(%gameClientGroup)
// {
// 	echo(getDateTime() @ " - Starting round " @ %roundNum @ "...");

// 	%roundNum = %gameClientGroup.RoundNum;

// 	startArenaMobSpawner($Arena_Loc[getArenaID(%roundNum)], getArenaMobData(%roundNum), %gameClientGroup);	
// }

// function startArenaMobSpawner(%arenaName, %mobs, %gameClientGroup)
// {
// 	echo(getDateTime() @ " - Starting Arena Mob Spawner: " @ %arenaName @ ", " @ %mobs @ "...");
// 	//uses %mobs scriptobject to start the spawning logic for %gameClientGroup

// 	//collect spawnbrick groups
// 	%names = getArenaMobSpawners(%arenaName);
// 	%allGroupNames = "";
// 	for (%i = 0; %i < getWordCount(%names); %i++)
// 	{
// 		%spawnName = %arenaName @ getWord(%names, %i);
// 		%brickName = "_" @ %spawnName;
// 		%group = %spawnName @ "_SimSet";
// 		%allGroupNames = %allGroupNames SPC %group;
// 		if (!isObject(%group))
// 		{
// 			%group = new SimSet(%group) {}; 
// 		}
// 		else
// 		{
// 			%group.clear();
// 		}

// 		%count = 0;
// 		while (isObject(%brickName))
// 		{
// 			%temp[%count] = %brickName.getID();
// 			%group.add(%temp[%count]);
// 			%count++;
// 			%brickName.setName("");
// 		}

// 		for (%j = 0; %j < %count; %j++)
// 		{
// 			%temp[%j].setName(%brickName);
// 		}
// 	}
// 	echo(getDateTime() @ " - Generated groups for bricks [" @ trim(%allGroupNames) @ "]");

// 	//start mob spawning logic
// 	echo(getDateTime() @ " - Starting mob spawner logic for " @ %arenaName @ ", " @ %mobs @ "...");

// 	mobSpawnerLogic(%mobs, 0, %allGroupNames, %arenaName, %gameClientGroup);
// }

// function mobSpawnerLogic(%mobData, %stage, %arenaName, %gameClientGroup)
// {
// 	cancel($ArenaMobSpawnerSched_[%arenaName]);

// 	%totalStages = %mobData.stageCount;
// 	if (%stage >= %totalStages)
// 	{
// 		//continue checking for stage win condition

// 		// $ArenaMobSpawnerSched_[%arenaName] = schedule(100, %mobData, mobSpawnerLogic, %mobData, %stage, %arenaName, %roundNum);

// 		//call endround if no mobs alive
// 		return;
// 	}

// 	%mobCount = %mobData.stage[%stage, "Count"];
// 	if (getWordCount(%mobCount) > 1)
// 	{
// 		%mobCount = getRandom(getWord(%mobCount, 0), getWord(%mobCount, 1));
// 	}
// 	%mobScaleFactor = %mobData.stage[%stage, "CountScaleFactor"];
// 	%mobCount = %mobCount + (%gameClientGroup.getCount() - 1) * %mobScaleFactor;

// 	%mobType = %mobData.stage[%stage, "type"];
// 	%mobSpawns = %mobData.stage[%stage, "spawnBrickName"];

// 	%stageTimeout = %mobData.stage[%stage, "timeoutValue"];

// 	//get spawn points
// 	%spawnBricks = new SimSet(){};
// 	%usedBricks = new SimSet(){};
// 	for (%i = 0; %i < getWordCount(%mobSpawns); %i++)
// 	{
// 		%groupName = %arenaName @ getWord(%mobSpawns, %i) @ "_SimSet";
// 		if (isObject(%groupName))
// 		{
// 			for (%j = 0; %j < %groupName.getCount(); %j++)
// 			{
// 				%spawnBricks.add(%groupName.getObject(%j));
// 			}
// 		}
// 	}

// 	//spawn mobs
// 	for (%i = 0; %i < %mobCount; %i++)
// 	{
// 		if (%spawnBricks.getCount() <= 0)
// 		{
// 			%temp = %spawnBricks;
// 			%spawnBricks = %usedBricks;
// 			%usedBricks = %temp;
// 		}
// 		%spawnPoint = %spawnBricks.remove(getRandom(%spawnBricks.getCount() - 1));

// 		MRPGBot_Spawn(%spawnPoint, $RPGData_[%mobType]);
// 	}

// 	$ArenaMobSpawnerSched_[%arenaName] = schedule(100, %mobData, mobSpawnerLogic, %mobData, %stage, %arenaName, %roundNum);
// }

// function stopArenaMobSpawner(%arenaName)
// {
// 	cancel($ArenaMobSpawnerSched_[%arenaName]);

// 	echo(getDateTime() @ " - Stopping mob spawner logic for " @ %arenaName @ "...");

// 	//cancels spawning in an arena
// }

// function endRound(%roundNum)
// {

// }

// function checkRoundOver(%arenaData, %mobs)
// {

// }



// //Game Controls

// function startGame(%gameClientGroup) {
// 	//cancel all pending schedules
// 	cancel($masterSpawnSchedule);

// 	//reset round and player data
// 	%gameClientGroup.GameRunning = 1;
// 	%gameClientGroup.RoundNum = 1;
// 	%gameClientGroup.CurrentArena = "";
// 	%gameClientGroup.CurrentMobs = "";
	
// 	%gameClientGroup.TotalGoldGiven = 0;
// 	%gameClientGroup.TotalExpGiven = 0;

// 	for (%i = 0; %i < %gameClientGroup.getCount(); %i++)
// 	{
// 		%gameClientGroup.getObject(%i).resetLevel();
// 	}

// 	//send players to shop
// 	sendPlayersToShop();
// }

// function endGame()
// {
// 	%gameClientGroup.GameRunning = 0;
// 	sendPlayersToLobby();
// }

// function checkGameOver(%lastDied)
// {
// 	if (!%gameClientGroup.GameRunning)
// 	{
// 		return;
// 	}

// 	%lastDied.arena = "";
// 	%gameClientGroup = %lastDied.gameClientGroup;

// 	for (%i = 0; %i < %gameClientGroup.getCount(); %i++)
// 	{
// 		%cl = %gameClientGroup.getObject(%cl);
// 		if (%cl.currentArena $= %gameClientGroup.CurrentArena) 
// 		{
// 			//at least one person still alive
// 			return;
// 		}
// 	}
// }