@echo off

set EnjoyTronInstallersDemoPath="EnjoyTronInstallersDemo"

if exist %EnjoyTronInstallersDemoPath% rmdir /s /q %EnjoyTronInstallersDemoPath%

mkdir %EnjoyTronInstallersDemoPath%

mkdir %EnjoyTronInstallersDemoPath%\Admin
copy EnjoyTron\Tron.AdminClient.Setup\Release\* %EnjoyTronInstallersDemoPath%\Admin

mkdir %EnjoyTronInstallersDemoPath%\Debug
copy EnjoyTron\Tron.DebugClient.Demo.Setup\Release\* %EnjoyTronInstallersDemoPath%\Debug

if exist %EnjoyTronInstallersDemoPath%.zip ren %EnjoyTronInstallersDemoPath%.zip %EnjoyTronInstallersDemoPath%_old.zip
if exist %EnjoyTronInstallersDemoPath%.zip del %EnjoyTronInstallersDemoPath%.zip

7z a %EnjoyTronInstallersDemoPath%.zip %EnjoyTronInstallersDemoPath%/* test_map.txt

rmdir /s /q %EnjoyTronInstallersDemoPath%



set EnjoyTronZipsDemoPath="EnjoyTronZipsDemo"

if exist %EnjoyTronZipsDemoPath% rmdir /s /q %EnjoyTronZipsDemoPath%

mkdir %EnjoyTronZipsDemoPath%

mkdir %EnjoyTronZipsDemoPath%\Admin
copy EnjoyTron\Tron.Client\bin\Release\* %EnjoyTronZipsDemoPath%\Admin

mkdir %EnjoyTronZipsDemoPath%\Debug
copy EnjoyTron\Tron.DebugClient.Demo\bin\Release\* %EnjoyTronZipsDemoPath%\Debug

if exist %EnjoyTronZipsDemoPath%.zip ren %EnjoyTronZipsDemoPath%.zip %EnjoyTronZipsDemoPath%_old.zip
if exist %EnjoyTronZipsDemoPath%.zip del %EnjoyTronZipsDemoPath%.zip

7z a %EnjoyTronZipsDemoPath%.zip %EnjoyTronZipsDemoPath%/* test_map.txt

rmdir /s /q %EnjoyTronZipsDemoPath%