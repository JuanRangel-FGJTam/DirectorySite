jQuery(document).ready(function(){
    setTimeout( function(){
        $("#personSessionsWrapper").load(`/People/${currentPersonID}/sessions`);
        $("#personProceduresWrapper").load(`/People/${currentPersonID}/procedures`);
    }, 500);
});