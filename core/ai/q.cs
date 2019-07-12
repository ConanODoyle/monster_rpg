//Basic Q-learning to determine what weapon to use based on distance from target
//State: 
// - Bot name
// - Dist from target
// - in the future: Image? May add too much depth to analysis to develop a full Q table
//Q values correlate with the ideal weapon choice out of the current weapons, given the state

function getCurrentQState(%bot)
{
	%target = %bot.target;
	%targetPos = %target.position;
	if (%targetPos $= "")
	{
		return 0;
	}
	%botPos = %bot.position;

	%data = %bot.RPGData;
	%dist = mFloor(vectorDist(%targetPos, %botPos) / 4) * 4;

	%state = %data.name @ "_" @ %dist;
	return %state;
}

function getBestQChoice(%bot, %images)
{
	%state = getCurrentQState(%bot);
	if (%state $= "")
	{
		return "";
	}

	%learnedVals = $Q_[%state];
	%imageWC = getWordCount(%images);
	%valWC = getWordCount(%learnedVals);
	if (%imageWC != %valWC)
	{
		echo("Q Learning: Image list provided does not match vals, discarding learned values...");
		$Q_[%state] = "";
		for (%i = 0; %i < %imageWC; %i++)
		{
			%learnedVals = 0 SPC %learnedVals;
		}
		$Q_[%state] = %learnedVals = trim(%learnedVals);
	}
	%bestIDX = -1;
	for (%i = 0; %i < getWordCount(%learnedVals); %i++)
	{
		%curr = getWord(%learnedVals, %i);
		if (%bestIDX == -1)
		{
			%bestIDX = %i;
			%bestQVal = %curr;
		}
		else if (%bestQVal == %curr)
		{
			%bestIDX = %bestIDX SPC %i;
		}
		else if (%curr > %bestQVal)
		{
			%bestIDX = %i;
			%bestQVal = %curr;
		}
	}
	%choice = getWord(%bestIDX, getRandom(getWordCount(%bestIDX) - 1));
	return getWord(%images, %choice) TAB %choice TAB %state;
}

function rewardQChoice(%bot, %state, %idx, %value)
{
	%learnedVals = $Q_[%state];
	for (%i = 0; %i < getWordCount(%learnedVals); %i++)
	{
		%curr = getWord(%learnedVals, %i);
		if (%curr == %idx)
		{
			%curr = getMax(-10000, getMin(10000, %curr + %value));
		}
		%newVals = %newVals SPC %curr;
	}
	$Q_[%state] = trim(%newVals);
}

function getBestQValue(%state)
{
	%learnedVals = $Q_[%state];
	for (%i = 0; %i < getWordCount(%learnedVals); %i++)
	{
		%curr = getWord(%learnedVals, %i);
		if (%best $= "" || %best < %curr)
		{
			%best = %curr;
		}
	}
	return %best;
}