$file = ".\InfoValutar\InfoValutarWebAPI\Controllers\InfoController.cs"
$date = Get-Date -Format "yyyyMMdd:HHmmss"
Get-ChildItem Env:

$author= $Env:BUILD_SOURCEVERSIONAUTHOR

$commitText = $env:BASH_COMMITMESSAGE
((Get-Content -path $file -Raw) -replace '{LatestCommit}',$commitText -replace '{LastAuthor}',$author -replace '{DateCommit}' , $date ) | Set-Content -Path $file
(Get-Content -path $file -Raw)
