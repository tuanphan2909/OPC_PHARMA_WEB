let isLoading = false;
let interval; // Để lưu trữ interval

function showLoader() {
    const loader = document.querySelector(".loading-container");
    loader.style.display = "flex";
    resetProgress(); // Đặt lại tiến trình về 0% khi hiển thị loader
}

function hideLoader() {
    const loader = document.querySelector(".loading-container");
    loader.style.display = "none";
}

function resetProgress() {
    clearInterval(interval); // Xóa interval trước đó nếu có
    const progressBarFill = document.querySelector(".progress-bar-fill");
    const loadingText = document.querySelector(".loading-text");
    progressBarFill.style.width = `0%`;
    loadingText.textContent = `0%`;
}

function updateProgress(start, end, duration) {
    clearInterval(interval); // Xóa interval trước đó nếu có
    const progressBarFill = document.querySelector(".progress-bar-fill");
    const loadingText = document.querySelector(".loading-text");

    let current = start;
    const step = (end - start) / (duration * 10); // Sửa lại để tính đúng số bước

    interval = setInterval(() => {
        current += step;
        if (current >= end) {
            current = end;
            clearInterval(interval);
        }
        progressBarFill.style.width = `${current}%`;
        loadingText.textContent = `${Math.floor(current)}%`;
    }, 100);
}

async function fetchData(actionUrl, formData) {
    const response = await fetch(actionUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: new URLSearchParams(formData)
    });

    if (!response.ok) {
        throw new Error('Network response was not ok ' + response.statusText);
    }

    return response.text();
}

function processData(data) {
    return new Promise(resolve => {
        setTimeout(() => {
            resolve();
        }, 2000);
    });
}

async function loadDataWithLoader(actionUrl, formData, targetSelector) {
    if (isLoading) {
        return;
    }

    isLoading = true;
    showLoader();

    // Bắt đầu cập nhật tiến trình từ 0% đến 10% ngay lập tức
    updateProgress(0, 10, 0.5);

    try {
        const data = await fetchData(actionUrl, formData);
        updateProgress(10, 50, 2.5); // Đảm bảo cập nhật đúng thời gian

        await processData(data);
        updateProgress(50, 75, 2.5); // Đảm bảo cập nhật đúng thời gian

        setTimeout(() => {
            document.querySelector(targetSelector).innerHTML = data;

            // Kiểm tra xem DataTable đã được khởi tạo chưa
            if ($.fn.DataTable.isDataTable('#example')) {
                $('#example').DataTable().clear().destroy();
            }

            $('#example tbody').html(data);
            try {
                $('#example').DataTable({
                    dom: 'Bfrtip',
                    buttons: [
                        'copy', 'csv', 'excel', 'pdf', 'print'
                    ]
                });
            } catch (error) {
                console.error("Error initializing DataTable:", error);
                alert("Vui lòng chọn OKE"); // Thông báo tùy chỉnh của bạn
            }

            updateProgress(75, 100, 2.5); // Đảm bảo cập nhật đúng thời gian
            setTimeout(() => {
                hideLoader();
                isLoading = false;
            }, 500); // Thêm độ trễ nhỏ trước khi ẩn loader để đảm bảo nó hoàn thành 100%
        }, 1000);
    } catch (error) {
        console.error('Error:', error);
        hideLoader();
        isLoading = false;
    }
}

function runReport(event) {
    event.preventDefault();
    const actionUrl = document.getElementById("dateForm").action;
    const formData = new FormData(document.getElementById("dateForm"));
    resetProgress(); // Đặt lại tiến trình về 0% trước khi bắt đầu tải mới

    // Gọi hàm updateProgress ngay lập tức để tiến trình bắt đầu từ 0%
    updateProgress(0, 10, 0.5);

    // Tiếp tục với việc tải dữ liệu
    loadDataWithLoader(actionUrl, formData, '#example tbody');
}

document.addEventListener("DOMContentLoaded", function () {
    const checkboxes = document.querySelectorAll("input[type='checkbox'][name='columns']");
    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            toggleColumnVisibility(this.value, this.checked);
        });
    });

    function toggleColumnVisibility(columnClass, isVisible) {
        const thElements = document.querySelectorAll("th." + columnClass);
        const tdElements = document.querySelectorAll("td." + columnClass);

        thElements.forEach(function (thElement) {
            thElement.style.display = isVisible ? "" : "none";
        });

        tdElements.forEach(function (tdElement) {
            tdElement.style.display = isVisible ? "" : "none";
        });
    }

    checkboxes.forEach(function (checkbox) {
        toggleColumnVisibility(checkbox.value, checkbox.checked);
    });

    const dateForm = document.getElementById("dateForm");
    if (dateForm) {
        dateForm.removeEventListener("submit", runReport);
        dateForm.addEventListener("submit", runReport);
    }

    // Chặn lỗi mặc định của DataTables và thay thế bằng thông báo tùy chỉnh
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        alert("Đã xảy ra lỗi khi khởi tạo bảng. Vui lòng kiểm tra lại dữ liệu đầu vào và thử lại."); // Thông báo tùy chỉnh của bạn
        console.log("DataTables error: ", message);
    };

    // Initialize DataTable with Buttons
    $('#example').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        language: {
            emptyTable: "Không có dữ liệu nào trong bảng này",
            zeroRecords: "Không tìm thấy bản ghi nào",
            infoEmpty: "Không có bản ghi nào để hiển thị",
        }
    });
});