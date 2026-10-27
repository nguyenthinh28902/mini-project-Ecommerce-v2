document.addEventListener("DOMContentLoaded", function () {
    ["SortBy", "IsDescending"].forEach(function (id) {
        const element = document.getElementById(id);

        if (element && !element.tomselect) {
            new TomSelect(element, {
                create: false,
                maxOptions: 100,
                closeAfterSelect: true
            });
        }
    });
});
function initTableResizer(tableName) {
    const tables = document.querySelectorAll('.table-resizable');

    tables.forEach(function (table) {
        const cols = table.querySelectorAll('th');
        
        // Nếu có truyền tableName thì mới dùng Key để lưu localStorage
        const storageKey = tableName ? `table_width_${tableName}` : null;
        let savedWidths = {};

        // Chỉ đọc localStorage nếu có storageKey
        if (storageKey) {
            try {
                const stored = localStorage.getItem(storageKey);
                if (stored) {
                    savedWidths = JSON.parse(stored);
                }
            } catch (e) {
                console.error("Lỗi đọc localStorage cho table width:", e);
            }
        }

        cols.forEach(function (col, index) {
            const colKey = col.getAttribute('data-col-key') || `col_${index}`;
            
            // Khôi phục chiều rộng cũ nếu có lưu trong localStorage
            if (storageKey && savedWidths[colKey]) {
                col.style.width = `${savedWidths[colKey]}px`;
                col.style.minWidth = `${savedWidths[colKey]}px`;
            }

            // Tránh tạo trùng lặp resizer nếu hàm bị gọi lại nhiều lần
            if (col.querySelector('.resizer')) return;

            const resizer = document.createElement('div');
            resizer.classList.add('resizer');
            col.appendChild(resizer);

            let x = 0;
            let w = 0;

            const mouseDownHandler = function (e) {
                x = e.clientX;
                w = col.offsetWidth;
                resizer.classList.add('resizing');

                document.addEventListener('mousemove', mouseMoveHandler);
                document.addEventListener('mouseup', mouseUpHandler);
                e.preventDefault();
            };

            const mouseMoveHandler = function (e) {
                const dx = e.clientX - x;
                const newWidth = Math.max(35, w + dx);
                col.style.width = `${newWidth}px`;
                col.style.minWidth = `${newWidth}px`;
            };

            const mouseUpHandler = function () {
                resizer.classList.remove('resizing');
                document.removeEventListener('mousemove', mouseMoveHandler);
                document.removeEventListener('mouseup', mouseUpHandler);

                // CHỈ LƯU VÀO LOCALSTORAGE NẾU CÓ TRUYỀN tableName
                if (storageKey) {
                    const currentWidths = {};
                    cols.forEach((c, idx) => {
                        const key = c.getAttribute('data-col-key') || `col_${idx}`;
                        currentWidths[key] = c.offsetWidth;
                    });

                    try {
                        localStorage.setItem(storageKey, JSON.stringify(currentWidths));
                    } catch (e) {
                        console.error("Lỗi ghi localStorage cho table width:", e);
                    }
                }
            };

            resizer.addEventListener('mousedown', mouseDownHandler);
        });
    });
}
function resetTableResizer(tableName) {
    if (!tableName) {
        console.warn("Cần truyền vào tableName để xóa cấu hình kích thước cột!");
        return;
    }

    const storageKey = `table_width_${tableName}`;
    
    try {
        // 1. Xóa trong localStorage
        localStorage.removeItem(storageKey);
        
        // 2. Reset lại style trực tiếp trên các thẻ th của bảng đang hiển thị
        const tables = document.querySelectorAll('.table-resizable');
        tables.forEach(function (table) {
            const cols = table.querySelectorAll('th');
            cols.forEach(function (col) {
                col.style.width = '';
                col.style.minWidth = '';
            });
        });

        console.log(`Đã reset kích thước cột cho bảng: ${tableName}`);
    } catch (e) {
        console.error("Lỗi khi xóa cấu hình table width trong localStorage:", e);
    }
}
// 1. Chạy khi load trang lần đầu
document.addEventListener('DOMContentLoaded', function () {
    initTableResizer();
});