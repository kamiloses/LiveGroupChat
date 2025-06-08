
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
