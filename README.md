# Przychodnia - Aplikacja WPF

Aplikacja WPF dla zarządzania wizytami w przychodni. Aplikacja pozwala na dodawanie pacjentów, pracowników, umawianie wizyt oraz generowanie raportów. Działa w zależności od roli użytkownika: administrator, rejestrator, lekarz, pacjent i dyrektor.

## Wymagania

- .NET Framework 4.7.2 lub nowszy
- Visual Studio 2019 lub nowszy

## Instalacja

1. Sklonuj repozytorium:

    ```bash
    git clone https://github.com/twojrepozytorium/przychodnia.git
    ```

2. Otwórz rozwiązanie w Visual Studio.

3. Przygotuj pliki `wizyty.txt` i `pacjenci.txt` w katalogu głównym projektu. Przykładowe formaty plików:

    - `wizyty.txt`:

      ```text
      pesel_pacjenta,login_lekarza,data_wizyty,godzina_wizyty,status_wizyty,wywiad,rozpoznanie,zalecenia
      ```

    - `pacjenci.txt`:

      ```text
      imie,nazwisko,pesel,telefon
      ```

4. Skompiluj i uruchom projekt.

## Użytkowanie

### Role użytkowników

- **Administrator**: Może dodawać pacjentów, pracowników, umawiać wizyty, wyświetlać pacjentów, pracowników, wizyty oraz generować raporty.
- **Rejestrator**: Może dodawać pacjentów, umawiać wizyty, wyświetlać pacjentów i wizyty.
- **Lekarz**: Może wyświetlać pacjentów i wizyty.
- **Pacjent**: Może wyświetlać swoje wizyty.
- **Dyrektor**: Może wyświetlać raporty, pacjentów i pracowników.

### Funkcje

- **Dodaj Pacjenta**: Dodaje nowego pacjenta do systemu.
- **Dodaj Pracownika**: Dodaje nowego pracownika do systemu.
- **Umów Wizytę**: Umawia wizytę pacjenta z lekarzem.
- **Wyświetl Pacjentów**: Wyświetla listę pacjentów.
- **Wyświetl Pracowników**: Wyświetla listę pracowników.
- **Wyświetl Wizyty**: Wyświetla listę wizyt.
- **Wyświetl Raporty**: Generuje i wyświetla raport liczby wizyt każdego dnia u konkretnego lekarza.

### Instrukcja

1. **Logowanie**: Po uruchomieniu aplikacji użytkownik loguje się przy użyciu swojego loginu i hasła.
2. **Interfejs użytkownika**: W zależności od roli użytkownika, odpowiednie przyciski będą widoczne.
3. **Dodawanie i przeglądanie danych**: Użytkownik może dodawać pacjentów, pracowników, umawiać wizyty oraz przeglądać listy pacjentów, pracowników i wizyt.
4. **Generowanie raportów**: Dyrektor może wygenerować raport liczby wizyt każdego dnia dla każdego lekarza.

### Przykłady plików wejściowych

- `wizyty.txt`:

  ```text
  12345678901,lekarz1,2023-01-01,10:00,Zrealizowana,Wywiad1,Rozpoznanie1,Zalecenia1
  12345678901,lekarz2,2023-01-02,11:00,Zaplanowana,Wywiad2,Rozpoznanie2,Zalecenia2
