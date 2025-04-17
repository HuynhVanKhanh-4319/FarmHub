# FarmHub
🌾 FarmHub - Hệ thống quản lý nông nghiệp thông minh
# Người Thực Hiện: Huỳnh Văn Khanh
| Thành phần | Công nghệ |
|-----------|-----------|
| Backend | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Cơ sở dữ liệu | SQL Server |
| Authentication | Identity + Google OAuth |
| Biểu đồ | Chart.js |
| Lịch | FullCalendar.js |
| Giao diện | Bootstrap 5 |
| Quản lý người dùng | ASP.NET Identity |
## 🚀 Tính năng chính

### 👩‍🌾 Cho người dùng
- Quản lý **mùa vụ** (Season) với thông tin: tên mùa vụ, ngày bắt đầu, ngày thu hoạch, diện tích, ghi chú.
- Gắn mùa vụ với **giống cây trồng** (Crop).
- Quản lý **lịch trình canh tác** (Schedule) cho từng mùa vụ.
- Ghi nhận **sản lượng thu hoạch** (Report) với ghi chú.
- Xem biểu đồ sản lượng theo thời gian.
- Lịch hiển thị tất cả các hoạt động nông nghiệp.
-  Đăng nhập
- Hỗ trợ đăng nhập qua:
Tài khoản người dùng nội bộ (ASP.NET Identity),
Google
- Người dùng có thể:
Đặt lại mật khẩu qua email
Quản lý mùa vụ riêng của mình

### 🛠️ Cho Admin
- Dashboard thống kê:
  - Số mùa vụ đang diễn ra.
  - Sản lượng trung bình theo tháng/năm.
  - Biểu đồ sản lượng (Yield Chart).
  - **Biểu đồ mật độ lịch trình ** theo mùa vụ.
  - **Lịch toàn cục ** hiển thị toàn bộ hoạt động nông nghiệp của tất cả người dùng.
