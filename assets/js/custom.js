// =============  Data Table - (Start) ================= //

$(document).ready(function(){
    
    var table = $('#example').DataTable({
        
        buttons:['copy', 'csv', 'excel', 'pdf', 'print']
        
    });
    
    
    table.buttons().container()
        .appendTo('#example_wrapper .col-md-6:eq(0)');

    var table2 = $('#table2').DataTable({
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
    });

    table2.buttons().container()
        .appendTo('#table2_wrapper .col-md-6:eq(0)');

    var table3 = $('#table3').DataTable({
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
    });

    table3.buttons().container()
        .appendTo('#table3_wrapper .col-md-6:eq(0)');
});

// =============  Data Table - (End) ================= //
