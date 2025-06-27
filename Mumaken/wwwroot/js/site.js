// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function DataTableClientFun(controller, action, column, driverType) {

    debugger;

    var columns = [];
    columns.push({ "data": column[2], "name": column[2], "visible": false, "autoWidth": true });
    // fields
    columns.push({ "data": column[3], "name": column[3], "autoWidth": true });
    columns.push({
        "data": column[4],
        "name": column[4],
        "autoWidth": true,
        "createdCell": function (td, cellData, rowData, row, col) {
            $(td).attr('dir', 'ltr');
        }
    });
    //columns.push({ "data": column[2], "name": column[2], "autoWidth": true });
    columns.push({ "data": column[5], "name": column[5], "autoWidth": true });
    // GetImgProfile
    columns.push({
        "render": function (data, type, row, meta) { return `<img src="${row.imgProfile}" height="80" width="80px" />`; }
    });

    columns.push({ "data": column[7], "name": column[7], "autoWidth": true });
    //columns.push({ "data": column[6], "name": column[6], "autoWidth": true });
    columns.push({
        "data": column[8],
        "name": column[8],
        "autoWidth": true,
        "value": function (data, type, row, meta) {
            // Return the value to be used for sorting
            return data[column[8]]; // Assuming column[5] contains the property name
        }
    });
    // status
    columns.push({
        "render": function (data, type, row, meta) {
            if (row.isActive == true)
                return `<label id="${row.id}" style="color:green;font-size: 17px;">${localization.active}</label>`;
            else
                return `<label id="${row.id}" style="color:red;font-size: 17px;">${localization.inactive}</label>`;
        }
    });

    // change status
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.status}" onclick="Validation('${row.id}')" class="btn btn-primary btn-rounded" />`; }
    });

    // delete user
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.DeleteText}" onclick="DeleteUser('${row.id}')" class="btn btn-danger btn-rounded" />`; }
    });





    let isRtl = $('html').attr("lang");
    console.log(isRtl);
    let tableTanguage = {};

    let arTable = {
        paginate: {
            previous: `<i class="fa-solid fa-angles-left"></i>`,
            next: `<i class="fa-solid fa-angles-right"></i>`,
        },
        sProcessing: "جارٍ التحميل...",
        sLengthMenu: "أظهر _MENU_ مدخلات",
        sZeroRecords: "لم يعثر على أية سجلات",
        sInfo: "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
        sInfoEmpty: "يعرض 0 إلى 0 من أصل 0 سجل",
        sInfoFiltered: "(منتقاة من مجموع _MAX_ مُدخل)",
        sInfoPostFix: "",
    };

    let enTable = {
        paginate: {
            previous: `<i class="fa-solid fa-angles-left"></i>`,
            next: `<i class="fa-solid fa-angles-right"></i>`,
        },
        sLengthMenu: "Display _MENU_ records per page",
        sZeroRecords: "Nothing found - sorry",
        zInfo: "Showing page _PAGE_ of _PAGES_",
        sInfoEmpty: "No records available",
        sInfoFiltered: "(filtered from _MAX_ total records)",
    };
   

    if (isRtl == 'ar') {
        tableTanguage = arTable;
    } else {
        tableTanguage = enTable;
    }

    $("#datatable-responsive_clients").DataTable({
        "dom": 'Bflrtip',
        "buttons": ['copy', 'csv', 'excel', 'pdf', 'print'],
        //"lengthMenu": [
        //    [100, 250, 500, -1],
        //    [100, 250, 500, 'All']
        //],
        "lengthMenu": [10, 50, 100, 200],



        "language": tableTanguage,


        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": true, // for disable multiple column at once
        "ajax": {
            "url": `/${controller}/${action}`,
            "type": "POST",
            "datatype": "json",
            data: { driverType: driverType }, // Pass driverType as a parameter
        },
        //"ordering": true,
        //"order": [[0, 'asc']],
        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "visible": false,
        //        "searchable": false
        //    }],

        "columns": columns

    });



}
function DataTableDeliveryCompanyFun(controller, action, column) {

    debugger;

    var columns = [];

    // fields
    columns.push({ "data": column[1], "name": column[1], "autoWidth": true });
    columns.push({
        "data": column[2],
        "name": column[2],
        "autoWidth": true,
        "createdCell": function (td, cellData, rowData, row, col) {
            $(td).attr('dir', 'ltr');
        }
    });
    // GetImgProfile
    columns.push({
        "render": function (data, type, row, meta) { return `<img src="${row.imgProfile}" height="80" width="80px" />`; }
    });

    columns.push({ "data": column[4], "name": column[4], "autoWidth": true });
    columns.push({ "data": column[5], "name": column[5], "autoWidth": true });
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.Drivers}" onclick="getDriver('${row.id}')" class="btn btn-primary btn-rounded" />`; }
    });
    // status
    columns.push({
        "render": function (data, type, row, meta) {
            if (row.isActive == true)
                return `<label id="${row.id}" style="color:green;font-size: 17px;">${localization.active}</label>`;
            else
                return `<label id="${row.id}" style="color:red;font-size: 17px;">${localization.inactive}</label>`;
        }
    });

    // change status
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.status}" onclick="Validation('${row.id}')" class="btn btn-primary btn-rounded" />`; }
    });

    // delete user
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.DeleteText}" onclick="DeleteUser('${row.id}')" class="btn btn-danger btn-rounded" />`; }
    });






    let isRtl = $('html').attr("lang");
    console.log(isRtl);
    let tableTanguage = {};

    let arTable = {
        paginate: {
            previous: `<i class="fa fa-chevron-left" aria-hidden="true"></i>`,
            next: `<i class="fa fa-chevron-right" aria-hidden="true"></i>`,
        },
        sProcessing: "جارٍ التحميل...",
        sLengthMenu: "أظهر _MENU_ مدخلات",
        sZeroRecords: "لم يعثر على أية سجلات",
        sInfo: "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
        sInfoEmpty: "يعرض 0 إلى 0 من أصل 0 سجل",
        sInfoFiltered: "(منتقاة من مجموع _MAX_ مُدخل)",
        sInfoPostFix: "",
    };

    let enTable = {
        paginate: {
            previous: `<i class="fa fa-chevron-left" aria-hidden="true"></i>`,
            next: `<i class="fa fa-chevron-right" aria-hidden="true"></i>`,
        },
        sLengthMenu: "Display _MENU_ records per page",
        sZeroRecords: "Nothing found - sorry",
        zInfo: "Showing page _PAGE_ of _PAGES_",
        sInfoEmpty: "No records available",
        sInfoFiltered: "(filtered from _MAX_ total records)",
    };


    if (isRtl == 'ar') {
        tableTanguage = arTable;
    } else {
        tableTanguage = enTable;
    }



    $("#datatable-responsive_clients").DataTable({
        "dom": 'Bflrtip',
        "buttons": ['copy', 'csv', 'excel', 'pdf', 'print'],
        //"lengthMenu": [
        //    [100, 250, 500, -1],
        //    [100, 250, 500, 'All']
        //],
        "lengthMenu": [10, 50, 100, 200],


        "language": tableTanguage,


        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": true, // for disable multiple column at once
        "ajax": {
            "url": `/${controller}/${action}`,
            "type": "POST",
            "datatype": "json",
        },
        //"ordering": true,
        //"order": [[0, 'asc']],
        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "visible": false,
        //        "searchable": false
        //    }],

        "columns": columns

    });



}
function DataTableRentalCompanyFun(controller, action, column) {

    

    debugger;

    var columns = [];

    // fields
    columns.push({ "data": column[1], "name": column[1], "autoWidth": true });
    columns.push({
        "data": column[2],
        "name": column[2],
        "autoWidth": true,
        "createdCell": function (td, cellData, rowData, row, col) {
            $(td).attr('dir', 'ltr');
        }
    });
   /* columns.push({ "data": column[2], "name": column[2], "autoWidth": true });*/
    // GetImgProfile
    columns.push({
        "render": function (data, type, row, meta) { return `<img src="${row.imgProfile}" height="80" width="80px" />`; }
    });

    columns.push({ "data": column[4], "name": column[4], "autoWidth": true });
    columns.push({ "data": column[5], "name": column[5], "autoWidth": true });
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.Cars}" onclick="getCar('${row.id}')" class="btn btn-primary btn-rounded" />`; }
    });
    // status
    columns.push({
        "render": function (data, type, row, meta) {
            if (row.isActive == true)
                return `<label id="${row.id}" style="color:green;font-size: 17px;">${localization.active}</label>`;
            else
                return `<label id="${row.id}" style="color:red;font-size: 17px;">${localization.inactive}</label>`;
        }
    });

    // change status
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.status}" onclick="Validation('${row.id}')" class="btn btn-primary btn-rounded" />`; }
    });

    // delete user
    columns.push({
        "render": function (data, type, row, meta) { return `<input type="button" value="${localization.DeleteText}" onclick="DeleteUser('${row.id}')" class="btn btn-danger btn-rounded" />`; }
    });



    let isRtl = $('html').attr("lang");
    console.log(isRtl);
    let tableTanguage = {};

    let arTable = {
        paginate: {
            previous: `<i class="fa fa-chevron-left" aria-hidden="true"></i>`,
            next: `<i class="fa fa-chevron-right" aria-hidden="true"></i>`,
        },
        sProcessing: "جارٍ التحميل...",
        sLengthMenu: "أظهر _MENU_ مدخلات",
        sZeroRecords: "لم يعثر على أية سجلات",
        sInfo: "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
        sInfoEmpty: "يعرض 0 إلى 0 من أصل 0 سجل",
        sInfoFiltered: "(منتقاة من مجموع _MAX_ مُدخل)",
        sInfoPostFix: "",
    };

    let enTable = {
        paginate: {
            previous: `<i class="fa fa-chevron-left" aria-hidden="true"></i>`,
            next: `<i class="fa fa-chevron-right" aria-hidden="true"></i>`,
        },
        sLengthMenu: "Display _MENU_ records per page",
        sZeroRecords: "Nothing found - sorry",
        zInfo: "Showing page _PAGE_ of _PAGES_",
        sInfoEmpty: "No records available",
        sInfoFiltered: "(filtered from _MAX_ total records)",
    };


    if (isRtl == 'ar') {
        tableTanguage = arTable;
    } else {
        tableTanguage = enTable;
    }



    $("#datatable-responsive_clients").DataTable({
        "dom": 'Bflrtip',
        "buttons": ['copy', 'csv', 'excel', 'pdf', 'print'],
        //"lengthMenu": [
        //    [100, 250, 500, -1],
        //    [100, 250, 500, 'All']
        //],
        "lengthMenu": [10, 50, 100, 200],


        "language": tableTanguage,


        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": true, // for disable multiple column at once
        "ajax": {
            "url": `/${controller}/${action}`,
            "type": "POST",
            "datatype": "json",
        },
        //"ordering": true,
        //"order": [[0, 'asc']],
        //"columnDefs":
        //    [{
        //        "targets": [0],
        //        "visible": false,
        //        "searchable": false
        //    }],

        "columns": columns

    });



}
