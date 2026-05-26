# Transfer Booking Core API Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Move only Booking, cancel/confirm booking, Payment, Booking Management API, and Review/Rating API core from `BookingKhachSan` into `BookingKS`.

**Architecture:** `BookingKS` is a new API-only .NET solution with Domain/Application/Infrastructure/API projects. Replace placeholder classes with focused Domain entities, Application CQRS handlers, Infrastructure repositories/services, and API controllers copied/adapted from `BookingKhachSan`. Do not add `BookingHotel.Web` or MVC views.

**Tech Stack:** .NET 8, ASP.NET Core Web API, EF Core, MediatR, AutoMapper, FluentValidation, SQL Server, existing VNPay/MoMo service patterns.

---

## File Structure

Source project: `D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan`.
Target project: `D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS`.

Copy/adapt only these areas:

- Domain core:
  - `BookingHotel.Domain/Common/**`
  - `BookingHotel.Domain/Entities/Booking.cs`, `BookingDetail.cs`, `Payment.cs`, `Review.cs`, plus required `Hotel.cs`, `Room.cs`, `RoomType.cs`, `Users.cs`, `Role.cs`, `Voucher.cs`, `BookingVoucher.cs`, `Amenity*.cs`, image entities if referenced by DbContext/config.
  - `BookingHotel.Domain/Enums/BookingStatus.cs`, `PaymentStatus.cs`, `PaymentMethod.cs`, `ReviewRating.cs`, plus required enums referenced by copied entities.
  - `BookingHotel.Domain/Exceptions/**`, `Interfaces/**`, `ValueObjects/**` if referenced.
- Application core:
  - `BookingHotel.Application/Common/**`
  - `BookingHotel.Application/DTOs/Bookings/**`, `Payments/**`, `Reviews/**`, plus required DTOs used by mappings.
  - `BookingHotel.Application/Interfaces/Repositories/IRepositories.cs`
  - `BookingHotel.Application/Interfaces/Services/IServices.cs`
  - Booking commands/queries for create/cancel/confirm/check-in/check-out/history/details/manager-list/code lookup.
  - Payment commands/queries for process/update/status.
  - Review commands/queries for create/get hotel reviews.
  - `DependencyInjection.cs`, `Behaviours/**`, `Validators/**` if required by DI.
- Infrastructure core:
  - `BookingHotel.Infrastructure/Data/ApplicationDbContext.cs`, `SeedData.cs`
  - `BookingHotel.Infrastructure/Configurations/**` required by copied entities.
  - `BookingHotel.Infrastructure/Repositories/BookingRepository.cs`, `HotelRepository.cs`, `RoomRepository.cs`, `UserRepository.cs`
  - `BookingHotel.Infrastructure/Services/VNPayService.cs`, `MoMoService.cs`, `EmailService.cs`, `JwtService.cs`, `CurrentUserService.cs`, `FileService.cs` only if referenced by DI/interfaces.
  - `DependencyInjection.cs`
- API core:
  - `Controllers/BookingsController.cs`, `PaymentsController.cs`, `ReviewsController.cs`
  - Keep existing `AuthController.cs`, `HotelsController.cs` if required for compile.
  - `Extensions/**`, `Middlewares/**`, `Program.cs` only as needed to wire DI/MediatR/EF.

## Task 1: Copy core source files into BookingKS

**Files:** Copy listed file groups from source to target, overwriting placeholder classes in target.

- [ ] **Step 1: Copy Application and Domain core folders**

Run:
```bash
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Domain/Common" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Domain/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Domain/Entities" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Domain/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Domain/Enums" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Domain/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Domain/Exceptions" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Domain/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Domain/Interfaces" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Domain/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Domain/ValueObjects" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Domain/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/Common" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/DTOs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/Features" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/Interfaces" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/Behaviours" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/Validators" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/DependencyInjection.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/DependencyInjection.cs"
```
Expected: files copied, placeholders overwritten where path matches.

- [ ] **Step 2: Copy Infrastructure core files**

Run:
```bash
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/Configurations" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/Data" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/Repositories" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/Services" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/Interceptors" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/"
cp -r "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/Extensions" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/DependencyInjection.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/DependencyInjection.cs"
```
Expected: repository/service/DbContext code present in target.

