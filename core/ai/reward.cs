package MRPGBot_KillRewardPackage
{
	function Armor::damage(%data, %obj, %sourceObject, %pos, %damage, %damageType)
	{
		%obj.lastDamagedBy = %sourceObject;
		%obj.lastDamagedByClient = getBrickgroupFromObject(%sourceObject).client;
		return parent::damage(%data, %obj, %sourceObject, %pos, %damage, %damageType);
	}

	function Armor::onDisabled(%data, %obj, %state)
	{
		if (isObject(%obj.RPGData) && isObject(%obj.lastDamagedByClient))
		{
			%data = %obj.RPGData;
			%recipient = %obj.lastDamagedByClient;
			%goldReward = getRandom(getWord(%data.goldReward, 0), getWord(%data.goldReward, 1));
			%expReward = getRandom(getWord(%data.expReward, 0), getWord(%data.expReward, 1));

			%recipient.addScore(%goldReward);
			%recipient.addExperience(%expReward);
		}
		return parent::onDisabled(%data, %obj, %state);
	}
};
activatePackage(MRPGBot_KillRewardPackage);