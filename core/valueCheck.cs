package MRPG_ValueCheckPackage
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
		return %ret;
	}
}
schedule(100, 0, activatePackage, MRPG_ValueCheckPackage);