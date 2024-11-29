function submitGeneralDataForm(event){
    event.preventDefault();

    const form = $(this);
    const url = `/users/${currentUserID}/generals`;
    const method = form.attr('method');
    
    $.ajax({
        url: url,
        type: method,
        data: form.serialize(),
        success: function(response) {
            Swal.fire({
                title: "Datos actualizados",
                icon: "success"
            });
        },
        error: function(xhr, status, error) {
            console.error('Error:', error);
            Swal.fire({
                title: "Error al actualizar los datos",
                icon: "error"
            });
        }
    });
}

function submitCredentialsForm(event){
    event.preventDefault();

    const form = $(this);
    const url = `/users/${currentUserID}/credentials`;
    const method = form.attr('method');
    
    $.ajax({
        url: url,
        type: method,
        data: form.serialize(),
        success: function(response) {
            Swal.fire({
                title: "Datos actualizados",
                icon: "success"
            });
        },
        error: function(xhr, status, error) {
            console.dir('Error:', xhr);
            Swal.fire({
                title: `Error al actualizar los datos`,
                icon: "error",
                message: "Some message"
            });
        }
    });
}

jQuery(document).ready(function(){
    $('#generalDataForm').on('submit', submitGeneralDataForm);
    $('#credentialsForm').on('submit', submitCredentialsForm);
});