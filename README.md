# Sate Ambal Pak Tino
![image](https://github.com/user-attachments/assets/db6c4135-a428-44b3-a1bb-1ee6d9f8695a)

<div align="justify">
Selamat datang di repository Sate Ambal Pak Tino. Repository ini berisi bot-bot hasil rekayasa para insinyur bawahan Pak Tino yang dapat digunakan untuk bertarung di Robocode Tank Royale. Bot yang dibuat mengimplementasikan Algoritma Greedy dengan berbagai heuristik yang berbeda-beda untuk berusaha meraih poin tertinggi dalam baku hantam ini. Selamat mencoba!!!
</div>

## Description
<div align="justify">
Pada repository ini terdapat empat bot yang menerapkan algoritma greedy dengan heuristik yang berbeda-beda. Berikut penjelasan singkatnya.<br>
</div>

1. Bot Psatir
    <div align="justify">
   Bot ini menerapkan algoritma greedy dengan melakukan ramming pada musuh berenergi rendah. Ide dari strategi ini adalah untuk memaksimalkan poin yang diperoleh dari tindakan ramming terhadap bot musuh. Melakukan ramming terhadap bot musuh memberikan dua kali dari damage yang dilakukan dan membunuh bot musuh memberikan ram damage bonus berupa 30% dari damage yang telah dilakukan terhadap bot yang dibunuh
    </div>
2. Bot Prundung
    <div align="justify">
   Bot ini menerapkan algoritma greedy untuk selalu menuju corner dan me-lock musuh. Ide dari strategi ini adalah memaksimalkan poin bertahan hidup lebih lama untuk mendapatkan survival score dengan bergerak ke corner dan menge-lock musuh hingga mati atau tidak terdeteksi untuk mendapatkan bullet damage.
    </div>
3. Bot Psilat
    <div align="justify">
   Bot ini menerapkan algoritma freedy berupa menyerang lawan jarak dekat. Ide dari strategi ini adalah memaksimalkan poin bullet damage dengan memperbesar peluang tiap peluru mengenai target. Hal ini dilakukan dengan menyerang lawan yang berjarak dekat saja. Apabila lawan yang terdeteksi terlalu jauh maka bot akan mendekat terlebih dahulu.
    </div>
4. Bot Pecundunk
    <div align="justify">
   Bot ini menerapkan algoritma greedy berupa survival. Ide dari strategi ini adalah memaksimalkan poin survival. Hal ini dilakukan dengan melakukan gerakan random sehingga serangan lawan akan sulit untuk mengenai bot.
   </div>

## Getting Started

### ⚙️ Requirements

- [.NET SDK](https://dotnet.microsoft.com/) (.NET 6.0 or later)
- [Robocode Tank Royale CLI](https://robocode-tank-royale.io/cli/)
- Editor: Visual Studio / VS Code / Rider

### 🚀 Installing

1. **Clone Repository**  
   ```bash
   git clone https://github.com/mraifalkautsar/Tubes1_SateAmbalPakTino.git
2. **Build setiap bot**
   ```bash
   cd namaBot
   dotnet build
4. **Jalankan dengan Robocode Tank Royale**
   ```bash
   java -jar robocode-tankroyale-gui-0.30.0.jar

## Authors

1. [@Muhammad Ra’if Alkautsar (13523011)](https://github.com/mraifalkautsar)
2. [@Orvin Andika Ikhsan A (13523017)](https://github.com/orvin14)
3. [@Reza Ahmad Syarif (13523119)](https://github.com/Rejaah)
