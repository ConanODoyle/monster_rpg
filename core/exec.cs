exec("./ui.cs");
exec("./canDamage.cs");
exec("./persistence.cs");
exec("./playerLevels.cs");
exec("./classes.cs");
// exec("./arena.cs");
// exec("./gameControl.cs");

echo("");
echo("--Executing AI--");
exec("./ai/logic.cs");
exec("./ai/callbacks.cs");
exec("./ai/search.cs");
exec("./ai/spawn.cs");
exec("./ai/reward.cs");

echo("");
echo("--Executing mobs--");
exec("./ai/mobs/goblin.cs");

echo("");
echo("--Executing weapons--");
exec("./weapons/rifles.cs");