package MRPG_RoundCheck 
{
	function GameConnection::onDeath(%cl, %sourceObject, %sourceClient, %damageType, %damageLoc)
	{
		schedule(1000, 0, checkGameOver, %cl);
		return parent::onDeath(%cl, %sourceObject, %sourceClient, %damageType, %damageLoc);
	}
};
activatePackage(MRPG_RoundCheck);




function sendPlayersToLobby()
{
	//respawn all players, send to lobby
}

function sendPlayersToShop()
{
	//let them pick classes and shop

	//initial level
	if ($MRPG_RoundNum == 1)
	{
		for (%i = 0; %i < ClientGroup.getCount(); %i++)
		{
			%cl = ClientGroup.getObject(%i);
			%cl.applyClassData("Swordsman");
			%cl.gold = 100;

			$MRPG_TotalGoldGiven = 100;
		}
	}

	//schedule sending them to arena

}

function sendPlayersToArena(%arena)
{
	//only send players who aren't already in arena
}

function stopArenaMobSpawner(%arena)
{
	//cancels spawning in an arena
}



//Round controls

function startRound(%arena)
{

}

function startArenaMobSpawner(%arena, %mobs)
{
	//uses %mobs scriptobject to start the spawning logic for %arena
}

function endRound(%arena)
{

}

function checkRoundOver(%arena, %mobs)
{

}



//Game Controls

function startGame() {
	//cancel all pending schedules
	cancel($masterSpawnSchedule);

	//reset round and player data
	$MRPG_GameRunning = 1;
	$MRPG_RoundNum = 1;
	$MRPG_CurrentArena = "";
	$MRPG_CurrentMobs = "";
	
	$MRPG_TotalGoldGiven = 0;
	$MRPG_TotalExpGiven = 0;

	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		ClientGroup.getObject(%i).resetLevel();
	}

	//send players to shop
	sendPlayersToShop()
}

function endGame()
{
	$MRPG_GameRunning = 0;
	sendPlayersToLobby();
}

function checkGameOver(%lastDied)
{
	if (!$MRPG_GameRunning)
	{
		return;
	}

	%lastDied.arena = "";

	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%cl = ClientGroup.getObject(%cl);
		if (%cl.currentArena $= $MRPG_CurrentArena) 
		{
			//at least one person still alive
			return;
		}
	}
}