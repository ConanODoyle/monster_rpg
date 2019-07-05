//// structure ////
// %bot.RPGData = scriptobject with fields defining properties
// - etc...

// - botLogicFunc
// - botActionFunc
// - damageCallbackFunc

// - etc...

package MRPGBot_DamageCallback
{
	function Armor::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType)
	{
		if (isObject(%obj.RPGData))
		{
			%func = %obj.RPGData.damageCallbackFunc;
			if (isFunction(%func))
			{
				call(%func, %obj, %sourceObj, %pos, %damage, %damageType);
			}
		}
		return parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
	}
};
activatePackage(MRPGBot_DamageCallback);
deactivatePackage(Support_RPGDamage);
activatePackage(Support_RPGDamage); //ensure that modified damage values come through so that the callback gets the right amt