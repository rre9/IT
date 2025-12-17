# Data Storage Locations - IT Help Desk System

## 📊 قاعدة البيانات (Database)

### الموقع الحالي (Development):
```
SQL Server LocalDB
Server: (localdb)\mssqllocaldb
Database Name: ITHelpDesk
```

### Connection String:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ITHelpDesk;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### الملف الفعلي:
- **LocalDB** يحفظ قاعدة البيانات في:
  ```
  C:\Users\{YourUsername}\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\
  ```
- أو في:
  ```
  C:\Users\{YourUsername}\
  ```

### للعثور على قاعدة البيانات:
1. افتح **SQL Server Management Studio (SSMS)**
2. اتصل بـ: `(localdb)\mssqllocaldb`
3. ستجد قاعدة البيانات `ITHelpDesk`

---

## 📁 الصور والملفات المرفقة (Attachments)

### الموقع الحالي:
```
{ProjectRoot}/wwwroot/uploads/{TicketId}/
```

### مثال:
```
C:\Users\USER\OneDrive\Desktop\it\ITHelpDesk\wwwroot\uploads\1\abc123def456.jpg
C:\Users\USER\OneDrive\Desktop\it\ITHelpDesk\wwwroot\uploads\2\xyz789ghi012.png
```

### هيكل المجلدات:
```
wwwroot/
└── uploads/
    ├── 1/          (ملفات تذكرة رقم 1)
    │   ├── abc123.jpg
    │   └── def456.png
    ├── 2/          (ملفات تذكرة رقم 2)
    │   └── xyz789.pdf
    └── 3/          (ملفات تذكرة رقم 3)
        └── ...
```

### معلومات الملفات في قاعدة البيانات:
- **اسم الملف الأصلي**: `FileName` في جدول `TicketAttachments`
- **المسار النسبي**: `FilePath` (مثل: `uploads/1/abc123.jpg`)
- **تاريخ الرفع**: `UploadTime`
- **حجم الملف**: `FileSize` (bytes)

---

## 🔧 تغيير مواقع الحفظ

### 1. تغيير قاعدة البيانات إلى SQL Server كامل:

#### في `appsettings.json` أو `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ITHelpDesk;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

#### أو استخدام Windows Authentication:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=ITHelpDesk;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

### 2. تغيير موقع حفظ الملفات:

#### خيار 1: مجلد مخصص على السيرفر
عدّل `TicketAttachmentService.cs`:
```csharp
// بدلاً من:
var uploadsFolder = Path.Combine(_environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", ticketId.ToString());

// استخدم:
var uploadsFolder = Path.Combine(@"D:\ITHelpDeskFiles", "uploads", ticketId.ToString());
```

#### خيار 2: استخدام Azure Blob Storage أو AWS S3
- تحتاج إلى إضافة NuGet packages:
  - `Azure.Storage.Blobs` (لـ Azure)
  - `AWSSDK.S3` (لـ AWS)
- تعديل `TicketAttachmentService` لرفع الملفات إلى السحابة

#### خيار 3: حفظ في قاعدة البيانات (BLOB)
- غير موصى به للملفات الكبيرة
- يبطئ قاعدة البيانات
- يصعب النسخ الاحتياطي

---

## 📋 إعدادات الإنتاج (Production)

### قاعدة البيانات:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=PROD-SQL-SERVER;Database=ITHelpDesk_Prod;User Id=ithelpdesk_user;Password=SecurePassword123!;TrustServerCertificate=True;"
  }
}
```

### الملفات:
```csharp
// في TicketAttachmentService.cs
var uploadsFolder = Path.Combine(@"\\FileServer\ITHelpDesk\Uploads", ticketId.ToString());
// أو
var uploadsFolder = Path.Combine(@"D:\Applications\ITHelpDesk\Files", "uploads", ticketId.ToString());
```

---

## 🔒 الأمان والنسخ الاحتياطي

### قاعدة البيانات:
1. **نسخ احتياطي يومي**: استخدم SQL Server Maintenance Plans
2. **نسخ احتياطي أسبوعي**: احفظ نسخة في موقع آخر
3. **الصلاحيات**: استخدم حساب مستخدم محدود الصلاحيات (ليس sa)

### الملفات:
1. **النسخ الاحتياطي**: ادمج مجلد `wwwroot/uploads` في النسخ الاحتياطي
2. **الأمان**: تأكد من أن IIS/Application Pool لديه صلاحيات القراءة/الكتابة
3. **الحماية**: لا تسمح بالوصول المباشر للملفات (استخدم Controller للتحميل)

---

## 📊 حجم البيانات المتوقع

### قاعدة البيانات:
- **مستخدم واحد**: ~1 KB
- **تذكرة واحدة**: ~2-5 KB
- **سجل نشاط واحد**: ~1 KB
- **مرفق واحد (في DB)**: ~500 bytes (فقط metadata)

**تقدير**: 1000 تذكرة = ~5-10 MB في قاعدة البيانات

### الملفات:
- **صورة JPG**: 100 KB - 2 MB (متوسط)
- **صورة PNG**: 200 KB - 3 MB (متوسط)
- **PDF**: 500 KB - 5 MB (متوسط)

**تقدير**: 1000 تذكرة × 2 ملفات = ~200 MB - 4 GB (حسب حجم الملفات)

---

## 🛠️ أدوات الإدارة

### عرض قاعدة البيانات:
1. **SQL Server Management Studio (SSMS)**
2. **Azure Data Studio**
3. **Visual Studio** (Server Explorer)

### عرض الملفات:
- استخدم **File Explorer** للوصول إلى `wwwroot/uploads`
- أو استخدم **PowerShell**:
  ```powershell
  Get-ChildItem -Path "wwwroot\uploads" -Recurse | Select-Object FullName, Length
  ```

### تنظيف الملفات المحذوفة:
```sql
-- في SQL Server
-- احذف المرفقات المحذوفة من قاعدة البيانات
DELETE FROM TicketAttachments WHERE TicketId NOT IN (SELECT Id FROM Tickets);
```

---

## 📝 ملاحظات مهمة

1. **LocalDB للتطوير فقط**: لا تستخدم LocalDB في الإنتاج
2. **النسخ الاحتياطي**: احرص على نسخ قاعدة البيانات والملفات بانتظام
3. **الأمان**: تأكد من حماية مجلد الملفات من الوصول المباشر
4. **الأداء**: إذا كان لديك آلاف الملفات، فكر في استخدام Cloud Storage
5. **المساحة**: راقب مساحة القرص الصلب للملفات

---

## 🔍 التحقق من المواقع

### قاعدة البيانات:
```powershell
# في PowerShell
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT name FROM sys.databases WHERE name = 'ITHelpDesk'"
```

### الملفات:
```powershell
# في PowerShell (من مجلد المشروع)
Get-ChildItem -Path "wwwroot\uploads" -Recurse | Measure-Object -Property Length -Sum
```

---

**آخر تحديث**: 2024

