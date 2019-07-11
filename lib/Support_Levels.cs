function getLevelEXP(%level)
{
	%answer = 0;

	for(%i = 1; %i < %level; %i++)
	{
		%answer += mFloor(%i + 300 * mpow(2, (%i / 7)));
	}

	return mFloor(%answer / 4);
}

function GameConnection::addExperience(%cl, %amt)
{
	%amt = mFloor(%amt);
	%cl.exp += %amt;
	%levelExp = getLevelEXP(%cl.level + 1);
	if (%cl.exp >= %levelExp)
	{
		%cl.exp -= getLevelEXP(%cl.level + 1);
		%cl.levelUp();
	}
}

function GameConnection::levelUp(%cl)
{
	%cl.level++;
	if (isObject(%cl.player))
	{
		%pl = %cl.player;
		%pl.spawnExplosion("spawnProjectile", "1 1 1");
		%pl.setDamageLevel(%pl.getDamageLevel() - %pl.getDatablock().maxDamage * 0.1); //heal 10% on level up
	}
	%cl.playSound(rewardSound);
	%cl.maxDamage = 100 + (%cl.level - 1) * 10;
}