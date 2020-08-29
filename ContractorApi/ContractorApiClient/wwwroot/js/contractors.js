var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/Contractor",
            "type": "GET",
            "dataType": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "10%" },
            { "data": "fullName", "width": "30%" },
            { "data": "type", "width": "5%" },
            { "data": "inn", "width": "10%" },
            { "data": "kpp", "width": "5%" }
            ,
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/ContractorList/Edit?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px; margin-bottom:5px'>
                            Edit
                        </a>
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/api/Contractor?id='+${data})>
                            Delete
                        </a>
                        </div>`;
                }, "width": "10%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}