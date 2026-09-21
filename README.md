# 📖 Novel Reading Platform (كِتَاب)

> **A modern, luxury ASP.NET Core 10 MVC web platform for digital novel publishing, online reader customization, and paid chapter access.**

---

## 🌟 Key Features

* 📚 **Extensive Novel Library & Categories**: Explore digital novels categorized by genres (Fantasy & Parallel Worlds, Mystery & Crime, Drama & Romance, History & Legends).
* 🔒 **Free & Paid Chapter Protection**: Free access to intro chapters, with secure locking for premium chapters unlocked upon instant purchase.
* 📖 **Interactive Custom Reader Interface (Reader Mode)**:
  * Adjustable font sizing (Small, Medium, Large).
  * Multiple display themes (Light, Warm Sepia, Night Dark).
  * Smooth navigation between previous and next chapters with quick index access.
* 💳 **Simulated Payment Gateway (NovelsPay)**:
  * Multiple payment methods supported (Visa/Mastercard Credit Cards, E-Wallets like Vodafone Cash, Fawry Express).
  * Instant chapter activation upon successful payment completion.
* 👤 **Personal Reader Library & Order Invoices**:
  * View all purchased chapters with direct one-click reading access.
  * Complete transaction order history with reference IDs and invoice amounts.
* 🛡️ **Comprehensive Admin Control Panel**:
  * Performance analytics (Total Revenue, Transactions, Reader Count, Novel Statistics).
  * Full Novel Management (Add, Edit, Delete, Toggle Publication Status).
  * Chapter Management (Set Chapter Prices, Free vs Paid Status, Content Editing).
* 🎨 **Refactored Emerald & Slate UI/UX**:
  * Luxurious Royal Emerald (`#0d9488`), Ocean Sapphire (`#0284c7`), and Warm Gold (`#f59e0b`) color palette (Zero Purple).
  * High-contrast typography, full Arabic RTL layout optimization, and universal SVG image fallback protection.

---

## 🛠️ Technology Stack

* **Framework**: ASP.NET Core 10 MVC (.NET 10)
* **Database & ORM**: Entity Framework Core & ApplicationDbContext (Automated DbInitializer Seeding)
* **Security & Auth**: ASP.NET Core Identity (Role-Based Access: `Admin` & `Reader`)
* **Frontend & UI**: Bootstrap 5 RTL, FontAwesome 6, Cairo Arabic Google Font, Custom CSS3 & JS ES6 System

---

## 🚀 Getting Started & Local Setup

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Installation & Run Steps
1. Open terminal/PowerShell in the project root directory:
   ```bash
   cd "Novel Reading Platform"
   ```

2. Restore packages and build the solution:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Launch the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to the local server URL (typically `https://localhost:7147` or `http://localhost:5147`).

---

## 🔑 Demo Accounts Credentials

Seed data automatically initializes pre-configured accounts on first launch:

| Role | Email | Password |
|---|---|---|
| **System Administrator (Admin)** | `admin@novelhub.com` | `Admin@123456` |
| **Golden Reader** | `reader@novelhub.com` | `User@123456` |
| **Demo Reader (Sara)** | `sara@novelhub.com` | `User@123456` |
| **Demo Reader (Omar)** | `omar@novelhub.com` | `User@123456` |

---

## 📂 Project Structure

```
Novel Reading Platform/
├── Areas/
│   └── Admin/                 # Admin Panel (Dashboard, AdminNovels, AdminChapters)
├── Controllers/              # MVC Controllers (Home, Novels, Chapters, Checkout, Account)
├── Data/                     # EF Core Context & Seeding (ApplicationDbContext, DbInitializer)
├── Models/                   # Domain Models & ViewModels (Novel, Chapter, Order, Purchase)
├── Views/                    # Reader & User Facing Razor Views
├── wwwroot/                  # Static Web Assets (CSS, JS, Favicons)
│   ├── css/site.css          # Emerald Slate UI System Styles
│   └── js/site.js            # Image Fallback & SVG Book Cover Generator Script
├── Program.cs                # Application Entry Point & Dependency Injection Configuration
└── NovelPlatform.csproj      # C# Project File
```

---

## 📜 License & Rights

All rights reserved &copy; 2026 - **Novel Reading Platform (كِتَاب)**
