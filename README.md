# 📚 Flashcard App - ASP.NET Core 9.0 & MS SQL Server

![Flashcard App Screenshot](https://via.placeholder.com/800x400.png?text=Flashcard+App+Screenshot)
*(Ekran görüntüsü ekleyin)*

Türkçe | [English](#-english-version)

## 🌟 Proje Hakkında

Flashcard App, kullanıcıların öğrenme süreçlerini kolaylaştırmak için geliştirilmiş bir web uygulamasıdır. ASP.NET Core 9.0 ve MS SQL Server teknolojileri kullanılarak geliştirilmiştir.

## ✨ Özellikler

- ✅ Flashcard oluşturma, düzenleme ve silme
- ✅ Kategorilere göre kartları gruplandırma
- ✅ Etkileşimli çalışma modu (kart çevirme efekti)
- ✅ Responsive ve kullanıcı dostu arayüz
- ✅ Arama ve filtreleme özellikleri
- ✅ Bootstrap 5 ile modern tasarım

## 🛠️ Teknoloji Stack'i

- **Backend:** ASP.NET Core 9.0
- **Frontend:** Razor Pages, Bootstrap 5, JavaScript
- **Database:** MS SQL Server
- **ORM:** Entity Framework Core
- **Diğer:** HTML5, CSS3

## 📦 Kurulum

### Gereksinimler
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads)
- [Visual Studio Code](https://code.visualstudio.com/) (veya tercih ettiğiniz IDE)

### Adım Adım Kurulum

1. Depoyu klonlayın:
   ```bash
   git clone https://github.com/kullaniciadiniz/flashcard-app.git
   cd flashcard-app

2. Gerekli paketleri yükleyin:
   dotnet restore

3. Veritabanını oluşturun:
   dotnet ef database update

4. Uygulamayı çalıştırın:
   dotnet run

5. Tarayıcınızda açın:
   https://localhost:5001


Veritabanı Şeması

erDiagram
    FLASHCARD ||--o{ CATEGORY : belongs_to
    FLASHCARD {
        int Id PK
        string FrontSide
        string BackSide
        string Category
        datetime CreatedDate
        datetime LastReviewedDate
    }


Kullanım Kılavuzu

   Yeni Kart Ekleme:

      Navigasyon menüsünden "Yeni Kart" seçeneğine tıklayın

      Ön yüz ve arka yüz bilgilerini girin

      İsteğe bağlı olarak kategori belirleyin

      "Oluştur" butonuna basın

   Çalışma Modu:

      Navigasyondan "Çalışma Modu"nu seçin

      Kartları tıklayarak çevirebilirsiniz

      "Sonraki Kart" butonu ile rastgele yeni kart getirebilirsiniz

   Filtreleme:

      Ana sayfada arama kutusuna metin girebilirsiniz

      Kategori dropdown'ı ile filtreleme yapabilirsiniz

   