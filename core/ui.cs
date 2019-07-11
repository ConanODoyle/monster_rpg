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
		if (%i * 1/%num >= %percent && !%bypass)
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
	%num = 10;

	%str = "\c6[\c5";
	for (%i = 0; %i < %num; %i++)
	{
		if (%i * 1/8 >= %percent && !%bypass)
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
	%levelExp = getLevelExp(%cl.level + 1) | 0;
	%expBars = getExpBars(%exp / %levelExp);

	if (%levelExp > 1000000)
	{
		%levelExp = mFloatLength((%levelExp | 0) / (1000000 | 0), 2) @ "M";
	}
	else if (%levelExp > 1000)
	{
		%levelExp = mFloatLength((%levelExp | 0) / (1000 | 0), 2) @ "K";
	}

	if (%exp > 1000000)
	{
		%exp = mFloatLength((%exp | 0) / (1000000 | 0), 2) @ "M";
	}
	else if (%exp > 1000)
	{
		%exp = mFloatLength((%exp | 0) / (1000 | 0), 2) @ "K";
	}

	%exp = "\c3Level " @ %cl.level + 0 @ " \c5" @ %exp @ "/" @ %levelExp;
	%gold = "<just:right>\c3Gold: " @ mFloor(%cl.score) @ " ";

	%cl.bottomprint("<font:Consolas:24>" @ %hpbars SPC %hp @ %gold @ "<br><just:left>" @ %expBars SPC %exp, -1, 0);
}

package MRPG_UIPackage
{
	function Armor::damage(%data, %obj, %sourceObject, %pos, %damage, %damageType)
	{
		%cl = %obj.client;
		%ret = parent::damage(%data, %obj, %sourceObject, %pos, %damage, %damageType);
		if (isObject(%cl) && %cl.getClassName() $= "GameConnection")
		{
			bottomprintInfo(%cl);
		}
		return %ret;
	}

	function GameConnection::spawnPlayer(%cl)
	{
		%ret = parent::spawnPlayer(%cl);
		schedule(1, 0, bottomprintInfo, %cl);
		return %ret;
	}

	function GameConnection::addExperience(%cl, %amt)
	{
		parent::addExperience(%cl, %amt);
		bottomprintInfo(%cl);
	}

	function GameConnection::levelUp(%cl, %amt)
	{
		parent::levelUp(%cl, %amt);
		bottomprintInfo(%cl);
	}

	function GameConnection::incScore(%cl, %score)
	{
		parent::incScore(%cl, %score);
		bottomprintInfo(%cl);
	}
};
activatePackage(MRPG_UIPackage);