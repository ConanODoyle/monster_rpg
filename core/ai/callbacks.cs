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
			call(%func, %obj, %sourceObj, %pos, %damage, %damageType);
		}
		return parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
	}
};
activatePackage(MRPGBot_DamageCallback);