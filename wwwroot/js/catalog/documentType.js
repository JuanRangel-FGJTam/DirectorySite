/**
* @@param {MouseEvent} e
*/
function handleDeleteButtonClick(e)
{
    Swal.fire({
        title: "¿Desea continuar eliminando el catalogo?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Eliminar",
    }).then((result) =>
    {
        if (result.isConfirmed)
        {
            // * get the document type selected
            var documentTypeId = e.currentTarget.getAttribute("documenttype");

            $.ajax({
                url: `/catalog/document-types/${documentTypeId}`,
                type: 'delete',
                success: function(response) {
                    Swal.fire({ title: "Elemento eliminando", icon: "success" })
                    .then(()=>{
                        location.href = '/catalog/document-types';
                    });
                },
                error: function(xhr, status, error)
                {
                    var message = "Error al actualizar los datos";
                    try
                    {
                        message = xhr.responseText;
                    }
                    catch (error) { }

                    if(xhr.status == 401)
                    {
                        message = "No authorizado"
                    };
                    
                    Swal.fire({ title: message, icon: "error" });
                }
            });

        }
    });
}

$(document).ready(function(){
    $(".btn-del").click(handleDeleteButtonClick);
});