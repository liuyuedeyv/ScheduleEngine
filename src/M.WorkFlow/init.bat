
rem 1.������Ŀ���õİ�
dotnet publish -o  .\bin\Debug\netcoreapp2.1
dotnet publish -o  .\bin\Release\netcoreapp2.1

rem 2.��������ļ��Ƿ���ȷ
cls
set filename=appsettings.Development.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo %fpath%
echo "%filename% ������,�����Ƿ�����Ϊʼ�ո���"
pause)
set filename=appsettings.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 

echo "%filename% ������,�����Ƿ�����Ϊʼ�ո���"
pause)
set filename=global.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% ������,�����Ƿ�����Ϊʼ�ո���"
pause)
set filename=GW.ApiLoader.deps.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% ������,�����Ƿ�����Ϊʼ�ո���"
pause)
set filename=GW.ApiLoader.runtimeconfig.json
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% ������,�����Ƿ�����Ϊʼ�ո���"
pause)

set filename=GW.ApiLoader.dll
set fpath=%~dp0\bin\Debug\netcoreapp2.1\%filename%
if not exist %fpath% ( 
cls 
echo "%filename% ������,�����Ƿ�������"
pause)
cls
echo "������ʼ���ɹ�,�����ⰴ���˳�" 
pause
 