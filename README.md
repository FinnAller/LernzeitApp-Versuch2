# Prototyp LernzeitApp
Entwickelt von Jork Buchholz ab dem 24.05.2024 für das Lise Meitner Gymnasium Leverkusen
## Todo
- [x] StudentMissingLessonPage zu Ende bauen.
- [ ] StudentMissingLessonPage remote content
# Aufbau
Die App soll Schülern und Lehrern die Möglichkeit bieten, Lernzeiten und die zugehörigen Anwesenheiten per Handy einzusehen.
Es stehen drei veschiedene Versionen zur verfügung:
## Schüler
Schüler haben die Möglichkeit, die Lernzeiten des Tages anzusehen und der ihrer Wahl teilzunehmen, sollte noch platz sein.
### Schüler-Unterseiten
- StudentHomePage          (Eine Übersicht aller verfügbaren Module des Tages(es werden keine vollen angezeigt)
- StudentMenuPage          (Ein Menü zum Navigieren zwischen den Unterseiten)
- StudentModulOverviewPage (Eine flexible Seite zum darstellen der Infos eines ausgewählten Moduls)
- StudentMissingPage       (Eine Auflistung aller Fehlstunden mit der Info entschulditg/unentschuldigt)
## Lehrer
Lehrer können einsehen, welche Schüler sich für ihre Lernzeit eingeschrieben haben,
und bestätigen, ob sie anwesend sind.
### Lehrer-Unterseiten
- TeacherMyModulesPage       (Eine übersicht aller Module die dem angemeldeten Lehrer unterstellt sind)
- TeacherAllModuleOverviewPage  (Eine Übersicht aller Module des Tages)
- TeacherMenuPage            (Ein Menü zum Vavigieren zwischen den Unterseiten)
- TeacherClassMissingPage    (Eine Liste der Schüler der eigenen Klasse und deren Fehlstunden)
- TeacherStudentDBPage       (Eine Liste aller Schüler der Schule)
- TeacherStudentOverviewPage (Eine fleixble Seite zum darstellen der Daten eines Schülers und all seiner Fehlstunden, und die Möglichkeit, diese zu entschuldigen)
- TeacherModuleOverviewPage  (Eine übersicht eines Moduls und aller teilnehmenden Schüler, samt Möglichkeit, deren Anwesenheit zu bestätigen)
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
## Regelmäßigkeit von Lernzeiten
Nx2
![bitshifting](https://github.com/Joelbu537/LernzeitApp-Versuch2/assets/89338010/f7496b42-eb2e-4f11-a32b-6550c405fe4b)
Um die regelmäßigkeit eines Moduls zu bestimmen, einfach die Zahlen addieren.
Eine LErnzeit, die jeden Montag stattfindet, hat den Wert 33 (1 + 32)
## Wechseln des ausgewählten Moduls
Wenn ein Schüler seine Meinung ändert und ein anderes Modul besuchen möchte,  
muss er nur den Teilnehmen-Knopf der neuen Auswahl drücken.
## Aufbau login.dat
0. Email
1. Gehashtes Passwort
2. Timestamp
