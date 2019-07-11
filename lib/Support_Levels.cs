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
	if (%cl.exp > %levelExp)
	{
		%cl.exp -= getLevelEXP(%cl.level + 1);
		%cl.levelUp();
	}
}

function GameConnection::levelUp(%cl)
{
	%cl.level++;
}

RegisterPersistenceVar("level", false, "");
RegisterPersistenceVar("exp", false, "");