echo "starting build angular"
cd InfovalutarWebAng
npm i
npm i -g @angular/cli
ng build  --prod --build-optimizer
cd ..

$source= "InfovalutarWebAng/dist/InfovalutarWebAng/"
$dest= "InfoValutarWebAPI/wwwroot/"
echo "delete files"
Get-ChildItem -Path $dest -Include *.* -File -Recurse | foreach { $_.Delete()}
echo "copy files"
Get-ChildItem -Path $source | Copy-Item -Destination $dest

