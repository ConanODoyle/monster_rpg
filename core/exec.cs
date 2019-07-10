echo("");
echo("--Executing AI--");
exec("./ai/logic.cs");
exec("./ai/callbacks.cs");
exec("./ai/search.cs");
exec("./ai/spawn.cs");

echo("");
echo("--Executing mobs--");
exec("./ai/mobs/goblin.cs");

// echo("");
// echo("--Executing weapons--");