package MRPG_PlayerLevelNullCheck
{
	function GameConnection::onClientEnterGame(%cl)
	{
		%ret = parent::onClientEnterGame(%cl);
		if (%cl.maxDamage $= "")
		{
			%cl.maxDamage = 100;
			%cl.armor = 0;
			%cl.resist = 0;
			%cl.level = 1;
			%cl.class = "None";
		}
		%cl.exp = %cl.exp | 0;
		return %ret;
	}

	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);
		if (%cl.maxDamage $= "")
		{
			%cl.maxDamage = 100;
			%cl.armor = 0;
			%cl.resist = 0;
			%cl.level = 1;
			%cl.class = "None";
		}
		%cl.exp = %cl.exp | 0;
		return %ret;
	}

	function GameConnection::levelUp(%cl, %amt) 
	{
		%cl.level += %amt;
		if (isObject(%cl.player))
		{
			%pl = %cl.player;
			%pl.spawnExplosion("spawnProjectile", "1 1 1");
			
			if (%amt > 0)
			{
				%pl.setDamageLevel(%pl.getDamageLevel() - %pl.getDatablock().maxDamage * 0.2); //heal 20% on level up
			}
		}

		if (%amt > 0) 
		{
			announce("\c2" @ %cl.name @ " has leveled up to \c4Level " @ %cl.level @ "\c2! (+" @ %amt @ ")");
			echo(getDateTime() @ " - " @ %cl.getPlayerName() @ " has leveled up to Level " @ %cl.level @ "! (+" @ %amt @ ")"));
		}
		else
		{
			announce("\c0" @ %cl.name @ "has leveled down to \c4Level " @ %cl.level @ "\c0! (" @ %amt @ ")");
			echo(getDateTime() @ " - " @ %cl.getPlayerName() @ " has leveled down to Level " @ %cl.level @ "! (" @ %amt @ ")"));
		}

		%cl.playSound(rewardSound);
		// %cl.maxDamage = 100 + (%cl.level - 1) * 10;
		%cl.applyClassData();
	}
};
schedule(100, 0, activatePackage, MRPG_PlayerLevelNullCheck);

function GameConnection::resetLevel(%cl)
{
	%cl.maxDamage = 100;
	%cl.armor = 0;
	%cl.resist = 0;
	%cl.level = 1;
	%cl.class = "None";
	%cl.exp = 0 | 0;
}

function GameConnection::applyClassData(%cl, %class)
{
	if (%class $= "")
	{
		%class = %cl.class;
	}
	else
	{
		%cl.class = %class;
	}

	if (%class $= "None" || !isObject($MRPGClass_[%class]))
	{
		messageClient(%cl, '', "\c2Your class has been set to \c4None\c2!")
		echo(getDateTime() @ " - " @ %cl.name @ " has had their class set to None");
		%level = %cl.level;
		%exp = %cl.exp | 0;
		%cl.resetLevel();
		%cl.level = %level;
		%cl.exp = %exp | 0;
		return;
	}

	messageClient(%cl, '', "\c2Your class has been set to \c4" @ %class @ "\c2!")
	echo(getDateTime() @ " - " @ %cl.name @ " has had their class to set to " @ %class);

	%classObj = $MRPGClass_[%class];

	eval(%cl @ ".maxDamage = " @ %classObj.maxDamage @ " + (" @ strReplace(%classObj.healthPerLevel, "x", %cl.level) @ ");");
	eval(%cl @ ".armor = " @ %classObj.armor @ " + (" @ strReplace(%classObj.armorPerLevel, "x", %cl.level) @ ");");
	eval(%cl @ ".resist = " @ %classObj.resist @ " + (" @ strReplace(%classObj.resistPerLevel, "x", %cl.level) @ ");");
}