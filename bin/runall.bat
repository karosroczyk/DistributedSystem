@ECHO OFF
ECHO STARTING ALL APPLICATIONS...

C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe -nologo C:\Users\patry\OneDrive\Pulpit\rozproszone\Systemy_rozproszone\Promotor\Promotor.cs
START C:\Users\patry\OneDrive\Pulpit\rozproszone\Systemy_rozproszone\bin\Promotor.exe

C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe -nologo C:\Users\patry\OneDrive\Pulpit\rozproszone\Systemy_rozproszone\System_antyplagiatowy\Antyplagiat.cs
START C:\Users\patry\OneDrive\Pulpit\rozproszone\Systemy_rozproszone\bin\Antyplagiat.exe

C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe -nologo C:\Users\patry\OneDrive\Pulpit\rozproszone\Systemy_rozproszone\Wirtualny_dziekanat\Dziekanat.cs
START C:\Users\patry\OneDrive\Pulpit\rozproszone\Systemy_rozproszone\bin\Dziekanat.exe

PAUSE