<div align="center">

# NAIRA GAMAL

### Full-Stack .NET Developer

<p>
  <strong>Modern • Interactive • Full-Stack Portfolio</strong>
</p>

<p>
  A modern personal portfolio built from scratch to showcase my
  <br />
  projects, technical skills, experience, and professional journey.
</p>

<br />

<a href="https://raw.githubusercontent.com/nairagamal/naira-gamal-portfolio/refs/heads/main/screenshots/portfolio-desktop.JPG">
  <img src="https://img.shields.io/badge/LIVE_PORTFOLIO-6B1F3A?style=for-the-badge&logo=googlechrome&logoColor=white" alt="Live Portfolio"/>
</a>
<a href="https://github.com/nairagamal">
  <img src="https://img.shields.io/badge/GITHUB-111111?style=for-the-badge&logo=github&logoColor=white" alt="GitHub"/>
</a>
<a href="https://www.linkedin.com/in/naira-gamal/">
  <img src="https://img.shields.io/badge/LINKEDIN-6B1F3A?style=for-the-badge&logo=linkedin&logoColor=white" alt="LinkedIn"/>
</a>

<br /><br />

<img
src="https://raw.githubusercontent.com/nairagamal/naira-gamal-portfolio/refs/heads/main/screenshots/portfolio-desktop.JPG"
alt="Naira Gamal Portfolio Preview"
width="92%"
/>

</div>

---

## ✦ About The Project

This repository contains my personal **Full-Stack Developer Portfolio**, designed and developed from scratch to showcase my professional experience, technical skills, selected projects, and development journey.

The portfolio is more than a static website. It was developed as a **full-stack web application** with a dedicated frontend, backend API, database, and administration dashboard.

The architecture allows portfolio content to be managed dynamically through the dashboard instead of modifying the frontend source code every time new content needs to be added.

---

## ⚡ Project Highlights

| Feature             | Description                                           |
| ------------------- | ----------------------------------------------------- |
| 🎨 Modern UI        | Clean, responsive and interactive portfolio interface |
| ⚡ Animations        | GSAP-powered animations and smooth interactions       |
| 🌀 Smooth Scrolling | Lenis smooth scrolling experience                     |
| 🔌 REST API         | Dedicated ASP.NET Core Web API                        |
| 🗄️ Database        | SQL Server with Entity Framework Core                 |
| 🛠️ Admin Dashboard | Manage portfolio content dynamically                  |
| 🔐 Authentication   | Protected administrative functionality                |
| 📱 Responsive       | Optimized for desktop, tablet and mobile              |
| 🔄 Dynamic Content  | Content retrieved and managed through the API         |

---

# 🏗️ Architecture

The application is built using a **Frontend → API → Database** architecture with an additional administration layer for managing the portfolio content.

```text
                         ┌─────────────────────────┐
                         │         VISITOR         │
                         │                         │
                         │    Portfolio Website    │
                         └────────────┬────────────┘
                                      │
                                      │ REST API
                                      ▼
                         ┌─────────────────────────┐
                         │      ASP.NET CORE       │
                         │          API            │
                         │                         │
                         │  Controllers            │
                         │  Services               │
                         │  Business Logic         │
                         │  Authentication         │
                         └────────────┬────────────┘
                                      │
                                      │ EF Core
                                      ▼
                         ┌─────────────────────────┐
                         │       SQL SERVER        │
                         │                         │
                         │  Projects               │
                         │  Skills                 │
                         │  Experience             │
                         │  Education              │
                         │  Portfolio Content      │
                         └─────────────────────────┘


                         ┌─────────────────────────┐
                         │     ADMIN DASHBOARD     │
                         │                         │
                         │  Manage Projects        │
                         │  Manage Skills          │
                         │  Manage Experience      │
                         │  Manage Content         │
                         └────────────┬────────────┘
                                      │
                                      │ REST API
                                      ▼
                               ASP.NET Core API
```

---

# 🎨 Frontend

The frontend was designed to provide a modern and interactive user experience while keeping the interface clean and focused on presenting my professional profile and projects.

### ✨ Features

* Responsive design
* Modern dark / burgundy visual style
* Smooth scrolling
* Scroll-based animations
* Interactive sections
* Project showcase
* Skills and experience sections
* Mobile-friendly layout
* Micro-interactions

### 🛠️ Technologies

<p>
<img src="https://img.shields.io/badge/HTML5-E34F26?style=flat-square&logo=html5&logoColor=white"/>
<img src="https://img.shields.io/badge/CSS3-1572B6?style=flat-square&logo=css3&logoColor=white"/>
<img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black"/>
<img src="https://img.shields.io/badge/Bootstrap-7952B3?style=flat-square&logo=bootstrap&logoColor=white"/>
<img src="https://img.shields.io/badge/GSAP-88CE02?style=flat-square&logo=greensock&logoColor=111111"/>
<img src="https://img.shields.io/badge/Lenis-111111?style=flat-square"/>
</p>

---

# 🔌 Backend API

The portfolio is powered by a dedicated **ASP.NET Core Web API** that handles application data, business logic, and communication with the database.

### Backend responsibilities

* RESTful API endpoints
* CRUD operations
* Business logic
* Data access
* Authentication & authorization
* Portfolio content management
* Database communication
* Request validation

### 🛠️ Technologies

