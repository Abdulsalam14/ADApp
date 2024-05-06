let inp = document.getElementById("nameinput");

let form = document.getElementById("movieform");


form.onsubmit = (event) => {

    event.preventDefault();
    if (inp.value === "") return;
    $.ajax({
        url: `Home/AddMessageToQueue?message=${inp.value}`,
        method: 'GET',
        success: function () {
            console.log("ok");
            form.reset();
            setTimeout(() => {
                $.ajax({
                    url: `Home/DeleteMessageFromQueue`,
                    success: function () {
                        console.log("Message deleted from queue");
                    },
                    error: function (err) {
                        console.log(err);
                    }
                })
            },7000)
        },
        error: function (xhr, status, error) {
            console.error(status, error);
        }
    });


}