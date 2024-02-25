# Imag_Demo
Rozwiązanie zadania. Po sklonowaniu można uruchomić za pomocą komendy docker compose up, lub bezpośrednio z IDE Visual Studio.
Wszystkie zmienne środowiskowe są w pliku docker-compose.yml, domyślnie jest uruchamiany:
-server SQL na porcie 1433
-backend api na porcie 7142, można go przetestować za pomocą swaggera pod adresem: http://localhost:7142/swagger/index.html
-frontend do interakcji z bazą na porcie 80, dostępny z przeglądarki pod adresem localhost

Backend tworzy bazę DBImag i tabelę Customers wg. modelu code first i przy pierwszym uruchomieniu dodaje rekordy testowe
