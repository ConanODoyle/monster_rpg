//// structure ////
// %proj has fields assigned on creation, taken from the attacker
// - damage
// - level
// - penetration
// - type

// %bot.RPGData = scriptobject with fields defining properties
// - maxDamage
// - armor
// - resist
// - level

package MRPG_Damage
{
	function Armor::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType)
	{
		if (isObject(%obj.RPGData) && %sourceObj.getClassName() $= "Projectile")
		{
			%damage = getModifiedDamage(%obj, %sourceObj, %damage);
		}
		return parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
	}
};
activatePackage(MRPG_Damage);


function getModifiedDamage(%bot, %proj, %damage)
{
	%levDiff = getMax(0, %bot.RPGData.level - %proj.level);
	
	if (%levDiff < 100)
	{
		%physDef = %bot.RPGData.armor - %proj.penetration;
		%magicDef = %bot.RPGData.resist - %proj.penetration;
		%levelMod = (100 - %levDiff) / 100;
		%levelMod = %levelMod * mSqrt(%levelMod);
		//75 diff = 12.5% damage
		//50 diff = 35.36% damage
		//25 diff = 64.95% damage
		//15 diff = 78.37% damage
		//0 diff = 100% damage

		%rawDamage = %proj.damage $= "" ? %proj.getDatablock().directDamage : %proj.damage;
		%type = %proj.type $= "" ? %proj.getDatablock().type : %proj.type;

		if (%type $= "Magic")
		{
			%damage = %levelMod * getMax(%rawDamage * 0.05, %rawDamage - %magicDef);
		}
		else if (%type $= "Physical")
		{
			%damage = %levelMod * getMax(%rawDamage * 0.05, %rawDamage - %physDef);
		}
		else
		{
			%damage = %levelMod * %rawDamage;
		}
	}

	if (%bot.RPGData.maxDamage > 0)
	{
		if (%bot.damageFactor $= "")
		{
			%bot.damageFactor = %bot.getDatablock().maxDamage / %bot.RPGData.maxDamage;
		}
		%finalDamage = %damage * %bot.damageFactor * getWord(%bot.getScale(), 2);
	}
	return %finalDamage SPC %damage;
}