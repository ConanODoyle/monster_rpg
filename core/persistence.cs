if (isFile("Add-ons/Script_Player_Persistence/server.cs"))
{
	exec("Add-ons/Script_Player_Persistence/server.cs");
}

RegisterPersistenceVar("level", false, "");
RegisterPersistenceVar("exp", false, "");

RegisterPersistenceVar("maxDamage", false, "");
RegisterPersistenceVar("armor", false, "");
RegisterPersistenceVar("resist", false, "");
RegisterPersistenceVar("class", false, "");