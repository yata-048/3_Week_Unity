
**Project Title:** PARRY OR DIE!

**Event:** GCC - 3-Week Game Jam (Theme: ONE RULE)

**Tác giả:** Nguyễn Hoàng Dương

**Thể loại:** 2D Platformer Action / Boss Fight mini

**Engine:** Unity 2D (PC)

**I. TỔNG QUAN Ý TƯỞNG (OVERVIEW)**

- **Ý tưởng:** Trận đấu boss 2D góc nhìn ngang (Side-scrolling). Người chơi đối đầu với 1 con Boss duy nhất trên một sàn đấu khép kín.
- **The One Rule:** **Boss miễn nhiễm 100% với đòn đánh thường. Sát thương lên Boss CHỈ CÓ THỂ tạo ra khi Parry thành công đòn đánh của nó**.
- **Điểm nhấn gameplay:** Boss sở hữu các đòn đánh gồng delay khó chịu kiểu _Elden Ring_ (giơ vũ khí lên khựng lại thật lâu rồi mới bổ xuống chớp nhoáng), ép người chơi phải học thuộc nhịp vung kiếm thay vì bấm nút theo phản xạ hoảng loạn. (Nếu code được)

**II. GAMEPLAY & CƠ CHẾ CỐT LÕI**

**1. Game Loop**

1. Vào võ đài, Boss áp sát người chơi.
2. Boss vào animation gồng đòn.
3. Boss vung kiếm chém tới:

- **Bấm Parry chuẩn:** Triệt tiêu đòn, Boss dội sát thương và mất máu.
- **Bấm Parry hụt / Bấm sớm:** Người chơi ăn trọn đòn và mất máu.

5. Trận đấu kết thúc khi máu Boss về 0 (Thắng) hoặc máu Player về 0 (Thua).

**2. Bộ điều khiển Player (2D Platformer)**

- **A / D:** Di chuyển trái / phải.
- **Space:** Nhảy né đòn.
- **Shift:** Lướt (Dash) xuyên qua người Boss.
- **Chuột Phải:** Bật tư thế Parry (mở cửa sổ đỡ đòn trong ~0.2s).
- **Chuột Trái:** Chém thường (đánh trúng Boss chỉ nảy tia lửa vô hại).

**3. Cơ chế đòn đánh của Boss (Căn theo Frame Asset)**

Tận dụng animation sprite có sẵn tải từ itch.io, bóc tách animation đòn đánh của Boss thành 3 giai đoạn:

- **Frame gồng (Wind-up):** Sprite giơ kiếm lên cao, giữ frame này delay ngẫu nhiên/cố định để lừa người chơi.
- **Frame vung đòn (Active Hitbox):** Bật collider/hitbox chém xuống đúng 1-2 frame animation của sprite để quét va chạm trúng Player.
- **Frame hồi chiêu (Recovery):** Khựng lại trước khi chuyển sang chiêu tiếp theo.

**III. HỆ THỐNG KỸ THUẬT 

- **Interface FSM:** Quản lý các trạng thái tách biệt cho Player (Idle, Run, Jump, Dash, Parry, Hurt) và Boss (Chase, DelayWindUp, Attack, Hurt).
- **ScriptableObject:** Lưu các chỉ số máu, tốc độ di chuyển, thời gian mở cửa sổ Parry, và sát thương.
- **Raycast2D:**
- Bắn tia xuống dưới chân để kiểm tra tiếp đất (isGrounded) mượt mà cho Player.
- Bắn tia ngang để Boss nhận diện khoảng cách trước khi kích hoạt đòn đánh.

- **UnityEvent:** Bắn sự kiện trừ máu, chuyển cảnh Thắng/Thua, và kích hoạt hiệu ứng khi Parry thành công.
- **UI Canvas:** Thanh máu Boss dưới đáy màn hình, thanh máu Player góc trên, màn hình Pause và Victory/Defeat.

**IV. GAME FEEL & POLISH (LÀM KHI CÒN THỜI GIAN)**

- **Hitstop:** Dừng hình tích tắc khi Parry thành công để tạo độ nặng cho đòn đánh.
- **Camera Shake:** Rung chấn màn hình khi va chạm đao kiếm.
- **SFX & VFX:** Âm thanh kim loại _KENG!_ đanh thép, tia lửa bắn ra khi đỡ đòn thành công.
