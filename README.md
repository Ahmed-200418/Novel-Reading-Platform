# 📖 منصة كِتَاب | Novel Reading Platform

> **منصة رقمية حديثة وفاخرة لقراءة ونشر الروايات الإلكترونية وشراء الفصول الحصرية بأعلى معايير الجودة والأمان البصري.**

---

## 🌟 مميزات المنصة الرئيسية (Key Features)

* 📚 **مكتبة روايات شاملة وتصنيفات أدبية**: استكشاف الروايات حسب التصنيف (خيال وعوالم موازية، غموض وجريمة، دراما ورومانسية، تاريخ وأساطير).
* 🔒 **نظام الفصول المجانية والمدفوعة**: دعم القراءة المجانية للفصول الأولى مع قفل الفصول المتقدمة وإتاحتها بعد الشراء الفوري.
* 📖 **واجهة قارئ تفاعلية ومرنة (Reader Mode)**:
  * التحكم في حجم الخط (صغير، متوسط، كبير).
  * تغيير مظهر الصفحة (فاتح، دافئ Sepia، داكن Night Mode).
  * التنقل السلس بين الفصول السابقة والتالية مع زر العودة للفهرس.
* 💳 **بوابة دفع رقمية محاكاة (NovelsPay)**:
  * دعم خيارات الدفع المختلفة (بطاقة ائتمانية Visa/Mastercard، محفظة إلكترونية كـ فودافون كاش، فوري Express).
  * تفعيل فوري ومباشر للفصل في حساب القارئ فور إتمام المعاملة.
* 👤 **مكتبة المستخدم وسجل الفواتير (My Library & Invoices)**:
  * استعراض جميع الفصول الممتلكة والقراءة المباشرة لها.
  * جدول كامل بسجل الطلبات والمعاملات المالية وأرقام المعاملات الترجيعية.
* 🛡️ **لوحة تحكم الأدمن (Admin Dashboard)**:
  * إحصائيات المبيعات، إجمالي الإيرادات، عدد القُرّاء والروايات.
  * إدارة الروايات (إضافة، تعديل، حذف، تغيير حالة النشر).
  * إدارة الفصول لكل رواية وتحديد السعر والحالة (مجاني/مدفوع).
* 🎨 **هوية بصرية احترافية (Refactored Emerald & Slate UI)**:
  * ألوان زمردية ملكية (`#0d9488`) مع أزرق أوقيانوسي (`#0284c7`) وتدرجات ذهبية (`#f59e0b`).
  * تباين ناصع بدون أي ألوان بنفسجية، ودعم تام للغة العربية والاتجاه من اليمين للشمال (RTL).
  * نظام ذكي لمعالجة وتعويض أخطاء تحميل الصور (SVG Image Fallback).

---

## 🛠️ التقنيات المستخدمة (Tech Stack)

* **Framework**: ASP.NET Core 10 MVC (.NET 10)
* **Database & ORM**: Entity Framework Core & ApplicationDbContext (Seeded Data)
* **Security & Auth**: ASP.NET Core Identity (Role-based: Admin & Reader)
* **UI & Styling**: Bootstrap 5 RTL, FontAwesome 6, Cairo Arabic Font, Custom CSS & JS System

---

## 🚀 كيفية التشغيل والتشغيل المحلي (Getting Started)

### المتطلبات الأساسية
* مثبت [.NET 10 SDK](https://dotnet.microsoft.com/download) أو أحدث.

### خطوات التشغيل
1. افتح موجه الأوامر (Terminal/PowerShell) في مجلد المشروع:
   ```bash
   cd "Novel Reading Platform"
   ```

2. قم باسترجاع الحزم وبناء المشروع:
   ```bash
   dotnet restore
   dotnet build
   ```

3. شغل التطبيق:
   ```bash
   dotnet run
   ```

4. افتح المتصفح وانتقل إلى الرابط المعروض (عادةً `https://localhost:7147` أو `http://localhost:5147`).

---

## 🔑 حسابات الدخول للتجربة السريعة (Demo Credentials)

يتم تهيئة البيانات الافتراضية تلقائيًا عند تشغيل التطبيق لأول مرة:

| نوع الحساب | البريد الإلكتروني (Email) | كلمة المرور (Password) |
|---|---|---|
| **مسؤول النظام (Admin)** | `admin@novelhub.com` | `Admin@123456` |
| **القارئ الذهبي (Reader)** | `reader@novelhub.com` | `User@123456` |
| **قارئ تجريبي (Sara)** | `sara@novelhub.com` | `User@123456` |
| **قارئ تجريبي (Omar)** | `omar@novelhub.com` | `User@123456` |

---

## 📂 هيكل المشروع (Project Structure)

```
Novel Reading Platform/
├── Areas/
│   └── Admin/                 # لوحة تحكم الأدمن (Dashboard, AdminNovels, AdminChapters)
├── Controllers/              # المتحكمات (Home, Novels, Chapters, Checkout, Account)
├── Data/                     # قاعدة البيانات والتهيئة الأوّلية (ApplicationDbContext, DbInitializer)
├── Models/                   # النماذج الكيانات ونماذج العرض (Novel, Chapter, Order, ViewModels)
├── Views/                    # واجهات العرض الخاصة بالقرّاء والمستخدمين
├── wwwroot/                  # الملفات الثابتة (CSS, JS, Fonts)
│   ├── css/site.css          # الهوية البصرية والنظام البصري الزمرّدي
│   └── js/site.js            # سكربت حماية ومعالجة تعويض الصور (Fallback)
├── Program.cs                # إعدادات التطبيق وحقن الخدمات
└── NovelPlatform.csproj      # ملف تكوين مشروع C#
```

---

## 📜 الترخيص والدعم (License & Rights)

جميع الحقوق محفوظة &copy; 2026 - **منصة كِتَاب | Novel Reading Platform**
