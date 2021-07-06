$(document).ready(function () {
    //  Apply multi-select framework for the multi-select box
    $('.multi_select').select2();

    //  Apply datatable framework for the tables
    if (culture === "vi") {
        $('.data_table').DataTable({
            "language": {
                "decimal": ",",
                "info": "Hiển thị từ _START_ tới _END_ trong _TOTAL_ hàng",
                "infoEmpty": "Hiển thị từ 0 tới 0 trong 0 hàng",
                "infoFiltered": "(lọc từ toàn bộ _MAX_ hàng)",
                "infoPostFix": "",
                "thousands": " ",
                "lengthMenu": "Hiển thị _MENU_ hàng",
                "loadingRecords": "Đang tải...",
                "processing": "Đang xử lý...",
                "search": "Tìm kiếm:",
                "zeroRecords": "Không tìm thấy kết quả nào",
                "paginate": {
                    "first": "Đầu tiên",
                    "last": "Cuối cùng",
                    "next": "Sau",
                    "previous": "Trước"
                }
            }
        });
    }
    else {
        $('.data_table').DataTable();
    }

    //  Triggered when deleteModal is about to be shown
    $('#deleteModal').on('show.bs.modal', (e) => {

        //  Get data-id attribute of the clicked element
        let id = $(e.relatedTarget).data('id');

        //  Populate the text box
        document.querySelector('#deleteModal input[name="id"]').value = id;
        document.querySelector('#deleteModal #id_to_delete').innerText = id;
    });
});
$.validator.methods.range = function (value, element, param) {
    let globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}