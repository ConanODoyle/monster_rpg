package MRPG_CanDamagePackage
{
	function minigameCanDamage(%obj, %target)
	{
		%cl1 = %obj.client;
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