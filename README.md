Wymagania:
Docker engine
SDK NET 6

Instrukcja uruchomienia:
1. Uruchomić skrypt do utworznia obrazy dockerowego aplikacji oraz załądowania obrazu redis i postgres.
  Lokalizacja:
    school-rest-api\school-rest-api\school-rest-api\Docker\loadImages.bat
    lub
    school-rest-api\school-rest-api\school-rest-api\Docker\loadImages.sh

2. Uruchomić skrypt do utworznia kontenerów przez pomocy docker-compose.
  Lokalizacja
    school-rest-api\school-rest-api\school-rest-api\Docker\executeDockerCompose.bat
    lub
    school-rest-api\school-rest-api\school-rest-api\Docker\executeDockerCompose.sh

Środowisko jest gotowe do działania.

Dodatkowe informacje:

Adres do swaggera:
http://localhost:5000/swagger/index.html

Plik do importu zapytań dla postmana:
school-rest-api\school-rest-api\school-rest-api\Postman\school_rest_api.postman_collection

!!!
Konfiguracja do połączeń z bazami redis oraz postgres znajduje się w pliku appsettings.json.

Zmodyfikowanie adresów przed zbudowaniem obrazu dockerowego zakończy się błędem połaczenia przy próbie uruchomienia takiego kontenera.
