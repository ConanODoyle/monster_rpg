//// structure ////
// %bot.RPGData = scriptobject with fields defining properties
// - etc...

// - botLogicFunc
// - botActionFunc
// - damageCallbackFunc

// - etc...

package MRPGBot_DamageCallbackPackage
{
	function Armor::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType)
	{
		if (isObject(%obj.RPGData))
		{
			%func = %obj.RPGData.damageCallbackFunc;
			if (isFunction(%func))
			{
				call(%func, %obj, %sourceObj, %pos, getWord(%damage, 1), %damageType);
			}
		}
		return parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
	}
};
activatePackage(MRPGBot_DamageCallbackPackage);
deactivatePackage(MRPG_DamagePackage);
activatePackage(MRPG_DamagePackage); //ensure that modified damage values come through so that the callback gets the right amt