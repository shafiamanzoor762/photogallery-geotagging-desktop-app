# 📸 PhotoGallery Desktop App

A smart photo gallery **Windows desktop application** built using **.NET Windows Forms**, **C#**, and **SQLite**.

[![.NET](https://img.shields.io/badge/.NET-Windows-blueviolet?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQLite](https://img.shields.io/badge/SQLite-Integrated-lightgrey?logo=sqlite)](https://www.sqlite.org/index.html)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightblue?logo=windows)](https://www.microsoft.com/windows/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 🧠 Overview

**PhotoGallery Desktop App** helps users **organize, manage, and browse photos** efficiently.  
It leverages **metadata, geotags, and user-defined categories** to group images, making navigation smooth and intuitive.

Key Features:
- 🧍 Categorize photos by **person**
- 🎉 Sort/group by **event**
- 📍 Organize by **location (geotagging)**
- 📅 Search by **date/time**

---

## 🚀 Getting Started

### ✅ Requirements
Before running this project, ensure you have:
- Windows 10 or later
- [.NET Desktop Runtime 6.0+](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)  
- Visual Studio 2022 (with **.NET desktop development** workload)
- SQLite (installed via NuGet, auto-restored when building)

---

## 🛠 Installation

### Step 1: Clone the Repository

```
git clone https://github.com/shafiamanzoor762/photogallery-geotagging-desktop-app.git
cd photogallery-geotagging-desktop-app
```
### Step 2: Open in Visual Studio
Open PhotoGallery.sln in Visual Studio.
Restore NuGet packages (SQLite etc.).
Build the solution (Ctrl + Shift + B).

### Step 3: Run the Application
Press F5 to run in Debug mode.
Or run the compiled .exe from the bin/Debug or bin/Release folder.

### 📂 Project Structure
```
PhotoGallery-Desktop/
├── Forms/              # Windows Forms (UI)
├── Models/             # Data models (Photo, Event, Location)
├── Services/           # SQLite service, image metadata utilities
├── Helpers/            # Common helper classes
└── Program.cs          # Application entry point
```
---
### 💡 Features
#### 📂 Photo Importing — Add and manage photos.
#### 🔍 Smart Search — Search by person, event, location, or date.
#### 🧭 Geotagging Support — Reads and uses photo GPS metadata.
#### 🗃 SQLite Database — Fast and lightweight local storage.
#### 🖼 Windows Forms UI — Simple, clean desktop interface.
---
### 🛠 Technologies Used
#### Technology	Description
#### .NET WinForms	UI framework for Windows desktop apps
#### C#	Programming language
#### SQLite	Local database engine
#### System.Drawing	Image rendering & metadata handling
#### EXIF libs	Extract image metadata (date, GPS, etc.)
---
### 🐛 Troubleshooting
#### ❗ App won’t run?

  #### Install .NET Desktop Runtime 6.0+.

  #### Rebuild the solution in Visual Studio.

### 📂 Database errors?

#### Ensure SQLite .db file is accessible and not locked.

### 🖼 Photos not loading?

#### Check file permissions and supported formats (JPEG, PNG, etc.).
---
### 📘 Learn More
#### .NET Windows Forms
#### SQLite Documentation
#### C# Programming Guide
---
### 🤝 Contributing
#### Pull requests are welcome!
#### For major changes, please open an issue first to discuss what you’d like to add.
#### Follow best practices for C# project structure and naming.
---
### 📄 License
#### This project is licensed under the MIT License — see the LICENSE file for details.
