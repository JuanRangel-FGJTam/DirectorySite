function submitGeneralDataForm(event){
    event.preventDefault();
      
    const form = $(this);
    const url = `/people/${currentPersonID}/generals`;
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

function submitContactForm(event){
    event.preventDefault();

    Swal.fire({
        title: "No implementado",
        icon: "warning"
    });
      
    // const form = $(this);
    // const url = form.attr('action');
    // const method = form.attr('method');
    
    // $.ajax({
    //     url: url,
    //     type: method,
    //     data: form.serialize(),
    //     success: function(response) {
    //         console.log('Success:', response);
    //         alert('Contact Data updated successfully!');
    //     },
    //     error: function(xhr, status, error) {
    //         console.error('Error:', error);
    //         alert('An error occurred. Please try again.');
    //     }
    // });
}

jQuery(document).ready(function(){
    
    $('#generalDataForm').on('submit', submitGeneralDataForm);
    
    $('#contactForm').on('submit', submitContactForm);

});