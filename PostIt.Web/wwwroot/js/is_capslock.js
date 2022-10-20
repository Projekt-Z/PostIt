let body = document.querySelector('input');
let warning = document.getElementById('caps')

body.addEventListener('keyup', (event) => {
    if(event.getModifierState('CapsLock')) {
        warning.innerText = "CapsLock is On"
    }
})