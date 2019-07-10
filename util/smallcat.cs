package SmallCat
{
	function serverCmdMessageSent(%cl, %msg)
	{
		if (strPos(strlwr(%msg), "meow") >= 0)
		{
			if (%cl.meowComboReset - $Sim::Time < 0)
			{
				%cl.meowCombo = 0;
			}
			%cl.meowComboReset = $Sim::Time + 2;
			%cl.meowCombo++;
			if (%cl.meowCombo > 2)
			{
				%str = "MEOW";
				for (%i = 0; %i < %cl.meowCombo - 2; %i++)
				{
					%str = %str @ "!";
				}
			}
			else
			{
				%str = "meow";
			}
			schedule(1000, 0, chatMessageAll, '', "\c3Small Cat\c6: " @ %str);
		}
		else if (strPos(strlwr(%msg), "where is pecon") >= 0)
		{
			if (findClientByName("Pecon"))
			{
				schedule(1000, 0, chatMessageAll, '', "\c3Small Cat\c6: Pecon can currently be found in her natural habitat, on this server.");
			}
			else
			{
				if (%cl.name $= "Redconer")
				{
					schedule(1000, 0, chatMessageAll, '', "\c3Small Cat\c6: (\\/)! !(\\/) PECON IS PEGONE (\\/)! !(\\/)");
				}
				else
				{
					schedule(1000, 0, chatMessageAll, '', "\c3Small Cat\c6: Pecon is currently missing.");
				}
			}
		}
		parent::serverCmdMessageSent(%cl, %msg);
	}

	function spamAlert(%cl)
	{
		if (strPos(%cl.lastChatText, "meow") >= 0)
		{
			return 0;
		}
		return parent::spamAlert(%cl);
	}
};
activatePackage(SmallCat);
deactivatePackage(chatEval);
activatePackage(chatEval);