## 🧩 **StaffMatrix Application Summary**

**StaffMatrix** is a **WPF (.NET Framework)** desktop application designed to simplify **employee management** and **shift scheduling** for small to mid-size teams.
It provides a clean, modern interface for supervisors to manage staff information, organize work schedules, and store operational data securely using **local JSON files** (no external database required).

---

### 🚀 **Core Features**

#### 👥 **Employee Management**

* Add, edit, and delete employee records via a user-friendly interface.
* Fields include: **First Name**, **Last Name**, **Role**, **Email**, and **Phone**.
* Each employee is assigned an **auto-generated unique ID**.
* Employee data is saved automatically to a local file (`Data/employees.json`).
* Data persists between sessions — employees are reloaded when the app starts.
* Validation ensures all required fields are completed before saving.

#### 🕒 **Shift Scheduling**

* Allows creation of shift records including **Employee Name**, **Shift Date**, **Start Time**, **End Time**, and **Location**.
* Shifts are displayed in a grid for easy viewing.
* Supports saving and loading shifts using a local file (`Data/shifts.json`).
* Designed to integrate with employee data, enabling assignment by employee ID.

#### 🔐 **Login System**

* Simple admin login screen using **hardcoded or JSON-based user credentials** (`users.json`).
* Supports multiple user roles such as **Admin** and **Scheduler**.

#### 🗓 **Calendar View**

* Interactive calendar (ShiftCalendarView) that displays shifts by date.
* Clicking a date shows assigned employees and times.

#### 🧮 **Data Persistence**

* All application data (employees, users, and shifts) is stored in **structured JSON format**.
* Eliminates the need for SQL Server — perfect for offline or standalone setups.
* Future-ready for migration to SQL if desired.

#### 🧰 **Architecture**

* Built using **WPF (XAML + C#)** with a modular folder structure:

  ```
  StaffMatrix
   ├── Data
   │    ├── JsonDataHelper.cs
   │    ├── employees.json
   │    ├── shifts.json
   │    ├── users.json
   ├── Models
   │    ├── Employee.cs
   │    ├── Shift.cs
   │    ├── User.cs
   ├── Views
   │    ├── EmployeeManagementView.xaml
   │    ├── ShiftSchedulingView.xaml
   │    ├── ShiftCalendarView.xaml
   │    ├── LoginWindow.xaml
   └── MainWindow.xaml
  ```

#### 💡 **UI Highlights**

* Sidebar navigation (Dashboard, Employee Management, Shifts, Reports, Settings).
* Clean, readable XAML layouts.
* Scrollable grids for large data lists.
* Real-time data binding using `ObservableCollection`.

---

### 🧠 **Technology Stack**

| Component      | Technology                 |
| -------------- | -------------------------- |
| Framework      | .NET Framework (WPF)       |
| Language       | C#                         |
| UI             | XAML                       |
| Data Storage   | JSON (via Newtonsoft.Json) |
| IDE            | Visual Studio              |
| Design Pattern | MVVM-ready architecture    |
| OS Support     | Windows 10/11 Desktop      |

---

### 🔄 **Future Enhancements (Planned)**

* 📅 Weekly/Monthly shift summary reports.
* 🔍 Search and filter for employees or shifts.
* 🖨 Printable reports.
* 💾 Optional SQL Server integration for enterprise deployment.
* 📤 Export to CSV or Excel.
* 🔔 Shift reminder and alert system.

---

### 🏁 **Summary**

**StaffMatrix** delivers a lightweight yet scalable workforce management solution that’s easy to deploy and maintain.
It’s ideal for supervisors or small businesses who need a **simple, reliable scheduling tool** with **offline capability** and **clean, modern UX** — no external database or setup headaches.

