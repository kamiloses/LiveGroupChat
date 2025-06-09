
console.log("JS działa");
document.addEventListener('DOMContentLoaded', () => {
    let input = document.getElementById('username');
    let form = document.querySelector('form');
    let errorMessage = document.getElementById('error-message');

    form.addEventListener('submit', (e) => {
        const nickname = input.value.trim();

        if (nickname === '' || nickname.length < 3) {
            e.preventDefault();
            errorMessage.textContent='Nickname must be at least 3 characters long';
            console.log("DZIAŁA")
        }
        else errorMessage.textContent = '';
    });
});


const currentUserId = document.body.dataset.userId;

document.querySelectorAll(".message-left").forEach(msgEl => {
    const messageUserId = msgEl.getAttribute("data-user-id");

    if (messageUserId === currentUserId) {
        msgEl.classList.remove("message-left");
        msgEl.classList.add("message-right");
    }
});
