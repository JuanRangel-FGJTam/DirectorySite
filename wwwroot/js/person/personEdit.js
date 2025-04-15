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
        error: function(xhr, status, error)
        {
            var message = "Error al actualizar los datos";
            try
            {
                message = xhr.responseJSON.message;
            }
            catch (error)
            {
                //
            }

            if(xhr.status == 401)
            {
                message = "No authorizado"
            };
            
            Swal.fire({
                title: message,
                icon: "error"
            });
        }
    });
}

function submitContactForm(event){
    event.preventDefault();

    const form = $(this);
    const url = `/people/${currentPersonID}/contact`;
    const method = 'PATCH';
    
    $.ajax({
        url: url,
        type: method,
        data: form.serialize(),
        success: function(response) {
            Swal.fire({
                title: "Correo Actualizado",
                icon: "success"
            }).then(()=>{
                location.href = `/people/${currentPersonID}`;
            });
        },
        error: function(xhr, status, error)
        {
            var message = "Error al actualizar los datos";
            try
            {
                message = xhr.responseJSON.message;
            }
            catch (error)
            {
                //
            }

            if(xhr.status == 401)
            {
                message = "No authorizado"
            };

            Swal.fire({
                title: message,
                icon: "error"
            });
        }
    });
}

async function handleBanPersonSubmit(event)
{
    event.preventDefault();

    const form = $(this);
    const url = `/peopleBan/${currentPersonID}`;
    const method = 'PATCH';

    // * confirm for update
    var response = await Swal.fire({
        text: "Esta por modificar el estatus de la persona, ¿Desea continuar con esta acción?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Continuar",
        cancelButtonText: "Cancelar"
    });

    if(!response.isConfirmed)
    {
        return;
    }


    // * send the request
    $.ajax({
        url: url,
        type: method,
        data: form.serialize(),
        success: function(response)
        {
            Swal.fire({
                title: "Estatus de la persona actualizado",
                icon: "success"
            }).then(()=>{ location.href = `/people/${currentPersonID}`; });
        },
        error: function(xhr, status, error)
        {
            var message = "Error al actualizar los datos";
            try
            {
                message = xhr.responseJSON.message;
            }
            catch (error)
            {
                //
            }

            if(xhr.status == 401)
            {
                message = "No authorizado"
            };

            Swal.fire({
                title: message,
                icon: "error"
            });
        }
    });
}

jQuery(document).ready(function(){
    $('#generalDataForm').on('submit', submitGeneralDataForm);
    
    $('#contactForm').on('submit', submitContactForm);

    $('#banPersonForm').on('submit', handleBanPersonSubmit);
});