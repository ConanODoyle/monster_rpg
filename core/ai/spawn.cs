function MRPGBot_Spawn(%brick, %RPGData)
{
	if (!isObject($MRPG_AISet))
	{
		echo("ERROR (spawn.cs): AI Set nonexistent! Executing logic.cs...");
		talk("ERROR (spawn.cs): AI Set nonexistent! Executing logic.cs...");
		exec("./logic.cs");
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
	%brick.onBotSpawn();

	%bot.setTransform(%spawn);
	%bot.setMoveTolerance(0.5);
	%bot.spawnExplosion("spawnProjectile", getWord(%RPGData.scale, 2));

	$MRPG_AISet.add(%bot);
	%bot.nextThink = ($Sim::Time + 2 | 0) | 0;
}