<p>
<img src="https://img.shields.io/badge/C%23-6B1F3A?style=flat-square&logo=csharp&logoColor=white"/>
<img src="https://img.shields.io/badge/.NET_8-512BD4?style=flat-square&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/ASP.NET_Core-6B1F3A?style=flat-square&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/REST_API-111111?style=flat-square"/>
</p>

---

# 🛠️ Admin Dashboard

The portfolio includes a dedicated administration dashboard for managing its content dynamically.

Instead of editing the source code whenever information changes, the dashboard provides a centralized interface for managing the portfolio.

### Dashboard capabilities

* Manage projects
* Manage technical skills
* Manage professional experience
* Manage education
* Manage services
* Manage portfolio content
* Update information dynamically

The dashboard communicates with the backend through the REST API.

---

# 🗄️ Database

The application uses **Microsoft SQL Server** for persistent data storage.

**Entity Framework Core** is used as the ORM to handle communication between the ASP.NET Core API and the database.

```text
Frontend
    │
    │ HTTP Requests
    ▼
ASP.NET Core Web API
    │
    │ Entity Framework Core
    ▼
SQL Server
```

---

# 🔐 Authentication & Authorization

Administrative functionality is protected using authentication and authorization mechanisms.

Protected API endpoints ensure that only authorized users can perform administrative operations such as creating, updating, or deleting portfolio content.

---

# 🧰 Tech Stack

### Frontend

<p>
<img src="https://img.shields.io/badge/HTML5-E34F26?style=flat-square&logo=html5&logoColor=white"/>
<img src="https://img.shields.io/badge/CSS3-1572B6?style=flat-square&logo=css3&logoColor=white"/>
<img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black"/>
<img src="https://img.shields.io/badge/Bootstrap-7952B3?style=flat-square&logo=bootstrap&logoColor=white"/>
<img src="https://img.shields.io/badge/GSAP-88CE02?style=flat-square&logo=greensock&logoColor=111111"/>
<img src="https://img.shields.io/badge/Lenis-111111?style=flat-square"/>
</p>

### Backend

<p>
<img src="https://img.shields.io/badge/C%23-6B1F3A?style=flat-square&logo=csharp&logoColor=white"/>
<img src="https://img.shields.io/badge/.NET_8-512BD4?style=flat-square&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/ASP.NET_Core-6B1F3A?style=flat-square&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/REST_API-111111?style=flat-square"/>
</p>

### Database & Tools

<p>
<img src="https://img.shields.io/badge/SQL_Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white"/>
<img src="https://img.shields.io/badge/Git-F05032?style=flat-square&logo=git&logoColor=white"/>
<img src="https://img.shields.io/badge/GitHub-111111?style=flat-square&logo=github&logoColor=white"/>
<img src="https://img.shields.io/badge/Postman-FF6C37?style=flat-square&logo=postman&logoColor=white"/>
</p>

---

# 📂 Project Structure

```text
naira-gamal-portfolio/
│
├── frontend/
│   ├── assets/
│   ├── css/
│   ├── js/
│   └── ...
│
├── backend/
│   ├── Controllers/
│   ├── Services/
│   ├── Models/
│   ├── Data/
│   └── ...
│
├── screenshots/
│   └── portfolio-desktop.JPG
│
└── README.md
```

---

# 📸 Screenshots

## Desktop

<p align="center">
  <img
    src="https://raw.githubusercontent.com/nairagamal/naira-gamal-portfolio/refs/heads/main/screenshots/portfolio-desktop.JPG"
    width="92%"
    alt="Portfolio Desktop Preview"
  />
</p>

---

## 📱 Responsive Design

The portfolio is designed to adapt across different screen sizes, providing a consistent experience on desktop, tablet, and mobile devices.

> Additional mobile and dashboard screenshots can be added here as the project evolves.

---

# 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/nairagamal/naira-gamal-portfolio.git
```

### 2. Frontend

Navigate to the frontend directory and run the project using your preferred local development server.

### 3. Backend

Open the backend project in Visual Studio and configure the required SQL Server connection string.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  }
}
```

Apply the database migrations:

```bash
dotnet ef database update
```

Then run the ASP.NET Core API.

### 4. Dashboard

Configure the API base URL for the dashboard and run the dashboard application.

---

# 🌐 Live Portfolio

<p align="center">

<a href="https://nairagamal.ngamal.workers.dev/">
<img src="https://img.shields.io/badge/EXPLORE_MY_PORTFOLIO-6B1F3A?style=for-the-badge&logo=googlechrome&logoColor=white"/>
</a>

</p>

---

# 👩‍💻 About Me

I'm **Naira Gamal**, a Full-Stack .NET Developer focused on building modern web applications and backend systems.

My primary technologies include:

**C# • ASP.NET Core • Web API • SQL Server • Entity Framework Core • JavaScript • Angular**

I enjoy working across the complete development cycle — from understanding requirements and designing databases to developing APIs, building responsive interfaces, and deploying applications.

---

# 📬 Connect With Me

<p align="center">

<a href="https://www.linkedin.com/in/naira-gamal/">
<img src="https://img.shields.io/badge/LinkedIn-6B1F3A?style=for-the-badge&logo=linkedin&logoColor=white"/>
</a>

<a href="https://github.com/nairagamal">
<img src="https://img.shields.io/badge/GitHub-111111?style=for-the-badge&logo=github&logoColor=white"/>
</a>

</p>

---

<div align="center">

### Built with passion & code.

**Naira Gamal © 2026**

</div>
