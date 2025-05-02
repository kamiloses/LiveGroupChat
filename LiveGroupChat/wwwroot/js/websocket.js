function connectToWebSocket() {
    console.log("Próba połączenia z SignalR...");
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.on("ReceiveMessage", (username, message) => {
        console.log(`${username}: ${message}`);

        const messageContainer = document.querySelector(".message-container");

        const messageDiv = document.createElement("div");
        messageDiv.classList.add("message", username === "Nowak" ? "left" : "right");

        messageDiv.innerHTML = `
            <div style="font-size: 0.9em; color: #ccc;">
                <strong>${escapeHtml(username)}</strong>
                <span style="float: right;">${new Date().toLocaleString('pl-PL')}</span>
            </div>
            <div class="message-content">
                ${escapeHtml(message)}
                <div class="reaction-button-wrapper">
                    <span class="reaction-button" onclick="toggleReactions(this)">➕</span>
                    <div class="reaction-popup">
                        ${['❤️', '😂', '👍', '😮', '👎'].map(emoji => `
                            <form method="post" action="/home/emoji" style="display:inline;">
                                <input type="hidden" name="Id" value="0" />
                                <input type="hidden" name="reaction" value="${emoji}" />
                                <button type="submit" style="background:none; border:none; font-size:1.3em; cursor:pointer;">${emoji}</button>
                            </form>
                        `).join('')}
                    </div>
                </div>
                <div class="reactions"></div>
            </div>
        `;

        messageContainer.appendChild(messageDiv);

        // Auto-scroll do dołu
        messageContainer.scrollTop = messageContainer.scrollHeight;
    });

    connection.start()
        .then(() => {
            console.log("Połączono z serwerem SignalR");
        })
        .catch(err => console.error("Błąd połączenia z SignalR: ", err));

    document.querySelector(".send-btn").addEventListener("click", function(e) {
        e.preventDefault();
        const textInput = document.querySelector('input[name="Text"]');
        const text = textInput.value.trim();
        const username = "Janek"; // Możesz to zmieniać dynamicznie jeśli potrzebujesz

        if (text === "") return;

        connection.invoke("SendMessage", username, text)
            .then(() => {
                console.log("Wysłano wiadomość");
                textInput.value = ""; // wyczyść input po wysłaniu
            })
            .catch(err => {
                console.error("Błąd wysyłania wiadomości: ", err);
            });
    });
}

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function toggleReactions(el) {
    const popup = el.nextElementSibling;
    document.querySelectorAll('.reaction-popup').forEach(p => {
        if (p !== popup) p.style.display = 'none';
    });
    popup.style.display = popup.style.display === "block" ? "none" : "block";
}

document.addEventListener('click', function (e) {
    if (!e.target.closest('.reaction-button-wrapper')) {
        document.querySelectorAll('.reaction-popup').forEach(p => p.style.display = 'none');
    }
});

window.onload = function () {
    connectToWebSocket();
};
