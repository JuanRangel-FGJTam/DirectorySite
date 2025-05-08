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
    var args = [];
    args.push(`p=${document.currentPage}`);
    
    if (typeof document.filterSearch === 'string' && document.filterSearch.trim() !== "")
    {
        args.push(`search=${document.filterSearch}`);
    }

    if(document.filterStatus > 0)
    {
        args.push(`filter=${document.filterStatus}`);
    }
    $("#tableWrapper").load(`/Preregister/table-records?${args.join('&')}`);

    // * update the URL without reloading the page
    history.pushState(null, "", `?${args.join('&')}`);
}

function goToPage(page)
{
    document.currentPage = Number(page);
    loadDataTable();
}

jQuery(document).ready(()=> {
    $("#searchInput").on('change', (e) => {
        document.filterSearch = $("#searchInput").val();
        loadDataTable();
    });

    $("#updateRecordsButton").click(refreshData);
    loadDataTable();
});