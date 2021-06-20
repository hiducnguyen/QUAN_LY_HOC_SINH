$(document).ready(function () {
    //  Apply multi-select framework for the multi-select box
    $('.multi_select').select2();
    
    //  Apply datatable framework for the tables
    $('.data_table').DataTable({
        "language": {
            "decimal": "",
            "info": "Hiển thị từ _START_ tới _END_ trong _TOTAL_ hàng",
            "infoEmpty": "Hiển thị từ 0 tới 0 trong 0 hàng",
            "infoPostFix": "",
            "thousands": ",",
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

    //  Triggered when deleteModal is about to be shown
    $('#deleteModal').on('show.bs.modal', (e) => {

        //  Get data-id attribute of the clicked element
        let id = $(e.relatedTarget).data('id');

        //  Populate the text box
        document.querySelector('#deleteModal input[name="id"]').innerText = id;
        document.querySelector('#deleteModal #id_to_delete').innerText = id;
    });
});