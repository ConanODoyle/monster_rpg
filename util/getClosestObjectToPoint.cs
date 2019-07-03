function getClosestObjectToPoint(%objs, %pos)
{
	for (%i = 0; %i < getWordCount(%objs); %i++)
	{
		%curr = getWord(%objs, %i);
		if (isObject(%curr))
		{
			%currDist = vectorDist(%pos, %curr.getPosition());
			if (%currDist < %dist || !%hasFoundOnce)
			{
				%dist = %currDist;
				%best = %curr;
				%hasFoundOnce = 1;
			}
		}
	}
	
	return %best SPC %dist;
}

