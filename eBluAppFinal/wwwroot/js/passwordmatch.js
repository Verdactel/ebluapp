function validPassword() {
    if (document.getElementById('password').value ==
        document.getElementById('confirm_password').value) {
        document.getElementById('submit').disabled = false;
        document.getElementById('password_error').hidden = true;
    } else {
        document.getElementById('password_error').hidden = false;
        document.getElementById('submit').disabled = true;
    }
}