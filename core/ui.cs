function getHPBars(%cl)
{
	%pl = %cl.player;
	%num = 10;
	if (isObject(%pl))
	{
		%percent = 1 - %pl.getDamagePercent();
	}
	else
	{
		%str = "\c7[\c8";
		for (%i = 0; %i < %num; %i++)
		{
			%str = %str @ "|";
		}
		return %str @ "\c7]";
	}

	%str = "\c6[\c2";
	for (%i = 0; %i < %num; %i++)
	{
		if (%i * %num/100 > %percent && !%bypass)
		{
			%str = %str @ "\c7";
			%bypass = 1;
		}
		%str = %str @ "|";
	}
	%str = %str @ "\c6]";
	return %str;
}

function getExpBars(%percent)
{
	%num = 8;

	%str = "\c6[\c5";
	for (%i = 0; %i < %num; %i++)
	{
		if (%i * %num/100 > %percent && !%bypass)
		{
			%str = %str @ "\c7";
			%bypass = 1;
		}
		%str = %str @ "/";
	}
	%str = %str @ "\c6]";
	return %str;
}

function bottomprintInfo(%cl)
{
	%hpBars = getHPBars(%cl);
	if (!isObject(%cl.player))
	{
		%hp = "\c00\c6/" @ %cl.maxDamage + 0; 
	}
	else
	{
		%pl = %cl.player;
		%db = %pl.getDatablock();
		%percent = 1 - %pl.getDamagePercent();
		%hp = getMax(1, mFloor(%percent * %cl.maxDamage));
		if (%percent < 0.33)
		{
			%color = "\c0";
		}
		else if (%percent < 0.75)
		{
			%color = "\c3";
		}
		else
		{
			%color = "\c2";
		}
		%hp = %color @ %hp @ "\c6/" @ %cl.maxDamage + 0;
	}

	%exp = %cl.exp;
	%levelExp = getLevelExp(%cl.level + 1);
	%expBars = getExpBars(%exp / %levelExp);

	%exp = "\c3Level " @ %cl.level + 0 @ " \c5" @ %exp + 0 @ "/" @ %levelExp + 0;

	%cl.bottomprint("<font:Consolas:24>" @ %hpbars SPC %hp @ "<just:right>" @ %exp SPC %expBars, -1, 0);
}

package MRPG_UIPackage
{
	function Armor::damage(%data, %obj, %sourceObject, %pos, %damage, %damageType)
	{
		%cl = %obj.client;
		%ret = parent::damage(%data, %obj, %sourceObject, %pos, %damage, %damageType);
		if (isObject(%cl))
		{
			bottomprintInfo(%cl);
		}
		return %ret;
	}

	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);
		bottomprintInfo(%cl);
		return %ret;
	}

	function GameConnection::addExperience(%cl, %amt)
	{
		parent::addExperience(%cl, %amt);
		bottomprintInfo(%cl);
	}

	function GameConnection::levelUp(%cl)
	{
		parent::levelUp(%cl);
		bottomprintInfo(%cl);
	}
};
activatePackage(MRPG_UIPackage);