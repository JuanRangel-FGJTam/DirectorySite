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
            })
            .then(()=>{
                location.href = `/people/${currentPersonID}`;
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
    // const url = `/people/${currentPersonID}/contact`;
    // const method = form.attr('method');
    
    // $.ajax({
    //     url: url,
    //     type: method,
    //     data: form.serialize(),
    //     success: function(response) {
    //         Swal.fire({
    //             title: "Datos actualizados",
    //             icon: "success"
    //         });
    //     },
    //     error: function(xhr, status, error) {
    //         console.error('Error:', error);
    //         Swal.fire({
    //             title: "Error al actualizar los datos",
    //             icon: "error"
    //         });
    //     }
    // });
}

jQuery(document).ready(function(){
    $('#generalDataForm').on('submit', submitGeneralDataForm);
    
    $('#contactForm').on('submit', submitContactForm);
});