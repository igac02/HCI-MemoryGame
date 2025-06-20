Memory Game - Igra pamćenja
Opis sistema
Memory Game je WPF aplikacija razvijena u C# koja testira i poboljšava vašu sposobnost pamćenja kroz progresivne nivoe. Igra funkcionište tako što prikazuje sekvencu riječi koju trebate zapamtiti i reproducirati. Sa svakim novim nivoom, sekvenca postaje duža, čineći igru sve izazovnijom.
Glavni dijelovi interfejsa
1. Naslovni bar

Naslov: "MEMORY GAME" - prikazan ljubičastom bojom u gornjem lijevom uglu
Dugme za minimizovanje (—): Smanjuje aplikaciju u taskbar
Dugme za zatvaranje (✕): Zatvara aplikaciju

2. Panel za odabir teme
Panel sa šest dugmića za odabir kategorije riječi:

Životinje: Igra sa imenima životinja (pas, mačka, tigar, lav, itd.)
Gradovi: Igra sa imenima gradova (Beograd, Zagreb, London, Pariz, itd.)
Predmeti: Igra sa imenima predmeta (stolica, sto, lampa, telefon, itd.)
Brojevi: Igra sa brojevima (0-9, dva i trocifrani brojevi)
Voće i povrće: Igra sa imenima voća i povrća (jabuka, kruška, paradajz, itd.)
Nasumično: Kombinacija riječi iz svih kategorija

3. Glavna igračka oblast

Statusni tekst: Prikazuje trenutno stanje igre i instrukcije
Indikator nivoa: Kružni indikator u gornjem desnom uglu koji pokazuje trenutni nivo (pojavljuje se tokom igre)

4. Kontrolni panel (dno ekrana)

Polje za unos: Tekst polje gdje unosite zapamćenu sekvencu
Dugme POTVRDI: Potvrđuje vaš unos
Dugme POKRENI IGRU: Pokreće novu igru sa odabranom temom
Dugme PONOVI IGRU: Restartuje trenutnu igru
Panel najvišeg rezultata: Prikazuje vaš najbolji postignuti nivo

Korisničko uputstvo
Pokretanje igre

Odaberite temu

Kliknite na jedno od šest dugmića teme
Odabrano dugme će promijeniti boju u tirkiznu
Statusni tekst će potvrditi odabranu temu


Pokrenite igru

Kliknite na dugme "POKRENI IGRU"
Pojaviće se indikator nivoa u gornjem desnom uglu



Igranje

Priprema za nivo

Igra će početi sa "Pripremite se!" porukom
Sledi odbrojavanje 3-2-1 sekunde


Prikazivanje sekvence

Na ekranu će se prikazati sekvenca riječi koju trebate zapamtiti
Duže i kompleksnije sekvence će biti prikazane duže
Pažljivo zapamtite redoslijed riječi


Unos odgovora

Nakon što sekvenca nestane, pojaviće se polje za unos
Unesite zapamćenu sekvencu, odvojenu spacovima
Možete koristiti dugme "POTVRDI" ili pritisnuti Enter


Rezultat

Tačan odgovor: Prelazite na sljedeći nivo sa dužom sekvencom
Netačan odgovor: Igra se završava



Završetak igre
Kada napravite grešku:

Prikazaće se poruka o kraju igre sa postignuutim nivoom
Ako ste postigli novi rekord, automatski će biti sačuvan
Pojaviće se dugme "PONOVI IGRU" za novi pokušaj

Praćenje napretka

Indikator nivoa: Prikazuje trenutni nivo tokom igre
Najviši rezultat: Panel na dnu stalno prikazuje vaš najbolji postignuti nivo
Rezultati se automatski čuvaju u fajl "highscore.txt"

Detaljne funkcionalnosti dugmića
Dugmići tema

Funkcija: Biraju kategoriju riječi za igru
Vizalni efekat: Odabrano dugme mijenja boju
Rezultat: Postavlja listu riječi i omogućava pokretanje igre

POKRENI IGRU

Kada je dostupan: Nakon odabira teme
Funkcija: Inicijalizuje novu igru, kreće od nivoa 1
Efekat: Sakriva dugme i pokazuje indikator nivoa

POTVRDI

Kada je dostupan: Tokom unosa odgovora
Funkcija: Provjera unesene sekvence protiv tačne
Alternativa: Enter tipka u polju za unos

PONOVI IGRU

Kada se pojavljuje: Nakon završetka igre
Funkcija: Restartuje igru sa istom temom
Rezultat: Vraća na nivo 1 iste kategorije

Polje za unos

Format: Riječi odvojene spacovima
Ograničenja: Maksimalno 100 karaktera
Funkcionalnost: Automatski fokus kada je aktivno

Napomene za korištenje

Unos teksta: Nije osjetljiv na velika/mala slova
Razmaci: Koristite jedan space između riječi
Redoslijed: Mora biti tačno isti kao u prikazanoj sekvenci
Fokus: Aplikacija automatski postavlja fokus na potrebna polja
Čuvanje: Rezultati se automatski čuvaju, ne gubite ih zatvaranjem aplikacije

Sistem bodovanja

Nivo = broj riječi u sekvenci
Počinje se sa 1 riječi na prvom nivou
Svaki sljedeći nivo dodaje jednu riječ više
Vaš rezultat je poslednji uspješno završen nivo
Najviši rezultat se trajno čuva i prikazuje