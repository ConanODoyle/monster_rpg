function getLevelEXP(%level)
{
	%answer = 0;

	for(%i = 1; %i < %level; %i++)
	{
		%answer += mFloor(%i + 300 * mpow(2, (%i / 35)));
	}

	return mFloor(%answer / 4);
}

function GameConnection::addExperience(%cl, %amt)
{
	%amt = mFloor(%amt);
	%cl.exp = ((%cl.exp | 0) + %amt | 0) | 0;
	%levelExp = getLevelEXP(%cl.level + 1) | 0;
	while (%cl.exp >= %levelExp)
	{
		%cl.exp = ((%cl.exp | 0) - %levelExp | 0);
		%cl.levelUp(1);
		%levelExp = getLevelEXP(%cl.level + 1) | 0;
	}
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
			%pl.setDamageLevel(%pl.getDamageLevel() - %pl.getDatablock().maxDamage * 0.1); //heal 10% on level up
		}
	}
	%cl.playSound(rewardSound);
	%cl.maxDamage = 100 + (%cl.level - 1) * 10;
}