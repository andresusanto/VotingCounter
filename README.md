# Voting Counter
![Hasil Pemilu](/../screenshoot/screenshoots/hasilpemilu.png?raw=true "Hasil Pemilu")

## Tentang project ini
Project ini adalah sebuah sistem yang digunakan untuk pemilihan umum online yang aman dan terpercaya. Sistem ini telah diterapkan untuk Pemilu HMIF tahun 2014. Sistem ini terdiri dari **Online Voting Web** dan **Voting Counter** (repo ini).

## Pengembangan dan Kompilasi
Program ini dikembangkan di lingkungan pengembangan **Visual Studio 2010** dengan bahasa pemrograman C#. Untuk melakukan kompilasi anda mungkin membutuhkan **Visual Studio 2010** atau versi yang lebih baru.

## Copyright
Sistem pemilihan beserta software yang digunakan merupakan bagian dari paper yang sedang saya tulis yang bertajuk  `Building a Secure and Trustworthy Online Voting System`. Segala material beserta metode yang saya gunakan boleh anda gunakan untuk keperluan non komersial dengan lisensi GNU. Untuk penggunaan komersial, silahkan hubungi saya untuk informasi lebih lanjut.

## Teknis Pemilu
1. Sistem meng-*generate* akun peserta (*voter*) pemilu beserta sebuah token (unique random key untuk setiap peserta).
2. Sistem mengirimkan akun (username dan password) serta token ke peserta pemilu. Peserta pemilu mencatat token untuk keperluan validasi akhir.
3. Peserta melakukan pemilihan di web voting online.
4. Web voting online tidak menyimpan hasil voting ke database, melainkan dikirim ke sebuah email 3rd party (misal gmail) yang dipassword lebih dari 1 orang (dalam kasus pemilu HMIF adalah panitia, ketua aktif, ketua "DPR", senator).
5. Setelah semua voting selesai, orang-orang yang mempassword email memasukan password mereka ke program Voting Counter. Program VotingCounter akan menghitung suara yang masuk.
6. Hasil perhitungan kemudian dipublish (beserta unique token). Peserta dapat memeriksa apakah unique tokennya masuk ke suara yang benar atau tidak.
