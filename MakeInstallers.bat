@echo off

set EnjoyTronInstallersPath="EnjoyTronInstallers"

if exist %EnjoyTronInstallersPath% rmdir /s /q %EnjoyTronInstallersPath%

mkdir %EnjoyTronInstallersPath%

mkdir %EnjoyTronInstallersPath%\Admin
copy EnjoyTron\Tron.AdminClient.Setup\Release\* %EnjoyTronInstallersPath%\Admin

mkdir %EnjoyTronInstallersPath%\Debug
copy EnjoyTron\Tron.DebugClient.Setup\Release\* %EnjoyTronInstallersPath%\Debug

if exist %EnjoyTronInstallersPath%.zip ren %EnjoyTronInstallersPath%.zip %EnjoyTronInstallersPath%_old.zip
if exist %EnjoyTronInstallersPath%.zip del %EnjoyTronInstallersPath%.zip

7z a %EnjoyTronInstallersPath%.zip %EnjoyTronInstallersPath%/* test_map.txt

rmdir /s /q %EnjoyTronInstallersPath%



set EnjoyTronZipsPath="EnjoyTronZips"

if exist %EnjoyTronZipsPath% rmdir /s /q %EnjoyTronZipsPath%

mkdir %EnjoyTronZipsPath%

mkdir %EnjoyTronZipsPath%\Admin
copy EnjoyTron\Tron.Client\bin\Release\* %EnjoyTronZipsPath%\Admin

mkdir %EnjoyTronZipsPath%\Debug
copy EnjoyTron\Tron.DebugClient\bin\Release\* %EnjoyTronZipsPath%\Debug

if exist %EnjoyTronZipsPath%.zip ren %EnjoyTronZipsPath%.zip %EnjoyTronZipsPath%_old.zip
if exist %EnjoyTronZipsPath%.zip del %EnjoyTronZipsPath%.zip

7z a %EnjoyTronZipsPath%.zip %EnjoyTronZipsPath%/* test_map.txt

rmdir /s /q %EnjoyTronZipsPath%