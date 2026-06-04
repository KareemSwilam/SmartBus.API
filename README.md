<div align="center">

# SmartBus API

### Bus Booking & Trip Management Platform

Built with **ASP.NET Core 9** · **Clean Architecture** · **SQL Server** ·**Fawaterek Payment**·

</div>

##  Core Business Features

###  Trip Management

* Create trips with validation for company ownership, assigned driver, bus, locations, and departure/arrival times.
* Support multi-stop routes through ordered trip stops.
* Configure segment-based pricing for partial journeys.
* Search trips by route and date with server-side pagination.
* Manage trip lifecycle:

```text
Scheduled → Boarding → In Progress → Completed
                                ↘
                                 Cancelled / Delayed
```

---

###  Seat Management

* Automatically generate seats when a bus is created.
* Segment-aware seat availability.
* Prevent overlapping seat reservations across route segments.
* Real-time availability checks before booking.

---

###  Booking System

* Validate trip, route segment, and seat availability.
* Dynamic fare calculation based on trip segment.
* Atomic booking creation with seat reservation.
* Support booking cancellation and refund requests.
* Company administrators receive refund notifications.

---

### Payment Integration (Fawaterek)

* Create payment invoices through Fawaterek.
* Secure webhook processing for payment confirmation.
* HMAC-SHA256 signature verification.
* Refund webhook handling.
* Retrieve available payment methods dynamically.

---

###  Ticket Generation

* Background ticket processing using Channel-based queues.
* PDF ticket generation with QuestPDF.
* QR code generation using ZXing.Net.
* Email delivery through MailKit SMTP.
* Ticket validation endpoint for boarding staff.

---

###  Company & Driver Management

* Company management with logo uploads.
* Driver management with license information and ratings.
* Company blocking and moderation by administrators.
* Self-service management for company administrators.

---

###  Authentication & Authorization

* JWT Authentication with Refresh Token rotation.
* Email confirmation workflow.
* OTP-based password reset.
* Role-based authorization:

```text
Admin
CompanyAdmin
Passenger
```

---

###  Real-Time Notifications

* SignalR-based real-time communication.
* Role-based notification groups.
* Asynchronous notification queue processing.
* Read/Unread tracking support.
* Deep-link navigation through Action URLs.

Notification events include:

* Booking Confirmed
* Refund Requested
* New Booking for Company Admin
* Ticket Generated


## Clean Architecture

The solution is organized into four projects, each with a clearly defined responsibility.

| Layer | Project | Responsibility |
|---------|---------|---------|
| Presentation | `SmartBus.API` | Controllers, Swagger, JWT Authentication, SignalR Hub Mapping |
| Application | `SmartBus.Application` | Business Logic, DTOs, Services, Mapster, Result Pattern, Background Task Queue |
| Domain | `SmartBus.Domain` | Entities, Enums, Repository Contracts, Value Objects |
| Infrastructure | `SmartBus.Infrastructure` | EF Core, ASP.NET Identity, Repositories, Payment Integration, Email Service, SignalR, Background Services |

### Dependency Rule

Dependencies only point inward:

```text
SmartBus.API
        ↓
SmartBus.Application
        ↓
SmartBus.Domain

SmartBus.Infrastructure
        ↓
SmartBus.Application
        ↓
SmartBus.Domain
```

##  Booking & Payment Flow

```text
Search Trips
      │
      ▼
Check Seat Availability
      │
      ▼
Create Booking (Pending)
      │
      ├─ Validate Trip & Seat
      ├─ Calculate Fare
      └─ Reserve Seat
      │
      ▼
Create Fawaterek Invoice
      │
      ▼
Passenger Completes Payment
      │
      ▼
Payment Webhook
      │
      ├─ Verify HMAC Signature
      ├─ Confirm Booking
      ├─ Store Invoice Data
      ├─ Queue Ticket Generation
      └─ Queue Notifications
      │
      ▼
Generate PDF Ticket
      │
      ▼
Send Ticket via Email
      │
      ▼
Push Real-Time Notification
```

### Booking Validation

Before creating a booking, the system validates:

* Trip existence
* Start and destination stops
* Seat ownership
* Segment availability
* Fare calculation

### Cancellation Flow

```text
Passenger Cancels Booking
            │
            ▼
      Is Booking Confirmed?
          │         │
         No        Yes
          │         │
          ▼         ▼
   Cancel Directly  Create Refund Request
                     │
                     ▼
             Notify Company Admin
                     │
                     ▼
          Fawaterek Processes Refund
                     │
                     ▼
             Refund Webhook Received
                     │
                     ▼
      Booking → Cancelled
      Refund → Completed
```

