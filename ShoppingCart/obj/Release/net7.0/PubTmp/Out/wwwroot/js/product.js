

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        'ajax': { url: '/admin/product/getall' },
    
        "columns": [
        { data: 'title' },
        { data: 'isbn' },
        { data: 'listprice' },
        { data: 'author' },
        { data: 'category.name' }
    ]





    );

}

