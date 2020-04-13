
rem 1.生成项目引用的包
dotnet publish -o  .\bin\Debug\netcoreapp2.1
dotnet publish -o  .\bin\Release\netcoreapp2.1

rem 2.检测生成文件是否正确
cls
set filename=appsettings.Development.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo %fpath%
echo "%filename% 不存在,请检查是否设置为始终复制"
pause)
set filename=appsettings.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 

echo "%filename% 不存在,请检查是否设置为始终复制"
pause)
set filename=global.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% 不存在,请检查是否设置为始终复制"
pause)
set filename=GW.ApiLoader.deps.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% 不存在,请检查是否设置为始终复制"
pause)
set filename=GW.ApiLoader.runtimeconfig.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% 不存在,请检查是否设置为始终复制"
pause)

set filename=GW.ApiLoader.dll
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% 不存在,请检查是否有引用"
pause)
cls
echo "环境初始化成功,按任意按键退出" 
pause
 