# 📚 Kütüphane Otomasyon Sistemi

Modern ve kullanıcı dostu bir kütüphane yönetim sistemi. ASP.NET Core MVC ile Clean Architecture prensiplerine uygun olarak geliştirilmiştir.

## ✨ Özellikler

- **Kitap Yönetimi**: Kitap ekleme, düzenleme, silme ve arama
- **Üye Yönetimi**: Üye kaydı, durum takibi, ban sistemi
- **Ödünç İşlemleri**: Kitap ödünç verme, iade alma, gecikme takibi
- **Kullanıcı Rolleri**: Admin, Kütüphaneci, Üye rolleri
- **Raporlama**: Detaylı istatistikler ve raporlar
- **İletişim**: Üyelerden gelen mesajların yönetimi
- **Email Bildirimleri**: SMTP entegrasyonu ile email desteği

## 🛠️ Teknolojiler

| Teknoloji | Versiyon |
|-----------|----------|
| .NET | 8.0 |
| ASP.NET Core MVC | 8.0 |
| Entity Framework Core | 8.0.22 |
| SQL Server | Express/Standard |
| BCrypt.Net | 4.0.3 |

## 📁 Proje Yapısı

```
Kutuphane/
├── Core/
│   ├── Kutuphane.Domain/        # Entity'ler, Enum'lar
│   └── Kutuphane.Application/   # Servisler, DTO'lar, Interface'ler
├── Infrastructure/
│   ├── Kutuphane.Identity/      # Kimlik doğrulama
│   └── Kutuphane.Persistence/   # Veritabanı, Repository'ler
└── Presentation/
    ├── Kutuphane.WebUI/         # MVC Web Uygulaması
    └── Kutuphane.WebApi/        # API (opsiyonel)
```

## 🚀 Kurulum

Detaylı kurulum adımları için [KURULUM.md](KURULUM.md) dosyasına bakınız.

### Hızlı Başlangıç

1. **Gereksinimler**
   - .NET 8.0 SDK
   - SQL Server (Express veya üstü)
   - Visual Studio 2022 veya VS Code

2. **Veritabanı Ayarları**
   `appsettings.json` dosyasında connection string'i güncelleyin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=KutuphaneOtomasyonDB;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```

3. **Migration'ları Uygulama**
   ```bash
   dotnet ef database update --project Infrastructure/Kutuphane.Persistence --startup-project Presentation/Kutuphane.WebUI
   ```

4. **Uygulamayı Çalıştırma**
   ```bash
   cd Presentation/Kutuphane.WebUI
   dotnet run
   ```

## 👤 Kullanıcı Rolleri

| Rol | Yetkiler |
|-----|----------|
| **Admin** | Tam sistem erişimi, kullanıcı yönetimi |
| **Librarian** | Kitap, üye ve ödünç işlemleri yönetimi |
| **Member** | Kendi profilini görüntüleme, kitap arama |

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---

📧 Sorularınız için iletişime geçebilirsiniz.
