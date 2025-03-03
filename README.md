# Prototyp LernzeitApp
Entwickelt von Jork Buchholz ab dem 24.05.2024 für das Lise Meitner Gymnasium Leverkusen
## Aufbau
Die App soll Schülern und Lehrern die Möglichkeit bieten, Lernzeiten und die zugehörigen Anwesenheiten per Handy einzusehen.
Es stehen drei veschiedene Versionen zur verfügung:
### Schüler
Schüler haben die Möglichkeit, die Lernzeiten des Tages anzusehen und der ihrer Wahl teilzunehmen, sollte noch platz sein.
### Lehrer
Lehrer können einsehen, welche Schüler sich für ihre Lernzeit eingeschrieben haben,
und bestätigen, ob sie anwesend sind.
### Administrator
Administratoren entfernen und fügen Lernzeiten hinzu, verwalten die anderen Benutzerkonten und haben uneingeschränkten zugriff auf die Datenbank.
Diese Rolle solte nur Lehrern mit Wissen im Bereich Informatik und ranghohen Individuen zugeteilt werden.
Administratoren werden bis auf weiteres nicht über die App selbst zugreiffen, sondern die ändrrungen direkt an der Datenbank vornehmen.

Schüler und Lehrer verwenden die gleiche App, die Zugriffsstufe wird in der Datenbank festgestellt. Die App ruft anhand der Informationen verschieden Overlays auf.

## Verify
Verify ist der Prozess, bei dem Nutzerdaten an den Server geschickt werden, und ein Status zurückgegeben wird.  
Aufbau der Nachicht:  
```verify\r\nmax.mustermann@lmg.schulen-lev.de```  
Aufbau der Antwort:  
```verify\r\n1```
Stati:  
0 = Falsches Password/Falsche Email  
1 = Schülerkonto verifiziert  
2 = Lehrerkonto  verifiziert   
## GetMods
Getmods ruft eine Liste aller verfügbarer Module auf  
Aufbau der Nachicht:
```getmods```  
Aufbau der Antowrt:  
```getmods\rSchachAG\n9:00\n9:45\n4203\n8\n15\rSchule ohne Rassismus\n10:30\n11:00\n3009\n8\n3\n20\n```  
0.      Header  
1 - 999 Content  
## Wechseln des ausgewählten Moduls
Wenn ein Schüler seine Meinung ändert und ein anderes Modul besuchen möchte,  
muss er nur den Teilnehmen-Knopf der neuen Auswahl drücken.
## Aufbau login.dat
0. Email
1. Gehashtes Passwort
2. Timestamp
