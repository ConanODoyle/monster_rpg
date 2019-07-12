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
			if (getWordCount(%data.goldReward) > 1)
			{
				%goldReward = getRandom(getWord(%data.goldReward, 0), getWord(%data.goldReward, 1));
			}
			else
			{
				%goldReward = %data.goldReward;
			}

			if (getWordCount(%data.expReward) > 1)
			{
				%expReward = getRandom(getWord(%data.expReward, 0), getWord(%data.expReward, 1));
			}
			else
			{
				%expReward = %data.expReward;
			}

			%recipient.incScore(%goldReward);
			%recipient.addExperience(%expReward);
		}
		return parent::onDisabled(%data, %obj, %state);
	}

	function AIPlayer::onDeath(%bot, %killer, %killerClient, %damageType, %damageLoc)
	{
		//fix console error of bots dying
		return;
		// return parent::onDeath(%bot, %killer, %killerClient, %damageType, %damageLoc);
	}
};
activatePackage(MRPGBot_KillRewardPackage);