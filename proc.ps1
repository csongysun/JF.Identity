
$List = (Get-ChildItem -recurse | where { ! $_.PSIsContainer  -and $_.extension -eq ".proto"});

foreach($file in $List){
    $Dir = $file.Directory;
    $cmd = 'protoc -I=' + $Dir + ' --csharp_opt=file_extension=.g.cs --csharp_out=' + $Dir + ' ' + $file.FullName;
    invoke-expression $cmd;
}

