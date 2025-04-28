$(document).ready(function () {
    $('#myTable').DataTable({
        searching: true,
        ordering: true,
        paging: true,
        pageLength: 10
    });
});