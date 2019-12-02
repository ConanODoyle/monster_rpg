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