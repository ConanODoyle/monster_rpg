registerOutputEvent(gameConnection, "addSkillEXP", "string 50 80	int 1 999999 1	bool false", false);

registerOutputEvent(fxDtsBrick, "checkSkillMoreThan", "string 50 80	int 1 999999 1", true);

registerInputEvent(fxDtsBrick, "onHasSufficientSkill", "Self fxDtsBrick	Player player	Client gameConnection	Minigame minigame");
registerInputEvent(fxDtsBrick, "onHasInsufficientSkill", "Self fxDtsBrick	Player player	Client gameConnection	Minigame minigame");


function getLevelEXP(%level)
{
	%answer = 0;

	for(%i = 1; %i < %level; %i++)
	{
		%answer += mFloor(%i + 300 * mpow(2, (%i / 7)));
	}

	return mFloor(%answer / 4);
}

function gameConnection::addSkillEXP(%this, %skillName, %exp, %announce)
{
	%this.currentSkill = %skillName;

	if(%this.skillEXP[%skillName] $= "")
	{
		%this.skills = %this.skills TAB %skillName;
		%this.skillLevel[%skillName] = 1;
	}

	%this.skillEXP[%skillName] += %exp;

	if(%announce)
		%this.chatMessage("\c6You have gained\c4" SPC %exp SPC "\c6experience in the\c4" SPC %skillName SPC "\c6skill.");

	while(%this.skillEXP[%skillName] >= getLevelEXP(%this.skillLevel[%skillName] + 1))
	{
		%this.skillLevel[%skillName]++;
		%this.onLevelUp(%skillName, %this.skillLevel[%skillName]);
		%this.chatMessage("\c6You've gained a level in the\c4" SPC %skillName SPC "\c6skill! You're now level\c4" SPC %this.skillLevel[%skillName] @ "\c6.");
	}

	showHud(%this);
}

function fxDtsBrick::checkSkillMoreThan(%this, %skillName, %level, %client)
{
	$InputTarget_["Self"]   = %this;
	$InputTarget_["Player"] = %client.player;
	$InputTarget_["Client"] = %client;

	if($Server::LAN)
	{
		$InputTarget_["MiniGame"] = getMiniGameFromObject(%client);
	}
	else
	{
		if(getMiniGameFromObject(%this) == getMiniGameFromObject(%client))
			$InputTarget_["MiniGame"] = getMiniGameFromObject(%this);
		else
			$InputTarget_["MiniGame"] = 0;
	}

	if(%client.skillLevel[%skillName] >= %level)
		%this.processInputEvent("onHasSufficientSkill", %client);
	else
		%this.processInputEvent("onHasInsufficientSkill", %client);
}

function gameConnection::onLevelUp(%this, %skill, %level)
{
	// This function exists just for packaging.
	// Probably handy if you want to implement more exquisite features of certain skills; like a combat skill.
}

function serverCmdSkills(%client)
{
	%max = getFieldCount(%client.skills);

	for(%i = 0; %i < %max; %i++)
	{
		%skill = getField(%client.skills, %i);
		if(%skill $= "")
			continue;

		%client.chatMessage("\c6" @ %skill SPC "Level\c4" SPC %client.skillLevel[%skill] SPC "\c6(\c4" @ %client.skillEXP[%skill] SPC "\c6/\c4" SPC getLevelEXP(%client.skillLevel[%skill] + 1) @ "\c6) EXP");
	}
}