- [ ] **Step 3: Copy API core files**

Run:
```bash
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/Controllers/BookingsController.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/Controllers/BookingsController.cs"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/Controllers/PaymentsController.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/Controllers/PaymentsController.cs"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/Controllers/ReviewsController.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/Controllers/ReviewsController.cs"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/Extensions/ServiceCollectionExtensions.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/Extensions/ServiceCollectionExtensions.cs"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/Extensions/ApplicationExtensions.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/Extensions/ApplicationExtensions.cs"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/Middlewares/JwtMiddleware.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/Middlewares/JwtMiddleware.cs"
cp "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/Middlewares/ExceptionMiddleware.cs" "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/Middlewares/ExceptionMiddleware.cs"
```
Expected: API exposes booking/payment/review endpoints.

## Task 2: Restore project references and package dependencies

**Files:**
- Modify: `BookingKS/BookingHotel.Application/BookingHotel.Application.csproj`
- Modify: `BookingKS/BookingHotel.Infrastructure/BookingHotel.Infrastructure.csproj`
- Modify: `BookingKS/BookingHotel.API/BookingHotel.API.csproj`

- [ ] **Step 1: Compare package references with source**

Run:
```bash
dotnet list "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Application/BookingHotel.Application.csproj" package
dotnet list "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Application/BookingHotel.Application.csproj" package
dotnet list "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.Infrastructure/BookingHotel.Infrastructure.csproj" package
dotnet list "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.Infrastructure/BookingHotel.Infrastructure.csproj" package
dotnet list "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKhachSan/BookingHotel.API/BookingHotel.API.csproj" package
dotnet list "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/BookingHotel.API.csproj" package
```
Expected: identify missing MediatR, AutoMapper, FluentValidation, EF Core, JWT, MailKit/Newtonsoft/etc.

- [ ] **Step 2: Copy missing package references from source csproj files**

Use `Read` on both source and target `.csproj` files, then edit target `.csproj` to match source package references needed by copied code.
Expected: target projects restore without missing package errors.

## Task 3: Build and fix compile errors inside API core scope

**Files:** Any copied core files in `BookingKS` only.

- [ ] **Step 1: Build target solution**

Run:
```bash
dotnet build "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingKhachSan.sln"
```
Expected: first run may fail from missing references or copied features outside API core.

- [ ] **Step 2: Remove or narrow copied non-core features if they cause compile errors**

If errors come from reports/vouchers/hotel approval/admin UI-only features, remove those feature folders or controllers from `BookingKS`. Keep only dependencies required for Booking, Payment, Review, Auth/Hotel/Room lookup.
Expected: no non-core compile blockers remain.

- [ ] **Step 3: Rebuild until green**

Run:
```bash
dotnet build "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingKhachSan.sln"
```
Expected: `Build succeeded` with 0 errors. Warnings are acceptable only if pre-existing package vulnerability/nullability warnings.

## Task 4: Smoke verify API core

**Files:** No code changes unless smoke finds compile/runtime issue.

- [ ] **Step 1: Start API**

Run:
```bash
dotnet run --project "D:/Dat/Vscode/HueCIT/Booking-Online/BookingKS/BookingHotel.API/BookingHotel.API.csproj" --urls http://localhost:5088
```
Expected: API starts and applies migrations/DI without crashing.

- [ ] **Step 2: Smoke public endpoints**

Run in another shell if allowed:
```bash
curl -i http://localhost:5088/swagger
curl -i http://localhost:5088/api/reviews/hotel/1
```
Expected: Swagger responds; reviews endpoint returns 200/empty JSON or controlled error, not 500 from DI.

- [ ] **Step 3: Stop API**

Stop background process after smoke.
Expected: no lingering dotnet process from this run.

## Self-Review

- Spec coverage: Booking create/cancel/confirm/check-in/check-out/history/detail/manager query, Payment process/update/status, Review create/list are covered by copied Application/API/Infrastructure tasks.
- Placeholder scan: no TBD placeholders in required steps.
- Type consistency: target project names stay `BookingHotel.*`; solution path is `BookingKS/BookingKhachSan.sln`.
