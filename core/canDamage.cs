package MRPG_CanDamagePackage
{
	function minigameCanDamage(%obj, %target)
	{
		if (%obj.getType() != 0)
		{
			%cl1 = %obj.client;
		}
		else
		{
			%cl1 = %obj;
		}

		%cl2 = %target.client;
		if (isObject(%obj.RPGData) || isObject(%target.RPGData))
		{
			if (isObject(%cl1.minigame) || %cl1.enableMRPG 
				|| isObject(%cl2.minigame) || %cl2.enableMRPG)
			{
				return 1;
			}
		}
		return parent::minigameCanDamage(%obj, %target);
	}
};
activatePackage(MRPG_CanDamagePackage);