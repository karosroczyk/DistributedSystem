# Systemy_rozproszone


URUCHOMIENIE VOL2:
1. Odpalamy plik: 
a) runAll.bat, gdy chcemy odpalić jedną aplikację
b) runMultiple.bat, gdy chcemy odpalić dwie instancje aplikacji.
Oba te .bat uruchamiają także aplikacje promotora i antyplagiatu.


ODPALANIE PREVIOUS:
1. Wejść w cmd do ścieżki: System_antyplagiatowy\bin\Debug\netcoreapp2.2
2. Puścić komendę: dotnet System_antyplagiatowy.dll (Powinno pojawić się w konsoli "Waiting for server to connect...")
3. To samo dla: Promotor.dll i Wirtualny_dziekanat.dll
4. Wejść w cmd do ścieżki: User\bin\Debug\netcoreapp2.2
5. Puścić komendę: dotnet User.dll