---

##  Trip Creation & Seat Reservation

### Trip Creation Rules

A trip can only be created when:

* Company exists
* Driver belongs to the company
* Bus belongs to the company
* Locations exist
* Start and destination locations are different
* Departure time is before arrival time

The system automatically creates:

* Start stop (`StopOrder = 1`)
* End stop (`StopOrder = 2`)

Additional stops can be added later through the TripStop API.

### Segment-Based Pricing

SmartBus supports pricing by route segment.

Examples:

```text
Cairo → Ismailia = 120 EGP
Ismailia → Port Said = 90 EGP
Cairo → Port Said = 180 EGP
```

The booking service automatically determines whether to use:

* Full trip price
* Segment price from StopSegments

### Seat Reservation Strategy

Seats are reserved per route segment rather than for the entire trip.

Example:

```text
Passenger A
Seat 5
Stops 1 → 3

Passenger B
Seat 5
Stops 3 → 5
```

Since the segments do not overlap, both bookings are valid.

This maximizes seat utilization while preventing double-booking.
##  Authentication & Authorization

### Authentication Features

- JWT Access Tokens
- Refresh Token Rotation
- Email Confirmation
- OTP-Based Password Reset
- ASP.NET Identity Integration

### Roles

| Role | Permissions |
|--------|--------|
| Admin | Full system access |
| CompanyAdmin | Manage buses, drivers, trips, refunds |
| Passenger | Search, book, pay, cancel, receive tickets |

---
## PDF Ticket Generation

Tickets are generated asynchronously after successful payment confirmation and delivered directly to the passenger via email.

### Technologies Used

- **QuestPDF** – PDF document generation
- **ZXing.Net** – QR code generation
- **SkiaSharp** – QR code image rendering
- **MailKit** – Email delivery with PDF attachment

### Ticket Contents

Each generated ticket includes:

- Passenger information
- Route details
- Departure and arrival times
- Bus and seat number
- Fare amount
- Unique QR code for validation

### Generation Pipeline

```text
Payment Confirmed
        │
        ▼
Queue Ticket Job
        │
        ▼
Generate QR Code
        │
        ▼
Build PDF Ticket
        │
        ▼
Attach PDF to Email
        │
        ▼
Send Ticket to Passenger
```

### Background Processing

1. Payment webhook confirms the booking.
2. The booking ID is added to the background task queue.
3. `TicketBackgroundService` processes the job asynchronously.
4. A QR code is generated from the booking identifier.
5. QuestPDF creates the ticket document.
6. The PDF is emailed to the passenger as an attachment.

##  Getting Started

### Prerequisites

Before running the project, ensure you have:

* .NET 9 SDK
* SQL Server (LocalDB, SQL Server, or Azure SQL)
* SMTP credentials for email delivery
* Fawaterek account credentials:

  * API Key
  * Provider Key

---

##  Configuration

Update `appsettings.json` with your environment-specific settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SmartBusDb;Trusted_Connection=True;"
  },
  "JWTConfig": {
    "Secret": "<your-jwt-secret>"
  },
  "FawaterekPayment": {
    "BaseURL": "https://api.fawaterk.com",
    "APIKey": "<your-api-key>",
    "ProviderKey": "<your-provider-key>"
  },
  "EmailSettings": {
    "Host": "smtp.example.com",
    "Port": 587,
    "UserName": "no-reply@example.com",
    "Password": "<password>"
  }
}
```

> **Note:** Never commit real secrets or API keys to source control.

---

##  Running the Application

### Clone the Repository

```bash
git clone https://github.com/KareemSwilam/SmartBus.API.git
cd SmartBus.API
```

### Apply Database Migrations

```bash
dotnet ef database update --project SmartBus.Infrastructure
```

### Run the API

```bash
dotnet run --project SmartBus.API
```

### Open Swagger

Navigate to:

```text
https://localhost:{port}/swagger
```

---

## 🧰 Technology Stack

* ASP.NET Core 9
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* JWT Authentication
* SignalR
* QuestPDF
* ZXing.Net
* SkiaSharp
* MailKit
* Fawaterek Payment Gateway
* Mapster
* Clean Architecture

---

<div align="center">

###  SmartBus API

Built with ASP.NET Core 9, Clean Architecture, and modern backend practices.

**Developed by Kareem Magdy Swelam**

</div>
