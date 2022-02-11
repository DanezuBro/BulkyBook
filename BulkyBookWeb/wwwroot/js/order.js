var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("pending");
        } else {
            if (url.includes("completed")) {
                loadDataTable("completed");
            } else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                } else {
                    loadDataTable("all");
                }
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "columns": [
            { "data": "id", "width": "5%", "className": "dt-body-center"},
            { "data": "name", "width": "25%", "className": "dt-body-center"},
            { "data": "phoneNumber", "width": "15%", "className": "dt-body-center"},
            { "data": "applicationUser.email", "width": "15%", "className": "dt-body-center"},
            { "data": "orderStatus", "width": "15%", "className": "dt-body-center" },
            { "data": "orderTotal", "width": "10%", "className": "dt-body-center" },
            {
                "data": "id",
                "width": "5%",
                "className": "dt-body-center",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                            <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-primary mx-2 btn-sm"><i class="bi bi-pencil-square"></i></a>
                        </div>
                                `
                }
            },

        ]
    });
}

