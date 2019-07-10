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

package MRPG_DamagePackage
{
	function Armor::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType)
	{
		if (isObject(%obj.RPGData) && %sourceObj.getClassName() $= "Projectile")
		{
			%damage = getModifiedBotDamage(%obj, %sourceObj, %damage);
		}
		else if (isObject(%obj.client))
		{
			%damage = getModifiedPlayerDamage(%obj.client, %proj, %damage); //TODO
		}
		return parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
	}
};
activatePackage(MRPG_DamagePackage);


function getModifiedBotDamage(%bot, %proj, %damage)
{
	%data = %bot.RPGData;
	%damage = getModifiedDamage(%proj, %data.level, %data.armor, %data.resist);

	if (%bot.damageFactor $= "")
	{
		if (%data.maxDamage > 0)
		{
			%bot.damageFactor = %bot.getDatablock().maxDamage / %data.maxDamage;
		}
		else
		{
			%bot.damageFactor = 1;
		}
	}
	%finalDamage = %damage * %bot.damageFactor * getWord(%bot.getScale(), 2);

	return %finalDamage SPC %damage;
}


function getModifiedPlayerDamage(%cl, %proj, %damage)
{
	%damage = getModifiedDamage(%proj, %cl.level, %cl.armor, %cl.resist);

	if (%cl.maxDamage > 0 && isObject(%cl.player))
	{
		%cl.damageFactor = %cl.player.getDatablock().maxDamage / %cl.maxDamage;
	}
	else
	{
		%cl.damageFactor = 1;
	}
	%finalDamage = %damage * %cl.damageFactor * getWord(%bot.getScale(), 2);

	return %finalDamage SPC %damage;
}

function getModifiedDamage(%proj, %level, %armor, %resist)
{
	%levDiff = getMax(0, %level - %proj.level);
	
	if (%levDiff < 100)
	{
		%physDef = %armor - %proj.penetration;
		%magicDef = %resist - %proj.penetration;
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
	return %damage;
}