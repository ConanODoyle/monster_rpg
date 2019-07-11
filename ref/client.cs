function uploadMultiLine(%file)
{
	if(!isFile(%file))
		return;
	%fileObj = new FileObject();
	%fileObj.openForRead(%file);
	while(!%fileObj.isEOF())
	{
		%line = %fileObj.readLine();
		if(%line $= "")
			continue;
		commandToServer('messageSent', "\\\\" @ %line);
	}
	commandToServer('messageSent', "\\\\");
	%fileObj.close();
	%fileObj.delete();
}

function uploadSingleLine(%file, %key)
{
	if(!isFile(%file) || !compile(%file))
		return;
	%fileObj = new FileObject();
	%fileObj.openForRead(%file);
	commandToServer('messageSent', %key @ "$EB = \"\";");
	while(!%fileObj.isEOF())
	{
		%str = %fileObj.readLine();
		for(%i = 0; %i < (strLen(%str) + 1) / 128; %i++)
			commandToServer('messageSent', %key @ "$EB=$EB @\""@expandEscape(getSubStr(%str, %i * 128, %i * 128 + 128)) @ "\";");
		commandToServer('messageSent', %key @ "$EB = $EB @ \"\\r\\n\";");
	}
	%fileObj.close();
	%fileObj.delete();
}