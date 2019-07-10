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
		if (%damageType == $DamageType::Radius || %damageType == %sourceObj.getDatablock().radiusDamageType)
		{
			%isRadius = 1;
		}

		if (isObject(%obj.RPGData) && %sourceObj.getClassName() $= "Projectile")
		{
			%damage = getModifiedBotDamage(%obj, %sourceObj, %damage, %isRadius);
		}
		else if (isObject(%obj.client))
		{
			%damage = getModifiedPlayerDamage(%obj.client, %sourceObj, %damage, %isRadius); //TODO
		}
		return parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
	}
};
activatePackage(MRPG_DamagePackage);


function getModifiedBotDamage(%bot, %proj, %damage, %isRadius)
{
	%data = %bot.RPGData;
	%damage = getModifiedDamage(%proj, %data.level, %data.armor, %data.resist, %damage, %isRadius);

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
	%finalDamage = %damage * %bot.damageFactor * getWord(%bot.getScale(), 2); //negates scale dmg reduction in vanilla code

	return %finalDamage SPC %damage;
}


function getModifiedPlayerDamage(%cl, %proj, %damage, %isRadius)
{
	if (!isObject(%cl.player))
	{
		return %damage;
	}

	%damage = getModifiedDamage(%proj, %cl.level, %cl.armor, %cl.resist, %damage, %isRadius);

	if (%cl.maxDamage > 0)
	{
		%cl.damageFactor = %cl.player.getDatablock().maxDamage / %cl.maxDamage;
	}
	else
	{
		%cl.damageFactor = 1;
	}
	%finalDamage = %damage * %cl.damageFactor * getWord(%cl.player.getScale(), 2); //negates scale dmg reduction in vanilla code

	return %finalDamage SPC %damage;
}

function getModifiedDamage(%proj, %level, %armor, %resist, %damage, %isRadius)
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

		%projDB = %proj.getDatablock();
		if (%isRadius)
		{
			%rawDamage = %damage;
		}
		else
		{
			%rawDamage = %proj.damage $= "" ? (%projDB.directDamage ? %projDB.directDamage : %damage) : %proj.damage;
		}
		%type = %proj.type $= "" ? %projDB.type : %proj.type;

		// talk("levDiff: " @ %levDiff);
		// talk("physDef: " @ %physDef);
		// talk("magicDef: " @ %magicDef);
		// talk("levelMod: " @ %levelMod);
		// talk("rawDamage: " @ %rawDamage SPC " proj:["@ %proj.damage @ "," @ %proj.getDatablock().directDamage @ "]");

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