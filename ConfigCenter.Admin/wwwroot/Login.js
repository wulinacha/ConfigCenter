function Submit() {
    var data = {};
    data.name = $("#username").val();
    data.password = $("#password").val();
    $.post('/Login', data, function (res) {
        if (res.success) {
            window.location.href = res.message;
        } else {
            alert(res.message);
        }
    });
}


window.onload = function () {

    document.onkeydown = function (ev) {
        var event = ev || event
        if (event.keyCode == 13) {
            Submit()
        }
    }
}