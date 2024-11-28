function refreshData(event, force)
{
    // * set on loading
    $("#tableWrapper").html(`
        <div class="d-flex align-items-center alert alert-primary mt-3" role="alert">
            <div class="spinner-border" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <span class="mx-2" style="font-size: 1rem;">Cargando datos</span>
        </div>
    `);

    // * make the request
    const settings = {
        "async": true,
        "url": "/preregister/table-records/refresh",
        "method": "POST"
    };
    $.ajax(settings).done(function (response) {
        // * display las update
        $("#lastUpdateLabel").text(response.lastUpdate);

        loadDataTable();
    });
}

function loadDataTable()
{
    $("#tableWrapper").load(`/Preregister/table-records`);
}

jQuery(document).ready(()=>
{
    $("#updateRecordsButton").click(refreshData);
    
    loadDataTable();
});