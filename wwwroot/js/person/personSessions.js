
jQuery(document).ready(function(){
    setTimeout( function(){
        $("#personSessionsWrapper").load(`/People/${currentPersonID}/sessions`);
    }, 500);
});