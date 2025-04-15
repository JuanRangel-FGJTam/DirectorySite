function loadDataTable()
{
  var argsList = [];
  argsList.push(`page=${document.currentPage}`);
  var args = argsList.join('&');

  $("#tableWrapper").load(`/peopleban/table-records?${args}`);

  // * update the URL without reloading the page
  history.pushState(null, "", `?${args}`);
}

function goToPage(page)
{
    document.currentPage = Number(page);
    loadDataTable();
}

jQuery(document).ready(()=>
{
  loadDataTable();
});