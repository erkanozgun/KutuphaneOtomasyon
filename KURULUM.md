# 🚀 Kütüphane Otomasyon Sistemi - Kurulum Rehberi

Bu doküman, projenin sıfırdan kurulumu için gereken tüm adımları içermektedir.

---

## 📋 Gereksinimler

### Yazılım Gereksinimleri

| Yazılım | Minimum Versiyon | İndirme Linki |
|---------|------------------|---------------|
| .NET SDK | 8.0 | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) |
| SQL Server | Express 2019+ | [Download](https://www.microsoft.com/sql-server/sql-server-downloads) |
| Visual Studio | 2022 | [Download](https://visualstudio.microsoft.com/) |
| Git | 2.x | [Download](https://git-scm.com/) |

### Visual Studio Workload'ları
- ASP.NET and web development
- .NET desktop development

---

## 🔧 Adım Adım Kurulum

### 1. Projeyi İndirme

```bash
# Projeyi klonlayın veya zip olarak indirin
git clone <repository-url>
cd KutuphaneOtomasyon/Kutuphane
```

### 2. Bağımlılıkları Yükleme

```bash
# Solution klasöründe
dotnet restore
```

### 3. SQL Server Kurulumu

SQL Server kurulu değilse:

1. SQL Server Express'i indirin ve kurun
2. SQL Server Management Studio (SSMS) kurun
3. SQL Server'ın çalıştığını doğrulayın

**SQL Server Bağlantı Kontrolü:**
```powershell
# PowerShell'de SQL Server servislerini kontrol edin
Get-Service -Name "MSSQL*"
```

### 4. Veritabanı Bağlantı Ayarları

`Presentation/Kutuphane.WebUI/appsettings.json` dosyasını düzenleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SUNUCU_ADI\\SQLEXPRESS;Database=KutuphaneOtomasyonDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Email": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

#### Sunucu Adını Bulma

```powershell
# PowerShell'de sunucu adınızı öğrenin
$env:COMPUTERNAME
```

Tipik connection string formatları:
- **SQL Express**: `Server=BILGISAYAR_ADI\SQLEXPRESS`
- **LocalDB**: `Server=(localdb)\MSSQLLocalDB`
- **SQL Server**: `Server=localhost`

### 5. Migration'ları Uygulama (Veritabanı Oluşturma)

**Yöntem 1: Komut Satırından**
```bash
# Solution klasöründeyken
dotnet ef database update --project Infrastructure/Kutuphane.Persistence --startup-project Presentation/Kutuphane.WebUI
```

**Yöntem 2: Package Manager Console (Visual Studio)**
```powershell
# Default project: Kutuphane.Persistence olarak ayarlayın
Update-Database
```

> ⚠️ **Not**: İlk çalıştırmada tüm tablolar otomatik oluşturulacaktır.

### 6. Uygulamayı Çalıştırma

**Yöntem 1: Visual Studio**
1. `Kutuphane.sln` dosyasını açın
2. `Kutuphane.WebUI` projesini başlangıç projesi olarak ayarlayın
3. F5 veya Ctrl+F5 ile çalıştırın

**Yöntem 2: Komut Satırı**
```bash
cd Presentation/Kutuphane.WebUI
dotnet run
```

**Yöntem 3: Watch Mode (Otomatik Yenileme)**
```bash
cd Presentation/Kutuphane.WebUI
dotnet watch run
```

### 7. Uygulamaya Erişim

Varsayılan URL'ler:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001

---

## 👤 İlk Admin Kullanıcısı Oluşturma

Veritabanı ilk oluşturulduğunda admin kullanıcısı yoktur. Seed data eklemek için:

### Yöntem 1: SQL ile Manuel Ekleme

```sql
-- SSMS veya Azure Data Studio'da çalıştırın
USE KutuphaneOtomasyonDB;

INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, Role, IsActive, CreatedAt)
VALUES (
    'admin',
    'admin@kutuphane.com',
    '$2a$11$K3dxkS8jrFN8aQCOWKRjEuGTG8x1vk3POPMdfKk8.jKjnJ2tR5Ocy', -- Şifre: Admin123!
    'Sistem',
    'Admin',
    0, -- Admin rolü
    1, -- Aktif
    GETDATE()
);
```

> 📌 **Varsayılan Giriş**: 
> - **Kullanıcı Adı**: admin
> - **Şifre**: Admin123!

### Yöntem 2: Kayıt Sayfasından

1. `/Account/Register` sayfasına gidin
2. İlk kullanıcıyı oluşturun
3. SQL'den rolü Admin (0) olarak güncelleyin

---

## 📧 Email Ayarları (Opsiyonel)

Gmail SMTP kullanmak için:

1. Google hesabınızda 2FA aktif edin
2. App Password oluşturun: [Google App Passwords](https://myaccount.google.com/apppasswords)
3. `appsettings.json`'da güncelleyin:

```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "Email": "your-email@gmail.com",
  "Password": "xxxx-xxxx-xxxx-xxxx"
}
```

---

## 🔍 Sorun Giderme

### Sık Karşılaşılan Hatalar

| Hata | Çözüm |
|------|-------|
| `Cannot connect to SQL Server` | Connection string'i kontrol edin, SQL Server servisinin çalıştığından emin olun |
| `Migration failed` | EF Core tools yüklü mü? `dotnet tool install --global dotnet-ef` |
| `Certificate error` | Connection string'e `TrustServerCertificate=True` ekleyin |
| `Port already in use` | `launchSettings.json`'dan portu değiştirin |

### EF Core Tools Yükleme

```bash
# Global olarak EF Core CLI aracını yükleyin
dotnet tool install --global dotnet-ef

# Güncelleme için
dotnet tool update --global dotnet-ef
```

### Build Hataları

```bash
# Temiz build
dotnet clean
dotnet restore
dotnet build
```

---

## 📊 Veritabanı Şeması

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│    Books    │────<│   Copies    │>────│    Loans    │
└─────────────┘     └─────────────┘     └──────┬──────┘
                                               │
┌─────────────┐                         ┌──────┴──────┐
│    Users    │>────────────────────────│   Members   │
└─────────────┘                         └─────────────┘
```

---

## ✅ Kurulum Kontrol Listesi

- [ ] .NET 8.0 SDK yüklü
- [ ] SQL Server yüklü ve çalışıyor
- [ ] Connection string güncellendi
- [ ] Migration'lar uygulandı
- [ ] İlk admin kullanıcısı oluşturuldu
- [ ] Uygulama başarıyla çalışıyor
- [ ] (Opsiyonel) Email ayarları yapıldı

---

**🎉 Tebrikler! Kütüphane Otomasyon Sistemi kullanıma hazır.**
