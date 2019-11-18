echo "args 0 "  $args[0] 
echo "args 1 " $args[1]
$a= Get-Location
echo $a
echo now copy
Copy-Item -Path $args[0] -Destination $args[1] -Recurse -Force